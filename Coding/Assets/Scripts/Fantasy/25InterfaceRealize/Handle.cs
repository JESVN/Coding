using System;
using System.Collections;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
public class Handle : MonoBehaviour
{
    private int code=1;
    // Update is called once per frame
    IEnumerator Start()
    {
        while (true)
        {
            Notification(code.ToString());
            FindAllTypes(code.ToString());
            yield return new WaitForSeconds(1f);
        }
    }
    /// <summary>
    /// 没有继承MonoBehaviour的接口实现
    /// </summary>
    /// <param name="code"></param>
    void Notification(string code)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IReceive))&&!t.GetBaseClasses().Contains(typeof(MonoBehaviour))))
            .ToArray();
        foreach (var v in types)
        {
            ((IReceive)v.Assembly.CreateInstance(v.FullName)).Receive(code);
        }
    }
    /// <summary>
    /// 继承了MonoBehaviour的接口实现
    /// </summary>
    /// <param name="code"></param>
    public static void FindAllTypes(string code) 
    {
        var types = FindObjectsOfType<MonoBehaviour>().OfType<IReceive>();
        foreach (var t in types)
        {
            t.Receive(code);
        }
    }
}
