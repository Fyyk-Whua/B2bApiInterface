
using System;
using System.Security.Cryptography;
using System.Text;

namespace Util
{
    ///*************************************************************************/
    ///*
    ///* 文 件 名: AesClass.cs   
    ///* 命名空间: FYYK.Util.FrameUtil
    ///* 功    能: Aes加密、解密
    ///* 内    容: 
    ///* 原创作者: lau 
    ///* 生成日期: 2018.08.08
    ///* 版 本 号: V1.0.0.0
    ///* 修改日期:
    ///* 版权说明:  Copyright 2018-2027 武汉飞宇益克科技有限公司
    ///*
    ///**************************************************************************/
    public class AesClass
    {

        #region public
        #region AesEncrypt  AES 加密
        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str">明文（待加密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        public static string AesEncrypt(string str, string key)
        {
            if (String.IsNullOrEmpty(key))
                return aesEncrypt(str, _strkey);
            else
                return aesEncrypt(str, key);
        }
        #endregion



        #region AesDecrypt AES 解密
        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str">明文（待解密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        public static string AesDecrypt(string str, string key)
        {
            if (String.IsNullOrEmpty(key))
                return aesDecrypt(str, _strkey);
            else
                return aesDecrypt(str, key);

        }
        #endregion
        #endregion

        #region private

        private const string _strkey = "64ADF32FAEF21A27";

        #region aesDecrypt AES 解密
        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str">明文（待解密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        private static string aesDecrypt(string str, string strkey)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(strkey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
        #endregion

        #region aesEncrypt  AES 加密
        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str">明文（待加密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        private static string aesEncrypt(string str, string strkey)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(strkey),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion

        #endregion 

        /*

        /// <summary>
        /// 有密码的AES加密 
        /// </summary>
        /// <param name="text">加密字符</param>
        /// <param name="password">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <returns></returns>
        public static string JavaEncrypt(string toEncrypt,string key)
        {
 
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
 
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
 
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
 
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
 
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="text"></param>
        /// <param name="password"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string JavaDecrypt(string toDecrypt,string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
 
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
 
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
 
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        */
    }
}




