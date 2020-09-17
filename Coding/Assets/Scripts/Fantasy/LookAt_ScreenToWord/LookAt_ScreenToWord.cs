using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt_ScreenToWord : MonoBehaviour
{
    [Range(0f,1920f)]
    [SerializeField]
    private float _width=1920f;
    [Range(0f,1080)]
    [SerializeField]
    private float _heigh=0;
    [SerializeField]
    private string _str = "点击屏幕以改变物体坐标";
    private GUIStyle _style=new GUIStyle();
    private GUIContent _guiContent=new GUIContent();
    // Update is called once per frame
    void Start()
    {
        _guiContent.text = _str;
        _style.fontSize = 30;
        _style.alignment = TextAnchor.UpperCenter;//GUI.Lable标签居中
        _style.normal.textColor = Color.green;
    }
    void Update()
    {
        var lookAtVec = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,transform.position.z));
        transform.LookAt(lookAtVec,Vector3.forward);
    }

    void OnGUI()
    {
        //自行编写水平居中代码
        //如：1.  _width/2-(_style.fontSize*_guiContent.text.Length)/2   公式为：当前选择宽度/2-(字体大小*内容长度)/2
        GUI.Label(new Rect( Screen.width/2,_heigh,0,0),_guiContent,_style);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //鼠标是屏幕坐标，所有要转成世界坐标，此时分为两种情况：
            //1.若摄像机是透视模式，则鼠标屏幕坐标转世界坐标时，要注意z轴，因为屏幕坐标无z轴，所以这里要将对象的世界坐标先转成屏幕坐标并且获取z轴值组成一个新的屏幕坐标，然后此坐标再转世界坐标即可，如下：
            if (!Camera.main.orthographic)//透视模式
            {
                transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
            }
            //2.若摄像机是正交模式，则无需转换对象坐标，直接转换鼠标屏幕到世界坐标即可(不过z轴的值还是需要赋值)，如下：
            else//正交模式
            {
                var wordPoint= Camera.main.ScreenToWorldPoint(Input.mousePosition);
                wordPoint.z=transform.position.z;
                transform.position = wordPoint;
            }
        }
    }
}
