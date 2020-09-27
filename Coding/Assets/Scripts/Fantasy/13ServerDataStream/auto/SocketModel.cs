using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetFamre.auto
{
    [Serializable]//特性：此类可以被序列化
    /// <summary>
    /// 将消息传递过去
    /// </summary>
    public class SocketModel
    {
        /// <summary>
        /// 第一级  类型
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 第二级 区域码
        /// </summary>
        public byte Area { get; set; }

        /// <summary>
        /// 第三级 操作码
        /// </summary>
        public byte command { get; set; }

        /// <summary>
        /// 第四级 消息体
        /// </summary>
        public object message { get; set; }
        
        public T GetMessage<T>()
        {
            return (T)message;
        }
    }
}
