using System.Collections;
using UnityEngine;
public class Circle : MonoBehaviour
{
    /// <summary> 需要被实例化的对象 </summary>
    [SerializeField] private GameObject obj;

    /// <summary> 生成数量 </summary>
    [SerializeField] private int iconCount = 100;

    /// <summary> 圆半径 </summary>
    [SerializeField] private float fRadius = 20;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreatCircle());
    }
    /// <summary>
    /// 围绕一个点生成圆
    /// </summary>
    public IEnumerator CreatCircle()
    {
        float angle = 360f / iconCount;

        for (int i = 0; i < iconCount; i++)
        {
            GameObject go = null;
            if (obj != null)
                go = Instantiate(obj);
            else
                go =  GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.SetParent(transform, false);

            float x = fRadius * Mathf.Sin((angle * i) * (Mathf.PI / 180f));

            float y = fRadius * Mathf.Cos((angle * i) * (Mathf.PI / 180f));

            go.transform.localPosition = new Vector3(x, y, 0);

            go.transform.localEulerAngles = new Vector3(0, 0, Mathf.Abs(angle * i - 360));

            go.name = i.ToString();
            
            yield return new WaitForSeconds(0.05f);
        }
    }
}