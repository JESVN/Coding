using System.Collections.Generic;
using Unity.IL2CPP.CompilerServices;
/// <summary>
/// Update统一管理
/// </summary>
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public class UpdateManager: MonoSingleton<UpdateManager>
{
    private HashSet<IUpdate> m_UpdateExList = new HashSet<IUpdate>();
    private HashSet<IFixedUpdate> m_FixedUpdateExList = new HashSet<IFixedUpdate>();
    private HashSet<ILateUpdate> m_LateUpdateExList = new HashSet<ILateUpdate>();
    /// <summary>
    /// 执行统一接口IUpdate
    /// </summary>
    private void Update()
    {
        IEnumerator<IUpdate> _Enumerator = m_UpdateExList.GetEnumerator();
        while (_Enumerator.MoveNext())
        {
            _Enumerator.Current.OnUpdate();
        }
    }
    /// <summary>
    /// 执行统一接口IFixedUpdate
    /// </summary>
    private void FixedUpdate()
    {
        IEnumerator<IFixedUpdate> _Enumerator = m_FixedUpdateExList.GetEnumerator();
        while (_Enumerator.MoveNext())
        {
            _Enumerator.Current.OnFixedUpdate();
        }
    }
    /// <summary>
    /// 执行统一接口ILateUpdate
    /// </summary>
    private void LateUpdate()
    {
        IEnumerator<ILateUpdate> _Enumerator = m_LateUpdateExList.GetEnumerator();
        while (_Enumerator.MoveNext())
        {
            _Enumerator.Current.OnLateUpdate();
        }
    }
    /// <summary>
    /// 添加Update事件
    /// </summary>
    /// <param name="_AddTarget"></param>
    public void AddUpdate(IUpdate key)
    {
        if (!m_UpdateExList.Contains(key))
        {
            m_UpdateExList.Add(key); 
        }
    }
    /// <summary>
    /// 移除Update事件
    /// </summary>
    /// <param name="_AddTarget"></param>
    public void RemoveUpdate(IUpdate key)
    {
        if (m_UpdateExList.Contains(key))
        {
            m_UpdateExList.Remove(key);
        }
    }
    /// <summary>
    /// 添加FixedUpdate事件
    /// </summary>
    /// <param name="_AddTarget"></param>
    public void AddFixedUpdate(IFixedUpdate key)
    {
        if (!m_FixedUpdateExList.Contains(key))
        {
            m_FixedUpdateExList.Add(key);
        }
    }
    /// <summary>
    /// 移除FixedUpdate事件
    /// </summary>
    /// <param name="_AddTarget"></param>
    public void RemoveFixedUpdate(IFixedUpdate key)
    {
        if (m_FixedUpdateExList.Contains(key))
        {
            m_FixedUpdateExList.Remove(key);
        }
    }
    /// <summary>
    /// 添加LateUpdate事件
    /// </summary>
    /// <param name="_AddTarget"></param>
    public void AddLateUpdate(ILateUpdate key)
    {
        if (!m_LateUpdateExList.Contains(key))
        {
            m_LateUpdateExList.Add(key);
        }
    }
    /// <summary>
    /// 移除LateUpdate事件
    /// </summary>
    /// <param name="_AddTarget"></param>
    public void RemoveLateUpdate(ILateUpdate key)
    {
        if (m_LateUpdateExList.Contains(key))
        {
            m_LateUpdateExList.Remove(key);
        }
    }
}
