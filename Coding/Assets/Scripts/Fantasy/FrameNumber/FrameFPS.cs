using UnityEngine;
using UniRx;

public class FrameFPS : MonoBehaviour
{
    private float m_LastUpdateShowTime; //上一次更新帧率的时间;  

    private int m_FrameUpdate; //帧数;  

    private float m_FPS;
    
    private GUIStyle Style = new GUIStyle();

    [Header("是否显示小数点")] public bool IsDecimals;
    
    [Header("是否显示Fps")] public bool IsFps = true;
    
    [Header("帧数限制(-1表示无限制)")] [Range(-1, 100)]public int targetFrameRate = 60;
    
    [Header("更新帧率的时间间隔")] public float m_UpdateShowDeltaTime = 1f; //更新帧率的时间间隔;  
    
    void Awake()
    {
        Style.fontSize = 30;
        Style.normal.textColor = Color.green;
        Style.alignment = TextAnchor.UpperCenter;
    }

    // Use this for initialization  
    void Start()
    {
        m_LastUpdateShowTime = Time.realtimeSinceStartup;
        this.ObserveEveryValueChanged(self=>self.targetFrameRate).Subscribe(subdata =>
        {
            Debug.Log($"{subdata}");
            Application.targetFrameRate = subdata;
        });
    }

    // Update is called once per frame  
    void Update()
    {
        Frame();
    }
    /// <summary>
    /// 计算FPS
    /// </summary>
    private void Frame()
    {
        m_FrameUpdate++;
        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
        {
            m_FPS = m_FrameUpdate/(Time.realtimeSinceStartup - m_LastUpdateShowTime);
            m_FrameUpdate = 0;
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
        }
    }
    void OnGUI()
    {
        if (IsFps)
        {
            GUI.Label(new Rect(Screen.width / 2,0,0,0), IsDecimals?m_FPS.ToString("f2")+ "FPS":m_FPS.ToString("f0") + "FPS", Style);
        }
    }
}