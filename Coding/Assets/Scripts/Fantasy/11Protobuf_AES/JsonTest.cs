using System;
using System.IO;
using System.Text;
using UnityEngine;
using Random = System.Random;
public class JsonTest : MonoBehaviour
{
    private string path = @"F:\5.职场女性生理压力.mp4";
    void Start()
    {
        byte[] cgslBuffer= Encoding.UTF8.GetBytes("CGSL");
        Debug.Log($"{cgslBuffer.Length}");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToolLoom.RunAsync(() =>
            {
                using ( var fs=new FileStream(path,FileMode.Open,FileAccess.Read))
                {
                    Debug.Log($"原数据大小:{fs.Length}");
                    byte[] fileBuffer=new byte[fs.Length];
                    fs.Read(fileBuffer,0,fileBuffer.Length);
                    Debug.Log($"{fileBuffer.Length}");
                    byte[] cgslBuffer= Encoding.UTF8.GetBytes("CGSL");
                    Debug.Log($"{cgslBuffer.Length}");
                    byte[] newBuffer=new byte[fileBuffer.Length+cgslBuffer.Length];
                    cgslBuffer.CopyTo(newBuffer, 0);
                    fileBuffer.CopyTo(newBuffer, cgslBuffer.Length);
                    Debug.Log($"{newBuffer.Length}");
                    fs.Close();
                    fs.Dispose();
                    using (var fsCreat=new FileStream(path,FileMode.Create,FileAccess.ReadWrite))
                    {
                        fsCreat.Write(newBuffer,0,newBuffer.Length);
                        Debug.Log($"{"加密完成"}");
                    }
                }
            });
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToolLoom.RunAsync(() =>
            {
                using ( var fs=new FileStream(path,FileMode.Open,FileAccess.Read))
                {
                    Debug.Log($"原数据大小:{fs.Length}");
                    byte[] fileBuffer=new byte[fs.Length];
                    fs.Read(fileBuffer,0,fileBuffer.Length);
                    Debug.Log($"{fileBuffer.Length}");
                    byte[] newBuffer=new byte[fileBuffer.Length-4];
                    Array.Copy(fileBuffer,4,newBuffer,0,newBuffer.Length);
                    Debug.Log($"{newBuffer.Length}");
                    fs.Close();
                    fs.Dispose();
                    using (var fsCreat=new FileStream(path,FileMode.Create,FileAccess.ReadWrite))
                    {
                        fsCreat.Write(newBuffer,0,newBuffer.Length);
                        Debug.Log($"{"解密完成"}");
                    }
                }
            });
        }
    }
}
