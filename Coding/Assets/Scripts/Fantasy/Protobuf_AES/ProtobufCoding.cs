using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Google.Protobuf;
using ProtobufProMsg;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;
using LitJson;
public class ProtobufCoding:MonoSingleton<ProtobufCoding>
{
    private enum EAes
    {
        /// <summary>
        /// 加密
        /// </summary>
        Encryption=0,
        /// <summary>
        /// 解密
        /// </summary>
        Decode=1,
    }
    //public GUISkin theSkin;
    // [Range(0,1)]
    // [SerializeField]
    // private float _value;
    // private Rect rctBloodBar;

    [SerializeField]private Slider _slider=null;
    [SerializeField]private Text _text=null;
    [SerializeField]private string _path=@"F:\新建文本文档.txt";
    [SerializeField]private string _protobufKey;
    [SerializeField]private string _jsonKey;
    private string _suffixProtobufKey=".protobufData";
    private string _suffixJsonKey=".jsonData";
    private string _keyAES;
    private
    // Start is called before the first frame update
    void Start()
    {
        //rctBloodBar = new Rect(0, Screen.height-30, Screen.width, Screen.height);
    }
    // void OnGUI()
    // {
    //     //垂直
    //      // GUI.VerticalScrollbar(rctBloodBar, 1.0f, _value, 0.0f, 1.0f, GUI.skin.GetStyle("verticalScrollbar"));
    //     //水平
    //      GUI.HorizontalScrollbar(rctBloodBar, 1.0f, _value, 1.0f, 0.0f, GUI.skin.GetStyle("horizontalscrollbar"));
    // }

