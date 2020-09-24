using UnityEngine;

public class IssueRemoveLog : MonoBehaviour
{
    public bool _isLog;

    void Awake()
    {
        if (FindObjectOfType<GlobalLog>()==null)
        {
            GlobalLog.Instance.Init(_isLog);
        }
    }
}

public class GlobalLog : MonoBehaviour
{
    public static GlobalLog Instance;
    static GlobalLog()
    {
        GameObject go = new GameObject("[issueRemoveLog]");
        DontDestroyOnLoad(go);
        Instance = go.AddComponent<GlobalLog>();
    }
    public void Init(bool IsLog)
    {
#if !UNITY_EDITOR
        Debug.unityLogger.logEnabled = false;
#else
        Debug.unityLogger.logEnabled = IsLog;
#endif
    }
}