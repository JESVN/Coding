using System;
using System.Collections.Generic;
using UnityEngine;
using NetFamre.auto;

public class DataTest : MonoBehaviour
{
    public bool IsEnCode;
    public int _Type;
    public int _Area;
    public int _command;
    public string _data;
    public float lenght = 1024;
    private List<byte> cache = new List<byte>(); //消息缓存
    private byte[] buffer;//缓冲区的大小(默认1024)
    private int srcOffset; //偏移量
    private bool isReading = true; //是否能接收

    private delegate byte[] LengthDecode(ref List<byte> buffer); //分包处理委托

    private LengthDecode LD; //定义分包处理的委托

    // Start is called before the first frame update
    void Start()
    {
        LD = LengthCoding.DeCode;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnCode)
        {
            LD = LengthCoding.DeCode;
        }
        else
        {
            LD = null;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Clear();
            buffer = SetData((byte)_Type,(byte)_Area,(byte)_command,_data);
            Debug.Log($"大小：{buffer.Length}");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (buffer != null)
            {
                Send(buffer);
            }
        }
    }

    private void Clear()
    {
        cache.Clear();
        isReading = true;
        srcOffset = 0;
    }
    private byte[] SetData(byte Type, byte Area,byte command,string data)
    {
        SocketModel socketModel = new SocketModel();
        socketModel.Type = Type;
        socketModel.Area = Area;
        socketModel.command = command;
        Mess mess = new Mess(data);
        socketModel.message = mess;
        return IsEnCode ? LengthCoding.EnCode(MessageCoding.encode(socketModel)) : MessageCoding.encode(socketModel);
    }
    private void Send(byte[] buffer)
    {
        if (srcOffset >= buffer.Length)
        {
            Clear();
            Debug.Log($"{"数据已全部发送完毕且重置(可重新发送)"}");    
            return;
        }
        int realityLength = buffer.Length - srcOffset >= (int)lenght ? (int)lenght : buffer.Length - srcOffset;
        byte[] sendBuffer = new byte[realityLength];
        Buffer.BlockCopy(buffer, srcOffset, sendBuffer, 0, realityLength);
        srcOffset += realityLength;
        Debug.Log($"{srcOffset}");
        Receive(sendBuffer);
    }

    public void Receive(byte[] result)
    {
        cache.AddRange(result);
        if (isReading)
        {
            isReading = false;
            OnRead();
        }
    }

    public void OnRead()
    {
        byte[] buffer = null;
        if (LD != null) //假如存在分包现象
        {
            buffer = LD(ref cache); //执行分包
            if (buffer == null) //说明数据还不足以解析
            {
                isReading = true;
                return;
            }
        }
        else
        {
            if (cache.Count == 0)
            {
                isReading = true;
                return;
            }
            else
            {
                buffer = cache.ToArray();
                cache.Clear();
            }
        }
        SocketModel model = MessageCoding.decode(buffer);
        Debug.Log($"{model.Type},{model.Area},{model.command},{(model.message as Mess).message}");
        OnRead();
    }
}
[Serializable]
internal class Mess
{
    private string _message;

    public string message
    {
        private set { _message = value; }
        get { return _message; }
    }

    public Mess()
    {
    }

    public Mess(string message)
    {
        this.message = message;
    }
}