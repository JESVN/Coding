using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 滚动字幕，鼠标拖动时停止滚动
/// </summary>
public class TextRoll : MonoBehaviour,IPointerDownHandler
{
    [SerializeField]private ScrollRect _scrollRect;//组件ScrollRect
    [SerializeField] private int _speed;//速度
    private bool _isRill=true;
    private Vector2 _reset;

    void Start()
    {
        _reset = _scrollRect.content.anchoredPosition;
        Debug.Log($"{_scrollRect.content.rect.size.y}");
    }
    void OnEnable()
    {
        _scrollRect.content.anchoredPosition = _reset;
    }
    void LateUpdate()
    {
        Debug.Log($"{_scrollRect.content.rect.size.y}");
        var content = _scrollRect.content.rect.size;//获取Content物体上的组件RectTransform的width和height
        var viewport = _scrollRect.viewport.rect.size;//获取Viewport物体上的组件RectTransform的width和height
        var len=content.y-viewport.y;//两者height之差
        var pos = _scrollRect.content.anchoredPosition;//获取Content物体上的组件RectTransform的PosX,PosY
        if (!_isRill&&Input.GetKeyUp(KeyCode.Mouse0))
            _isRill = true;
        if (pos.y >len||!_isRill)//posY初始坐标为0，若是往下滑动，则posY逐渐增大，最大值为父物体Viewport与子物体Content的height之差，即len
            return;
        pos.y+=_speed*Time.deltaTime;//自动滚动
        _scrollRect.content.anchoredPosition=pos;//赋值
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _isRill = false;
    }
}
