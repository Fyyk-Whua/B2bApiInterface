using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;


namespace Util
{
    /// <summary>
    /// DES加密管理类
    /// </summary>
    public class DESEncryptHelper
    {
        
        
        private static readonly string DES_Key = "Rhgorup.888.cn!@#$%";//加密密钥

        /// <summary>
        /// DES加密（输出Base64格式）
        /// </summary>
        /// <param name="builderIdcard"></param>
        /// <returns></returns>
        public static string EncryptString(string str)
        {
            string myKey = DES_Key; // Config.Config.builderIdcardMS;
            if (myKey.Length < 9)
            {
                for (; ; )
                {
                    if (myKey.Length < 9)
                        myKey += myKey;
                    else
                        break;
                }
            }
            string encryptKey = myKey.Substring(0, 8);
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象   
            descsp.Mode = CipherMode.ECB;
            descsp.Padding = PaddingMode.PKCS7;
            byte[] key = Encoding.UTF8.GetBytes(encryptKey); //定义字节数组，用来存储密钥    
            byte[] data = Encoding.UTF8.GetBytes(str);//定义字节数组，用来存储要加密的字符串  
            MemoryStream MStream = new MemoryStream(); //实例化内存流对象      
            //使用内存流实例化加密流对象   
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);  //向加密流中写入数据      
            CStream.FlushFinalBlock();              //释放加密流      
            return Convert.ToBase64String(MStream.ToArray()).Replace("+", "%2B");//返回加密后的字符串  
        }


        
    }
}
