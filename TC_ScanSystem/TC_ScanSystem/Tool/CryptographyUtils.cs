using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public class CryptographyUtils
    {
        #region 3DES 加密
        /// <summary>
        /// 3DES 加密
        /// </summary>
        /// <param name="text">加密数据</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>        
        /// <param name="cipherMode">指定用于加密的块密码模式</param>
        /// <param name="paddingMode">指定在消息数据块比加密操作所需的全部字节数短时应用的填充类型</param>
        /// <returns>加密后的文本</returns>
        public static string TripleDESEncrypt(string text, string key, string iv, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
            byte[] Key = Encoding.UTF8.GetBytes(key);
            byte[] IV = Encoding.UTF8.GetBytes(iv);

            using (MemoryStream ms = new MemoryStream())
            {
                using (TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider())
                {
                    cryptoProvider.Padding = paddingMode;
                    cryptoProvider.Mode = cipherMode;

                    using (CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                    }
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        #endregion

        #region 3DES 解密
        /// <summary>
        /// 3DES 解密
        /// </summary>
        /// <param name="text">加密数据</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <param name="cipherMode">指定用于加密的块密码模式</param>
        /// <param name="paddingMode">指定在消息数据块比加密操作所需的全部字节数短时应用的填充类型</param>
        /// <returns>解密后的文本</returns>
        public static string TripleDESDecrypt(string text, string key, string iv, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            byte[] inputByteArray = Convert.FromBase64String(text);
            byte[] Key = Encoding.UTF8.GetBytes(key);
            byte[] IV = Encoding.UTF8.GetBytes(iv);

            using (MemoryStream ms = new MemoryStream())
            {
                using (TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider())
                {
                    cryptoProvider.Padding = paddingMode;
                    cryptoProvider.Mode = cipherMode;

                    using (CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                    }
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
        #endregion

        #region DES加密
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="text">加密数据</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <param name="cipherMode">指定用于加密的块密码模式</param>
        /// <param name="paddingMode">指定在消息数据块比加密操作所需的全部字节数短时应用的填充类型</param>
        /// <returns>加密后的文本</returns>
        public static string DESEncrypt(string text, string key, string iv, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
            byte[] Key = Encoding.UTF8.GetBytes(key);
            byte[] IV = Encoding.UTF8.GetBytes(iv);

            using (MemoryStream ms = new MemoryStream())
            {
                using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
                {
                    cryptoProvider.Padding = paddingMode;
                    cryptoProvider.Mode = cipherMode;
                    using (CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                    }
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        #endregion

        #region DES解密
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="text">加密数据</param>
        /// <param name="key">密钥</param>
        /// <param name="iv">向量</param>
        /// <param name="cipherMode">指定用于加密的块密码模式</param>
        /// <param name="paddingMode">指定在消息数据块比加密操作所需的全部字节数短时应用的填充类型</param>
        /// <returns>解密后的文本</returns>
        public static string DESDecrypt(string text, string key, string iv, CipherMode cipherMode = CipherMode.ECB, PaddingMode paddingMode = PaddingMode.PKCS7)
        {
            byte[] inputByteArray = Convert.FromBase64String(text);
            byte[] Key = Encoding.UTF8.GetBytes(key);
            byte[] IV = Encoding.UTF8.GetBytes(iv);

            using (MemoryStream ms = new MemoryStream())
            {
                using (DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider())
                {
                    cryptoProvider.Padding = paddingMode;
                    cryptoProvider.Mode = cipherMode;

                    using (CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                    }
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="text">要加密的文本</param>
        /// <returns>加密后的文本</returns>
        public static string MD5Encrypt(string text)
        {
            string hexCiphertext;
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                hexCiphertext = data.ToHex();
                md5.Clear();
            }
            return hexCiphertext.ToLower();
        }
        #endregion
    }
}