    void Update()
    {
        Debug.Log($"{"[log]"}");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AESED(EAes.Encryption);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AESED(EAes.Decode);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Serialize();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Deserialization();
        }
    }
    /// <summary>
    /// 加密/解密文件
    /// </summary>
    /// <param name="eAes"></param>
    private void AESED(EAes eAes)
    {
        if(eAes==EAes.Encryption)
            Debug.Log($"{"开始加密文件"}");
        else
            Debug.Log($"{"开始解密文件"}");
        RockonTime(x =>
        {
            using (var fs=new FileStream(_path,FileMode.Open, FileAccess.ReadWrite))
            {
                var buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                if(eAes==EAes.Encryption)
                    _keyAES = AES.GetMD5Hash(buffer);//获取md5Key
                Debug.Log($"{buffer.Length}");
                ToolLoom.RunAsync(() =>
                {
                    byte[] buAesEncrypt=null;
                    if (eAes == EAes.Encryption)
                    {
                        SetText("正在加密文件");
                        buAesEncrypt = AES.AESEncrypt(buffer, _keyAES);
                    }
                    else
                    {
                        SetText("正在解密文件");
                        buAesEncrypt = AES.AESDecrypt(buffer, _keyAES);
                    }
                    if (buAesEncrypt != null)
                    {
                        SetText("正在写入数据");
                        Debug.Log($"{buAesEncrypt.Length}");
                        var Aesfs = File.Create(_path);
                        AES.AsynWrite(Aesfs,buAesEncrypt,1, progress =>
                        {
                            SetProgress(progress);
                            if (progress==1)
                            {
                                AES.FlieCloseOperation(Aesfs);
                                x.Stop();
                                if (eAes == EAes.Encryption)
                                {
                                    StopTime(x, "加密完成", false);
                                }
                                else
                                {
                                    StopTime(x, "解密完成", false);
                                }
                                SetText("操作完毕");
                            }
                        });
                    }
                    else
                    {
                        if(eAes==EAes.Encryption)
                            Debug.LogError($"{"加密有误"}");
                        else
                            Debug.LogError($"{"解密有误"}");
                        StopTime(x, "[error]");
                    }
                });
            }
        });
    }
    /// <summary>
    /// 反序列化
    /// </summary>
    private void Deserialization()
    {
        RockonTime(x =>
        {
            using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)+_protobufKey+_suffixProtobufKey,FileMode.Open))
            {
                var buffer=new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                //Debug.Log($"{buffer.Length}");
                //buffer=AES.AESDecrypt(buffer,_key);//解密
                var proMsg=ProMsg.Parser.ParseFrom(buffer);
                Debug.Log($"{proMsg.Code},{proMsg.Heat},{proMsg.Spo2},{proMsg.Pi},{proMsg.Vec},{proMsg.Message}");
                StopTime(x,"反序列化完成");
            }
            using (var fs = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)+_jsonKey+_suffixJsonKey,FileMode.Open))
            {
                var buffer=new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                //Debug.Log($"{buffer.Length}");
                //buffer=AES.AESDecrypt(buffer,_key);//解密
                var jsonMsg = JsonMapper.ToObject<Model>(Encoding.UTF8.GetString(buffer));
                Debug.Log($"{jsonMsg.code},{jsonMsg.heat},{jsonMsg.spo2},{jsonMsg.pi},{jsonMsg.vec},{jsonMsg.message}");
            }
            StopTime(x,"protobuf_json反序列化完成");
        });
    }
    /// <summary>
    ///序列化
    /// </summary>
    private void Serialize()
    {
        RockonTime(x =>
        {
            var proMsg=new ProMsg();
            proMsg.Code = 100;
            proMsg.Heat = 175;
            proMsg.Spo2 = 150;
            proMsg.Pi = 255;
            proMsg.Vec = 200;
            proMsg.Message = "protobuf消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送protobuf";
            var model=new Model();
            model.code = 100;
            model.heat = 175;
            model.spo2 = 150;
            model.pi = 255;
            model.vec = 200;
            model.message = "json消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送消息发送json";
            var bufferProMsg = proMsg.ToByteArray();//序列化
            var strModel = JsonMapper.ToJson(model);
            var buffermodel = Encoding.UTF8.GetBytes(strModel);
            _jsonKey=_protobufKey = AES.GetMD5Hash(bufferProMsg);//获取md5Key
            //bufferProMsg=AES.AESEncrypt(bufferProMsg,_key);//加密
            //Debug.Log($"{bufferProMsg.Length}");
            using ( var fs=new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) +_protobufKey+_suffixProtobufKey,FileMode.Create,FileAccess.Write))
            {
                fs.Write(bufferProMsg,0,bufferProMsg.Length);
            }
            using ( var fs=new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) +_jsonKey+_suffixJsonKey,FileMode.Create,FileAccess.Write))
            {
                fs.Write(buffermodel,0,buffermodel.Length);
            }
            StopTime(x,"protobuf_json序列化完成");
        });
    }
    /// <summary>
    /// 计时
    /// </summary>
    /// <param name="ElapsedMilliseconds"></param>
    private void RockonTime(Action<Stopwatch> stopWatch)
    {
        var watch = new Stopwatch();
        watch.Start();
        if (stopWatch is null)
            throw new Exception("[ElapsedMilliseconds]回调不能为空");
        stopWatch?.Invoke(watch);
    }
    /// <summary>
    /// 停止计时
    /// </summary>
    /// <param name="stopWatch"></param>
    /// <param name="message"></param>
    /// <param name="Istop"></param>
    /// <param name="IsLog"></param>
    private void StopTime(Stopwatch stopWatch,string message=null,bool Istop=true,bool IsLog=true)
    {
        if (Istop)
            stopWatch.Stop();
        if(IsLog)
            Debug.Log($"{message},用时:{stopWatch.ElapsedMilliseconds}ms,{stopWatch.ElapsedMilliseconds/1000f}s");
    }
    /// <summary>
    /// 通知进度条
    /// </summary>
    /// <param name="progress"></param>
    public void SetProgress(float progress)
    {
        ToolLoom.QueueOnMainThread(() =>
        {
            _slider.value = progress;
        });
    }
    /// <summary>
    /// 通知消息
    /// </summary>
    /// <param name="message"></param>
    public void SetText(string message)
    {
        ToolLoom.QueueOnMainThread(() =>
        {
            _text.text = message;
        });
    }
}
/// <summary>
/// Json序列化
/// </summary>
[Serializable]
public class Model
{
    //枚举操作
    public int code;
    //脉搏速率0
    public int heat;
    // 血氧浓度1
    public int spo2;
    // PI值2
    public int pi;
    // 波线图坐标3
    public  int vec;
    // 消息体
    public  string message;
    // 设备序列号
    public  string GetSerialNo;
    // 当前所在的控制名(场景或UI界面)
    public  string SceneUIName;
    // 当前播放的资源路径
    public  string pathName;
    // 当前播放的名字
    public  string Name;
    // 脱敏
    public  string TMName;
    // 当前播放的资源进度
    public  string mp3_mp4_Size;
    // 当前滚动条数值
    public  string ScrollBar;
    // 设备当前的电量(0-100)
    public  string GetElectric;
    // 设备当前的网络环境是否正常(正常-异常)
    public  string GetNetworkEnvironment;
    // 设备当前的地区(国家)
    public  string GetAtState;
    // 设备当前的Android版本号
    public  string GetAndroidVersions;
    //DP操作
    public int DPHandle;
}
