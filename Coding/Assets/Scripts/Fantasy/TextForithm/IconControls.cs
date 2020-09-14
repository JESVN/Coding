using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class IconControls : MonoBehaviour,IDragHandler,IBeginDragHandler
{
    // Start is called before the first frame update
    void Start()
    {
        AnimationEvent evt=new AnimationEvent();;
        evt.functionName = "AnimatotEvent";
        evt.stringParameter = "触发消息";
        evt.time = 0.5f;
        Animator animator = transform.GetComponent<Animator>();
        AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];
        clip.AddEvent(evt);
        Debug.Log($"{Screen.width/2},{Screen.height/2}");
        Debug.Log($"{transform.position}");
    }

    public void AnimatotEvent(string message)
    {
        Debug.Log($"{message}");
    }
    // Update is called once per frame
    void Update()
    {
        // Debug.Log($"{transform.GetComponent<RectTransform>().anchoredPosition}");
        // Debug.Log($"{Camera.main.ScreenToWorldPoint(transform.GetComponent<RectTransform>().anchoredPosition)}");
        // Debug.Log($"{Camera.main.ViewportToWorldPoint(transform.GetComponent<RectTransform>().anchoredPosition)}");
        //transform.position = Input.mousePosition;
        // Vector3 pos;
        // RectTransformUtility.ScreenPointToWorldPointInRectangle( transform.GetComponent<RectTransform>(), transform.GetComponent<RectTransform>().anchoredPosition, Camera.main, out pos);
        // Debug.Log($"{pos}");
    }
    Vector3 offset=Vector3.zero;
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition+offset;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset =transform.position-Input.mousePosition;
    }
}
