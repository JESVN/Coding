#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ResLoad
{
    /// <summary>
    /// AssetDatabase方式加载(需要文件后缀)，可加载Assets目录下的任何资源。仅Editor模式下有效
    /// </summary>
    public static T AssetDatabaseMethod<T>(string path, Transform parent = null) where T : Object
    {
#if UNITY_EDITOR
        return GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<T>(path), parent);
#else
        return null;
#endif
    }
}