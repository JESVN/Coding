using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;
using DentedPixel;
public class TextControls : MonoBehaviour
{
    [Header("显示文本：")][SerializeField] private List<string> message;//显示文本
    [Header("控制文字显示速度：")][SerializeField] private float _showTime;//控制文字显示速度
    [Header("控制文字渐显速度：")][SerializeField] private float _showAlpha;//控制文字渐显速度
    
    private GameObject textAuto;
    private Transform _parent;
    private Coroutine _coroutineIELoadText;
    
    private int _parentCount;
    private int _parentIndex;
    private int _index;
    private int _messageIndex;
    private Color _color;
    private List<char> messagesList;
    // Start is called before the first frame update
    void Start()
    {
        if (message.Count == 0)
            return;
        _parent = transform.GetChild(0).GetChild(0);
        textAuto = Resources.Load<GameObject>("TextForithm/TextAuto");
        this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.Mouse0)).Subscribe(_ =>
        {
            if (_coroutineIELoadText!=null)
                StopCoroutine(_coroutineIELoadText);
            ResetLoad();
        });
    }
    /// <summary>
    /// 加载字体
    /// </summary>
    /// <returns></returns>
    private IEnumerator IELoadText()
    {
        while (true)
        {
            if (_parentCount > 1 && (_parentIndex <= _parentCount - 1))
            {
                _parent.GetChild(_parentIndex).gameObject.SetActive(true);
                _parent.GetChild(_parentIndex).GetComponent<Text>().text = messagesList[_index].ToString();
                 StartCoroutine(IEloadColor(_parent.GetChild(_parentIndex).transform));//不借助插件
                //LeanTween.textAlpha(_parent.GetChild(_parentIndex).GetComponent<RectTransform>(),1,_showAlpha);//插件实现
                _parentIndex++;
                _index++;
            }
            else
            {
                GameObject textGameObject=Instantiate(textAuto,_parent);
                if (textGameObject.GetComponent<Text>().color.a!=0)
                {
                    textGameObject.GetComponent<Text>().color=new Color(textGameObject.GetComponent<Text>().color.r,textGameObject.GetComponent<Text>().color.g,textGameObject.GetComponent<Text>().color.b,0);
                }
                textGameObject.GetComponent<Text>().text=messagesList[_index].ToString();
                StartCoroutine(IEloadColor(textGameObject.transform));//不借助插件
                //LeanTween.textAlpha(textGameObject.GetComponent<RectTransform>(),1,_showAlpha);//插件实现
                _index++;
            }
            if (_index.Equals(messagesList.Count))
                break;
            yield return new WaitForSeconds(_showTime);
        }
    }

    private IEnumerator IEloadColor(Transform colorGame)
    {
        Color a = colorGame.GetComponent<Text>().color;
        Color b=new Color(a.r,a.g,a.b,1);
        while (true)
        {
            colorGame.GetComponent<Text>().color =new Color(a.r,a.g,a.b,a.a+=_showAlpha);
            if (colorGame.GetComponent<Text>().color.a>=1)
                break;
            yield return null;
        }
    }
    /// <summary>
    /// 重新加载
    /// </summary>
    private void ResetLoad()
    {
        _parentCount = _parent.childCount;
        messagesList=message[_messageIndex].ToList();
        if (_messageIndex == message.Count - 1)
            _messageIndex = -1;
        _messageIndex++;
        _parentIndex = 1;
        _index = 0;
        for (int i = 1; i < _parent.childCount; i++)
        {
            _parent.GetChild(i).GetComponent<Text>().color=new Color(_parent.GetChild(i).GetComponent<Text>().color.r,_parent.GetChild(i).GetComponent<Text>().color.g,_parent.GetChild(i).GetComponent<Text>().color.b,0);
            _parent.GetChild(i).GetComponent<Text>().text = null;
            _parent.GetChild(i).gameObject.SetActive(false);
        }
        _coroutineIELoadText=StartCoroutine(IELoadText());
    }
}
