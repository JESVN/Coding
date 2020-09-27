using System;
using System.IO;
namespace NetFamre.auto
{
    /// <summary>
    /// 消息体的操作
    /// </summary>
    public class MessageCoding
    {
        /// <summary>
        /// 消息体的序列化
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static byte[] encode(SocketModel model)
        {
            MemoryStream ms = new MemoryStream();//实例化内存流
            BinaryWriter bw = new BinaryWriter(ms);//实例化写入流
            if (model != null)
            {
                bw.Write(model.Type);
                bw.Write(model.Area);
                bw.Write(model.command);
                if (model.message != null)
                {
                    bw.Write(SerializeUtil.serialize(model.message));
                }
            }
            byte[] result = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(),0,result,0,result.Length);
            bw.Close();
            ms.Close();
            return result;
        }
        /// <summary>
        /// 消息体的反序列化
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static SocketModel decode(byte[] buff)
        {
            MemoryStream ms = new MemoryStream(buff);            
            BinaryReader br = new BinaryReader(ms);
            SocketModel model = new SocketModel();
            model.Type=br.ReadByte();
            model.Area = br.ReadByte();
            model.command = br.ReadByte();
            byte[] result=br.ReadBytes((int)(ms.Length-ms.Position));
            if(result.Length!=0)
            {
                model.message=SerializeUtil.Derialize(result);
            }
            br.Close();
            ms.Close();
            return model;
        }
    }
}
