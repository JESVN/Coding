using System;
using System.Collections.Generic;
using System.IO;
namespace NetFamre.auto
{
    /// <summary>
    /// 数据包的操作
    /// </summary>
   public class LengthCoding
    {
        /// <summary>
        /// 防止粘包的操作
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] EnCode(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream();//实例化内存流
            BinaryWriter bw = new BinaryWriter(ms);//实例化写入
            bw.Write(buffer.Length);//写入长度
            bw.Write(buffer);//写入本体
            byte[] result = new byte[ms.Length];//新建一个字节数组
            Buffer.BlockCopy(ms.GetBuffer(),0, result, 0, result.Length);//将内存流中的字节数组复制给新的字节数组
            bw.Close();//关闭写入流
            ms.Close();//关闭内存流
            return result;//将新数组返回出去
        }
        /// <summary>
        /// 分包的操作
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] DeCode(ref List<byte> buffer)
        {          
            if(buffer.Count<4)//证明列表里面没有东西，无法解析分包
            {
                return null;
            }
            MemoryStream ms = new MemoryStream(buffer.ToArray());//实例化内存流
            BinaryReader br = new BinaryReader(ms);//实例化读取流
            int length = br.ReadInt32();//读取长度，判断是否能解析分包
            if(length>(int)(ms.Length-ms.Position))//证明还不足以解析分包
            {
                return null;
            }
            byte[] result=br.ReadBytes(length);//读取length长度的数据
            buffer.Clear();//清除列表
            buffer.AddRange(br.ReadBytes((int)(ms.Length-ms.Position)));// 将剩余没有读取到的东西添加进列表中，等下次再来读取
            br.Close();//关闭读取流
            ms.Close();//关闭内存流
            return result;//将新数组返回出去
        }
    }
}
