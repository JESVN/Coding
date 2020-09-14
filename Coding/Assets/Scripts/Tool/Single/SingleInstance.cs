#region C#单例
/// <summary>
/// 带接口的单例，通常用于可MOCK的类
/// </summary>
/// <typeparam name="T">单例的类型</typeparam>
/// <typeparam name="I">单例类型的接口</typeparam>
public class SingleInstance<T, I> where T : I
{
    private static object lockObj = new object();
    private static T mySelf = default(T);
    public static I Instance
    {
        get
        {
#if DEBUG
            if (Mocking != null)
                return Mocking;
#endif
            lock (lockObj)
            {
                if (mySelf == null)
                {
                    mySelf = InstanceCreater.CreateInstance<T>();
                }
            }

            return mySelf;
        }
    }
#if DEBUG
    public static I Mocking { get; set; }
#endif
}
/// <summary>
/// 单例,需要在当前类中定义
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingleInstance<T> where T : class
{
    private static object lockObj = new object();
    private static T mySelf;//= default(T);
    public static T Instance
    {
        get
        {
            lock (lockObj)
            {
                if (mySelf == null)
                {
                    mySelf = InstanceCreater.CreateInstance<T>();
                }
            }

            return mySelf;
        }
    }
#if MOCK
        public static void InstanceClear()
        {
           mySelf=null;
        } 
#endif
}
static class InstanceCreater
{
    public static T CreateInstance<T>()
    {
        var type = typeof(T);
        try
        {
            return (T)type.Assembly.CreateInstance(type.FullName, true, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, null, null, null);
        }
        catch (System.MissingMethodException ex)
        {
            throw new System.Exception(string.Format("{0}(单例模式下，构造函数必须为private)", ex.Message));
        }
    }
}
#endregion
#region Unity单例
/// <summary>
/// Mono(Unity)单例
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : UnityEngine.MonoBehaviour where T : UnityEngine.MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                return null;
            }
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        UnityEngine.GameObject singleton = new UnityEngine.GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();
                        DontDestroyOnLoad(singleton);
                    }
                }
                return _instance;
            }
        }
    }
    private static bool applicationIsQuitting = false;
    public void OnDestroy()
    {
        applicationIsQuitting = true;
    }
}
#endregion
#region 线程安全的单例方法
public sealed class DL
{
    private static readonly DL instance = new DL();
    /// <summary>
    /// 显式的静态构造函数用来告诉C#编译器在其内容实例化之前不要标记其类型
    /// </summary>
    static DL() { }
    private DL() { }
    public static DL Instance
    {
        get
        {
            return instance;
        }
    }
}
#endregion