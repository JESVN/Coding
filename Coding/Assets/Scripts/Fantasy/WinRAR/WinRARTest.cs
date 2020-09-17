using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Updatezip;

public class WinRARTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            YS(@"F:\VideoImage_File\",@"F:\a.zip");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            JY(@"F:\a.zip",@"F:\a\");
        }
    }

    private void YS(string yaar,string outarr)
    {
        string[] FileProperties = new string[2];  
        FileProperties[0] =yaar;//待压缩文件目录   
        FileProperties[1] =outarr; //压缩后的目标文件   
        ZipClass Zc = new ZipClass();  
        Zc.ZipFileMain(FileProperties);
        Debug.Log($"{"压缩完成"}");    
    }
    private void JY(string yaar,string outarr)
    {
        string[] FileProperties = new string[2];  
        FileProperties[0] = yaar;//待解压的文件   
        FileProperties[1] =outarr;//解压后放置的目标目录   
        UnZipClass UnZc = new UnZipClass();  
        UnZc.UnZip(FileProperties); 
        Debug.Log($"{"解压完成"}");  
    }
}
