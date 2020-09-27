using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace Utils
{
    /// <summary>
    /// AES加密解密(不能被继承)
    /// </summary>
    public sealed class AES
    {
        /// <summary>
        /// 获取唯一ID(字符串)
        /// </summary>
        /// <returns></returns>
        public static string GenerateId()  
        {  
            long i = 1;  
            foreach (byte b in System.Guid.NewGuid().ToByteArray())  
            {  
                i *= ((int)b + 1);  
            }  
            return string.Format("{0:x}", i - System.DateTime.Now.Ticks);  
        } 
        #region MD5值获取，根据Byte[]
        /// <summary>
        /// 获取MD5值(根据Byte[])
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetMD5Hash(byte[] buffer)
        {
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retval = md5.ComputeHash(buffer);//生成MD5值
                StringBuilder sb = new StringBuilder();
                foreach (var r in retval)
                {
                    sb.Append(r.ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                throw new Exception("GetMD5Hash Failed!" + e.Message);
            }
        }
        #endregion
        #region MD5值获取，根据FilePath
        /// <summary>
        /// 获取MD5值(根据FilePath)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string path)
        {
            try
            {
                FileStream fs=new FileStream(path,FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retval = md5.ComputeHash(fs);//生成MD5值
                fs.Close();
                StringBuilder sb = new StringBuilder();
                foreach (var r in retval)
                {
                    sb.Append(r.ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                throw new Exception("GetMD5Hash Failed!" + e.Message);
            }
        }
        #endregion
        #region 加密
        #region 加密字符串
        /// <summary>
        /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="EncryptString">待加密密文</param>
        /// <param name="EncryptKey">加密密钥</param>
        public static string AESEncrypt(string EncryptString, string EncryptKey)
        {
            return Convert.ToBase64String(AESEncrypt(Encoding.Default.GetBytes(EncryptString), EncryptKey));
        }
        #endregion
        #region 加密字节数组
        /// <summary>
        /// AES 加密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="EncryptByte">加密数组</param>
        /// <param name="EncryptKey">加密密匙</param>
        /// <param name="coefficient">加密写入系数coefficient*1024(默认coefficient=1)</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="CryptographicException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] AESEncrypt(byte[] EncryptByte, string EncryptKey,int coefficient=1)
        {
            if (EncryptByte.Length == 0) { throw (new Exception("明文不得为空")); }
            if (string.IsNullOrEmpty(EncryptKey)) { throw (new Exception("密钥不得为空")); }
            byte[] m_strEncrypt = null;
            byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
            byte[] m_salt = Convert.FromBase64String("gsf4jvkyhye5/d7k8OrLgM==");
            Rijndael m_AESProvider = Rijndael.Create();
            try
            {
                MemoryStream m_stream = new MemoryStream();
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(EncryptKey, m_salt);
                ICryptoTransform transform = m_AESProvider.CreateEncryptor(pdb.GetBytes(32), m_btIV);
                CryptoStream m_csstream = new CryptoStream(m_stream, transform, CryptoStreamMode.Write);
                AsynWrite(m_csstream,EncryptByte,coefficient, progress =>
                {
                    ToolLoom.QueueOnMainThread(() =>
                    {
                        ProtobufCoding.Instance.SetProgress(progress);
                    });
                    if (progress == 1)
                    {
                        m_csstream.FlushFinalBlock();
                        m_strEncrypt = m_stream.ToArray();
                        m_stream.Close(); 
                        m_stream.Dispose();
                        m_csstream.Close();
                        m_csstream.Dispose();
                    }
                });
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_AESProvider.Clear(); }
            return m_strEncrypt;
        }
        #endregion
        #endregion
        #region 解密
        #region 解密字符串
        /// <summary>
        /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="DecryptKey">解密密钥</param>
        public static string AESDecrypt(string DecryptString, string DecryptKey)
        {
            return Convert.ToBase64String(AESDecrypt(Encoding.Default.GetBytes(DecryptString), DecryptKey));
        }
        #endregion
 
        #region 解密字节数组
        /// <summary>
        /// AES 解密(高级加密标准，是下一代的加密算法标准，速度快，安全级别高，目前 AES 标准的一个实现是 Rijndael 算法)
        /// </summary>
        /// <param name="DecryptString">待解密密文</param>
        /// <param name="DecryptKey">解密密钥</param>
        /// <param name="coefficient">解密写入系数coefficient*1024(默认coefficient=1)</param>
        public static byte[] AESDecrypt(byte[] DecryptByte, string DecryptKey,int coefficient=1)
        {
            if (DecryptByte.Length == 0) { throw (new Exception("密文不得为空")); }
            if (string.IsNullOrEmpty(DecryptKey)) { throw (new Exception("密钥不得为空")); }
            byte[] m_strDecrypt=null;
            byte[] m_btIV = Convert.FromBase64String("Rkb4jvUy/ye7Cd7k89QQgQ==");
            byte[] m_salt = Convert.FromBase64String("gsf4jvkyhye5/d7k8OrLgM==");
            Rijndael m_AESProvider = Rijndael.Create();
            try
            {
                MemoryStream m_stream = new MemoryStream();
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(DecryptKey, m_salt);
                ICryptoTransform transform = m_AESProvider.CreateDecryptor(pdb.GetBytes(32), m_btIV);
                CryptoStream m_csstream = new CryptoStream(m_stream, transform, CryptoStreamMode.Write);
                AsynWrite(m_csstream, DecryptByte, coefficient, progress =>
                {
                    ToolLoom.QueueOnMainThread(() =>
                    {
                        ProtobufCoding.Instance.SetProgress(progress);
                    });
                    if (progress == 1)
                    {
                        m_csstream.FlushFinalBlock();
                        m_strDecrypt = m_stream.ToArray();
                        m_stream.Close();
                        m_stream.Dispose();
                        m_csstream.Close();
                        m_csstream.Dispose();
                    }
                });
            }
            catch (IOException ex) { throw ex; }
            catch (CryptographicException ex) { throw ex; }
            catch (ArgumentException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { m_AESProvider.Clear(); }
            return m_strDecrypt;
        }
        #endregion
        #endregion
        
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="array">要加密的 byte[] 数组</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] array, string key)
        {
            byte[] keyArray = Encoding.UTF8.GetBytes(key);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(array, 0, array.Length);
            return resultArray;
        } 

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="array">要解密的 byte[] 数组</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] array, string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(array, 0, array.Length);

            return resultArray;
        }
        /// <summary>
        ///关闭流操作
        /// </summary>
        /// <param name="stream"></param>
        public static void FlieCloseOperation(Stream stream)
        {
            stream.Flush();
            stream.Dispose();
            stream.Close();
        }
        /// <summary>
        /// 异步写入操作
        /// </summary>
        /// <param name="stream">操作流</param>
        /// <param name="buffer">数据</param>
        /// <param name="coefficient">加密写入系数coefficient*1024</param>
        /// <returns>进度返回</returns>
        public static void AsynWrite(Stream stream,byte[]buffer,int coefficient,Action<float> progress=null)
        {
            int length = coefficient * 1024;
            if (length> buffer.Length)
                length = buffer.Length;
            int offset = 0;
            while (true)
            {
                stream.Write(buffer, offset, length);
                if (buffer.Length <= 1024)
                {
                    progress?.Invoke(1);
                    break;
                }
                offset += length;
                progress?.Invoke(offset*1f/buffer.Length*1f);
                if (buffer.Length - offset <= length)
                {
                    stream.Write(buffer, offset, buffer.Length - offset);
                    progress?.Invoke(1);
                    break;
                }
            }
        }
    }
}