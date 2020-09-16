using UnityEngine;
public class ItemZoomOptimizeUpdates_Update : MonoBehaviour,IUpdate
{
    private float _zoomSize;//缩放大小
    private float _speed;//速度
    private float _min;//最小缩放
    private float _max;//最大缩放
    private Vector3 _zoom;
    void Awake()
    {
        RandomData();
        UpdateManager.Instance.AddUpdate(this);
    }
    /// <summary>
    /// 缩放逻辑
    /// </summary>
    private void Zoom()
    {
        transform.localScale += _zoom * Time.deltaTime * _speed;
        if (transform.localScale.x>_max&&_zoom.x.Equals(_zoomSize)||transform.localScale.x<_min&&_zoom.x.Equals(-_zoomSize))
        {
            _zoom=-_zoom;
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

    public void OnUpdate()
    {
        Zoom();
    }

    void OnDestroy()
    {
        if (UpdateManager.Instance!=null)
        {
            UpdateManager.Instance.RemoveUpdate(this);
        }
    }
}
