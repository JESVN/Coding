using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Move : MonoBehaviour
{
    [SerializeField] private float _speed;//移动速度
    [SerializeField] private List<Vector3> _targetList = new List<Vector3>();//移动位置
    // Start is called before the first frame update
    void Start()
    {
        _targetList.Insert(0,transform.position);
        if (_targetList.Count>1)
        {
            StartCoroutine(MovePingpong());
        }
    }
    /// <summary>
    /// 来回移动
    /// </summary>
    /// <returns></returns>
    private IEnumerator MovePingpong()
    {
        float distance;
        int index = 0;
        Vector3 targetPosition = _targetList[index];
        bool add=true;
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime); 
            distance =(transform.position-_targetList[index]).magnitude;
            if (distance == 0)
            {
                if (add)
                {
                    if (index < _targetList.Count - 1)
                    {
                        index++;
                    }
                    else
                    {
                        add = !add;
                        index--;
                    }
                }
                else
                {
                    if (index>0)
                    {
                        index--;
                    }
                    else
                    {
                        add = !add;
                        index++;
                    }
                }
                targetPosition = _targetList[index];
            }
            yield return null;
        }
    }
}
