using System.Collections;
using UniRx;
using UnityEngine;
public class ItemZoomUniRx_Update : MonoBehaviour
{
    private float _zoomSize;//缩放大小
    private float _speed;//速度
    private float _min;//最小缩放
    private float _max;//最大缩放
    private Vector3 _zoom;
    private bool IsEx=true;
    void Awake()
    {
        RandomData();
    }

    void Start()
    {
        MainThreadDispatcher.StartUpdateMicroCoroutine(Zoom());
    }
    /// <summary>
    /// 缩放逻辑
    /// </summary>
    private IEnumerator Zoom()
    {
        while (IsEx)
        {
            transform.localScale += _zoom * Time.deltaTime * _speed;
            if (transform.localScale.x>_max&&_zoom.x.Equals(_zoomSize)||transform.localScale.x<_min&&_zoom.x.Equals(-_zoomSize))
            {
                _zoom=-_zoom;
            }
            yield return null;
        }
    }
    /// <summary>
    /// 随机数据生成
    /// </summary>
    private void RandomData()
    {
        _min=Random.Range(0.1f,0.6f);
        _max=Random.Range(0.7f,1.1f);
        _zoomSize=Random.Range(0.01f,0.03f);
        _speed = Random.Range(50f,120f);
        _zoom=new Vector3(_zoomSize,_zoomSize,_zoomSize);
        transform.localScale = new Vector3(_max, _max, _max);
    }

    void OnDestroy()
    {
        IsEx = false;
    }
}
