using System.Collections.Generic;
using UnityEngine;
public class FibonacciSequence : MonoBehaviour
{
    [SerializeField][Range(1,1000)] private int number;//项数
    private Dictionary<int, long> dynamicSum = new Dictionary<int, long>();//动态存储计算结果，下次直接来取，提高效率
    private long number1=0;//第一项默认值
    private long number2=1;//第二项默认值
    
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"第{number}项的值为：{CalculateRecursion(number)}");
            Debug.Log($"第{number}项的值为：{Calculate(number)}");
            Debug.Log($"第{number}项的值为：{F(number,number1,number2)}");
        }
    }
    /// <summary>
    /// 快速算法(将结果存储起来，以便下次来取，提高运行效率)
    /// </summary>
    /// <param name="count"></param>
    private long Calculate(int count,bool isLog=false)
    {
        if (dynamicSum.ContainsKey(count))
        {
            long value;
            dynamicSum.TryGetValue(count,out value);
            return value;
        }
        count = count <= 0 ? 1 : count;
        long firstItem = 0;
        long secondItem = 1;
        long sum=0;
        for (int i =1; i <=count; i++)
        {
            if (i == 1)
            {
                if(isLog)
                    Debug.Log($"第{i}项的值为：{firstItem}");
                sum = firstItem;
            }
            else if (i == 2)
            {
                if(isLog)
                    Debug.Log($"第{i}项的值为：{secondItem}");
                sum = secondItem;
            }
            else
            {
                if(isLog)
                    Debug.Log($"第{i}项的值为：{firstItem+secondItem}");
                sum = firstItem + secondItem;
                firstItem = secondItem;
                secondItem=sum;
            }
        }
        dynamicSum.Add(count,sum);
        return sum;
    }
    /// <summary>
    /// 普通递归算法(速度相当慢且理解起来有点困难(导致CalculateRecursion(n)重复计算。因为，每个值最终被拆解为 CalculateRecursion(1)+CalculateRecursion(2))详情请看网址:https://www.cnblogs.com/panzi/p/7852752.html)
    /// 项数40以上就会计算很缓慢了，60左右就直接卡死，不能使用这个方法
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    private long  CalculateRecursion(long  n)
    {
        if (n == 1)
            return 0;
        else if (n == 2)
            return 1;
        return CalculateRecursion(n - 1)+CalculateRecursion(n - 2);
    }
    /// <summary>
    /// 尾递归算法(实际运行效率会比普通递归高)
    /// </summary>
    /// <param name="n"></param>
    /// <param name="number1"></param>
    /// <param name="number2"></param>
    /// <returns></returns>
    private long F(long n,long number1,long number2)
    {
        return n == 1 ? number1 : F(n-1,number2,number1+number2);
    }
}
