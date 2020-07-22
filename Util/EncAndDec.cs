using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Util
{
    ///*************************************************************************/
    ///*
    ///* 文 件 名: EncAndDec.cs   
    ///* 命名空间: Util.FrameUtil
    ///* 功    能: EncAndDec加解密基类
    ///* 内    容: 
    ///* 原创作者: lau 
    ///* 生成日期: 2018.08.08
    ///* 版 本 号: V1.0.0.0
    ///* 修改日期:
    ///* 版权说明:  Copyright 2018-2027 
    ///*
    ///**************************************************************************/
    public class EncAndDec
    {
        #region 静态构造,不能实例化 
        static EncAndDec() { } /**/
        #endregion

        #region HashSet
        private static readonly HashSet<char> h = new HashSet<char>(){
    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
    'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f',
    'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
    'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/',
    '='
        };
        #endregion

        #region getMD5 给一个字符串进行MD5加密 后转成Base64   用于平台Java  MD5加密一致
        ///   <summary>
        ///   给一个字符串进行MD5加密 后转成Base64   用于平台Java  MD5加密一致
        ///   </summary>
        ///   <param   name="strText">待加密字符串</param>
        ///   <returns>加密后的字符串</returns>
        public static string GetMD5ToBase64(string str)
        {
            return getMD5ToBase64(str);
        }

        private static string getMD5ToBase64(string str)
        {
            if (!Base64Util.IsBase64Formatted(str))
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                byte[] message;
                message = Encoding.Default.GetBytes(str);
                md5.ComputeHash(message).ToString();
                return Convert.ToBase64String(md5.Hash);
            }
            else
            {
                return str;
            }
        }
        #endregion

        #region DESEncrypt DES 加密算法
        /// <summary>
        /// DES 加密算法
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <returns></returns>
        public static string DESEncrypt(string pToEncrypt)
        {
            return dESEncrypt(pToEncrypt);
        }
        private static string dESEncrypt(string pToEncrypt)
        {
            string Key = "DKMAB5DE";//加密密钥必须为8
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(Key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }
        #endregion

        #region DESDecrypt DES 解密算法
        /// <summary>
        /// DES 解密算法 
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <returns></returns>
        public static string DESDecrypt(string pToDecrypt)
        {
            return dESDecrypt(pToDecrypt);
        }
        private static string dESDecrypt(string pToDecrypt)
        {
            if (Base64Util.IsBase64Formatted(pToDecrypt))  //未加密返回原数值
            {
                try
                {
                    string Key = "DKMAB5DE";//加密密钥必须为8
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                    for (int x = 0; x < pToDecrypt.Length / 2; x++)
                    {
                        int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                        inputByteArray[x] = (byte)i;
                    }
                    des.Key = ASCIIEncoding.ASCII.GetBytes(Key);
                    des.IV = ASCIIEncoding.ASCII.GetBytes(Key);
                    MemoryStream ms = new MemoryStream();
                    CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    StringBuilder ret = new StringBuilder();
                    return System.Text.Encoding.ASCII.GetString(ms.ToArray());
                }
                catch
                {
                    return pToDecrypt;
                }
            }
            else
            {
                return pToDecrypt;
            }

        }
        #endregion
    }
}



