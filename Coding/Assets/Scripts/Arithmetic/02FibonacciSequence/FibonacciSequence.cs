using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FibonacciSequence : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //0 1 1 2 3.....
        Calculate(15);
    }
    /// <summary>
    /// 普通算法
    /// </summary>
    /// <param name="count"></param>
    private void Calculate(int count)
    {
        count = count <= 0 ? 1 : count;
        int firstItem = 0;
        int secondItem = 1;
        for (int i =1; i <=count; i++)
        {
            if (i == 1)
            {
                Debug.Log($"第{i}项的值为：{firstItem}");
            }
            else if (i == 2)
            {
                Debug.Log($"第{i}项的值为：{secondItem}");
            }
            else
            {
                Debug.Log($"第{i}项的值为：{firstItem+secondItem}");
                int item = firstItem + secondItem;
                firstItem = secondItem;
                secondItem=item;
            }
        }
    }
    
    private long  CalculateRecursion(long  n)
    {
        if (n == 1 || n == 2)
        {
            return 1;
        }
        else if (n > 2)
        {
            long a = CalculateRecursion(n - 1);
            long b = CalculateRecursion(n - 2);
            return a + b;
        }
        else
        {
            return 0;
        }
    }
}
