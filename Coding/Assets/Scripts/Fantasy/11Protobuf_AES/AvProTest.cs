using System.IO;
using UnityEngine;
using RenderHeads.Media.AVProVideo;
public class AvProTest : MonoBehaviour
{
    private string path = @"F:\5.职场女性生理压力.mp4";
    private MediaPlayer mediaPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        mediaPlayer = GetComponent<MediaPlayer>();
    }
    void Start()
    {
        mediaPlayer = GetComponent<MediaPlayer>();
        //var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        // Debug.Log($"原数据大小:{fs.Length}");
        // byte[] fileBuffer=new byte[fs.Length];
        // fs.Read(fileBuffer,0,fileBuffer.Length);
        // fs.Close();
        // fs.Dispose();
        mediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToDataFolder,path,false);
        mediaPlayer.Play();
    }
}
