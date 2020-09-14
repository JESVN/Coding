using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fantasy.Rechecking_Duplicate_ExpandingMethod_AnonymousMethod
{
    public class Rechecking : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            TestA("1", str=>str.Equals("1"));
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(2);
            list.Add(2);
            list.Add(2);
            list.Add(2);
            list.Add(2);
            list.Add(2);
            list.Add(2);
            list.Add(4);
            list.Add(5);
            list.Add(6);
            list.Add(6);
            list.Add(7);
            list.Add(3);
            GetRepetitionCount(list);
            Debug.Log($"{Duplicate(list)}");
            GetRepetition(list).Debug("重复的值为:");
        }
        /// <summary>
        /// 查询一个数组中是否存在重复数字
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private bool Duplicate<T>(List<T> list) where T : struct
        {
            return list.Distinct().ToList().Count.Equals(list.Count) ? false : true;
        }
        /// <summary>
        /// 输出一个数组中重复的项和重复次数
        /// </summary>
        /// <param name="sourceArray"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private void GetRepetitionCount<T>(List<T> sourceArray) where T : struct
        {
            sourceArray.Sort();
            foreach (var VARIABLE in sourceArray)
            {
                int count = 0;
                foreach (var VARIABLE2 in sourceArray)
                {
                    if (VARIABLE.Equals(VARIABLE2))
                    {
                        count++;
                    }
                }
                if (count>1)
                    Debug.Log($"{VARIABLE}值重复了{count}次");
            }
        }
        /// <summary>
        /// 获取一个数组中重复的项
        /// </summary>
        /// <param name="sourceArray"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private Array GetRepetition<T>(List<T> sourceArray) where T : struct
        {
            return GetRepetitionList(sourceArray).Distinct().ToList().ToArray();
        }
        /// <summary>
        /// 获取重复列表
        /// </summary>
        /// <param name="sourceArray"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<T> GetRepetitionList<T>(List<T> sourceArray) where T : struct
        {
            List<T> destinationArray = sourceArray.Distinct().ToList();
            foreach (var VARIABLE in destinationArray)
            {
                if (sourceArray.Contains(VARIABLE))
                {
                    sourceArray.Remove(VARIABLE);
                }
            }
            return sourceArray;
        }
        private void TestA(string str,Func<string,bool> match)
        {
            
            if (match == null)
                throw new Exception($"委托{match}不能为空");
            if (match(str))
                Debug.Log($"这个输出为true");
            else
                Debug.Log($"这个输出为false");
        }
    }

    /// <summary>
    /// Array拓展方法
    /// </summary>
    public static class ArrayExpanding
    {
        /// <summary>
        /// 遍历输出数组
        /// </summary>
        /// <param name="array"></param>
        public static void Debug(this Array array,string message=null)
        {
            foreach (var VARIABLE in array)
            {
                UnityEngine.Debug.Log($"{message}{VARIABLE}");
            }
        }
    }
}
