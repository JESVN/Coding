using UnityEngine;
/// <summary>
/// 分帧输出
/// </summary>
public class MFrame : MonoBehaviour
{
    [Range(1,100)][SerializeField]private int _interval=1;//帧间隔
    private int[] _frames;//保持每一帧操作不一致
    
    //如：
    //1. _interval=3，则Time.frameCount % _interval表示每3帧执行一次相应的操作，
    //2. Time.frameCount % _interval=0和Time.frameCount % _interval=1是不同的帧，
    //3.(Time.frameCount % _interval)的范围就是0到 _interval-1之间
    
    // Start is called before the first frame update
    void Start()
    {
        _frames =new int[_interval];//范围是帧间隔大小
        for (int i = 0; i < _interval; i++)//添加值便可
        {
            _frames[i]=i;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % _interval==Get_frames(0))
        {
            Debug.Log($"第{Get_frames(0)}帧输出");   
        }
        if (Time.frameCount % _interval==Get_frames(1))
        {
            Debug.Log($"第{Get_frames(1)}帧输出");   
        }
        if (Time.frameCount % _interval==Get_frames(2))
        {
            Debug.Log($"第{Get_frames(2)}帧输出");   
        }
        if (Time.frameCount % _interval==Get_frames(3))
        {
            Debug.Log($"第{Get_frames(3)}帧输出");   
        }
    }
    private int Get_frames(int index)
    {
        return index >= _frames.Length ? _frames[index - 1] : _frames[index];
    }
}
