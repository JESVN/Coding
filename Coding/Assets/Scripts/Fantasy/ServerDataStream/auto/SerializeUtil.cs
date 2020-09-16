using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace NetFamre.auto
{
    /// <summary>
    /// 序列化的工具
    /// </summary>
    public class SerializeUtil
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] serialize(object value)
        {
            MemoryStream ms = new MemoryStream();//实例化内存流
            BinaryFormatter bf = new BinaryFormatter();//实例化序列化的类
            bf.Serialize(ms,value);//将value序列化进内存流中           
            byte[] result = new byte[ms.Length];//新建一个字节数组用于存储
            Buffer.BlockCopy(ms.GetBuffer(), 0, result, 0, result.Length);//将内存流中的字节数组复制给新的字节数组
            ms.Close();//关闭内存流
            return result;//将新的字节数组返回出去
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static object Derialize(byte[] buff)
        {
            MemoryStream ms = new MemoryStream(buff);//实例化内存流,并将传进来的字节数组放进去
            BinaryFormatter bf = new BinaryFormatter();//实例化序列化的类
            object value = bf.Deserialize(ms);//反序列化
            ms.Close();//关闭内存流
            return value;//将反序列得到object类型返回出去
        }
        /// <summary>
        /// 获取想要得到的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public T GetType<T>(object value)
        {
            return (T)value;
        }
    }
}
