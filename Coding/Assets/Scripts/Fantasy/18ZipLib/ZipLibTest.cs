using UnityEngine;
using Updatezip;
using UnityEngine.UI;
public class ZipLibTest : MonoBehaviour
{
    [SerializeField]private Slider _progress;
    [SerializeField]private Text _message;
    [SerializeField]private Text pro;
    [SerializeField] private string _path=@"F:\VideoImage_File";
    // Start is called before the first frame update
    void Start()
    {
        EncodingGBK();
    }
    void OnGUI()
    {
         if (GUI.Button(new Rect(Screen.width / 2 - 150 / 2 - 100, 0, 150, 100), "压缩"))
         {
             YS( _path,@"F:\a.zip");
         }

        ;
        if (GUI.Button(new Rect(Screen.width / 2 - 150 / 2 + 100, 0, 150, 100), "解压"))
        {
            JY(@"F:\a.zip",@"F:\a\");
        }
    }
    /// <summary>
    /// 压缩
    /// </summary>
    /// <param name="yaar"></param>
    /// <param name="outarr"></param>
    private void YS(string yaar,string outarr)
    {
        string[] FileProperties = new string[2];  
        FileProperties[0] =yaar;//待压缩文件目录   
        FileProperties[1] =outarr; //压缩后的目标文件   
        ZipClass Zc = new ZipClass();  
        Zc.ZipFileMain(FileProperties,_progress,_message,pro);
    }
    /// <summary>
    /// 解压
    /// </summary>
    /// <param name="yaar"></param>
    /// <param name="outarr"></param>
    private void JY(string yaar,string outarr)
    {
        string[] FileProperties = new string[2];  
        FileProperties[0] = yaar;//待解压的文件   
        FileProperties[1] =outarr;//解压后放置的目标目录   
        UnZipClass UnZc = new UnZipClass();  
        UnZc.UnZip(FileProperties,_progress,_message,pro);
    }
    /// <summary>
    /// 解决压缩出现文件名中文乱码问题
    /// </summary>
    private void EncodingGBK()
    {
        var gbk = System.Text.Encoding.GetEncoding("gbk");
        ICSharpCode.SharpZipLib.Zip.ZipConstants.DefaultCodePage = gbk.CodePage; 
    }
}
