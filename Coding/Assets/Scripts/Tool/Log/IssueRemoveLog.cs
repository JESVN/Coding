using UnityEngine;

public class IssueRemoveLog : MonoBehaviour
{
    void Awake()
    {
        if (GlobalLog.Instance)
            Destroy(gameObject);
    }
}

public class GlobalLog :MonoBehaviour
{
    public static GlobalLog Instance;
    static GlobalLog()
    {
        GameObject go = new GameObject("[issueRemoveLog]");
        DontDestroyOnLoad(go);
        Instance = go.AddComponent<GlobalLog>();
    }
    void Awake()
    {
#if !UNITY_EDITOR
        Debug.unityLogger.logEnabled = false;
#endif
        Debug.Log($"{Debug.unityLogger.logEnabled }");
    }
}
