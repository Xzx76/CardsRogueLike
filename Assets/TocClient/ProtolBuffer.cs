using ProtoBuf;
using System;
using System.IO;

namespace TocClient
{
    public class ProtolBuffer
    {
        public static byte[] Serialize<T>(T model)
        {
            try
            {  //涉及格式转换，需要用到流，将二进制序列化到流中
                using (MemoryStream ms = new MemoryStream())
                { //使用ProtoBuf工具的序列化方法
                    Serializer.Serialize<T>(ms, model);
                    //定义二进制数组，保存序列化后的结果
                    byte[] resul = new byte[ms.Length];
                    //将流的位置设为0，起始点
                    ms.Position = 0;
                    //将流中的内容读取到二进制数组中
                    ms.Read(resul, 0, resul.Length);
                    return resul;
                }
            }
            catch (Exception)
            {
                //throw ex;
                return null;
            }
        }

        //将收到的消息反序列化成对象
        public static T DeSerialize<T>(byte[] msg)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {   //将消息写入流中
                    ms.Write(msg, 0, msg.Length);
                    //将流的位置归0
                    ms.Position = 0;
                    //使用工具反序列化对象
                    T resul = Serializer.Deserialize<T>(ms);
                    return resul;
                }

            }
            catch (Exception)
            {
                return default(T);
            }

        }

        //将收到的消息反序列化成对象
        public static object DeSerialize(Type type, byte[] msg)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {   //将消息写入流中
                    ms.Write(msg, 0, msg.Length);
                    //将流的位置归0
                    ms.Position = 0;
                    object res = Serializer.Deserialize(type, ms);
                    return res;
                }

            }
            catch (Exception)
            {
                return default;
            }

        }
    }
}
