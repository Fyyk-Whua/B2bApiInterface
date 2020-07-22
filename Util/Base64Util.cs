using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    ///*************************************************************************/
    ///*
    ///* 文 件 名: Base64Util.cs   
    ///* 命名空间: Util.FrameUtil
    ///* 功    能: Base64加解密基类
    ///* 内    容: 
    ///* 原创作者: lau 
    ///* 生成日期: 2018.08.08
    ///* 版 本 号: V1.0.0.0
    ///* 修改日期:
    ///* 版权说明:  Copyright 2018-2027 武汉飞宇益克科技有限公司
    ///*
    ///**************************************************************************/
    public class Base64Util
    {
        
        #region EncodeBase64 将字符串使用base64算法加密 
        /// <summary> /// 将字符串使用base64算法加密   
        /// </summary>   
        /// <param name="code_type">编码类型（编码名称）   
        /// * 代码页 名称   
        /// * 1200 "UTF-16LE"、"utf-16"、"ucs-2"、"unicode"或"ISO-10646-UCS-2"   
        /// * 1201      
        /// <summary>   
        /// 将字符串使用base64算法加密   
        /// </summary>   
        /// <param name="code_type">编码类型（编码名称）   
        /// * 代码页 名称   
        /// * 1200 "UTF-16LE"、"utf-16"、"ucs-2"、"unicode"或"ISO-10646-UCS-2"   
        /// * 1201 "UTF-16BE"或"unicodeFFFE"   
        /// * 1252 "windows-1252"   
        /// * 65000 "utf-7"、"csUnicode11UTF7"、"unicode-1-1-utf-7"、"unicode-2-0-utf-7"、"x-unicode-1-1-utf-7"或"x-unicode-2-0-utf-7"   
        /// * 65001 "utf-8"、"unicode-1-1-utf-8"、"unicode-2-0-utf-8"、"x-unicode-1-1-utf-8"或"x-unicode-2-0-utf-8"   
        /// * 20127 "us-ascii"、"us"、"ascii"、"ANSI_X3.4-1968"、"ANSI_X3.4-1986"、"cp367"、"csASCII"、"IBM367"、"iso-ir-6"、"ISO646-US"或"ISO_646.irv:1991"   
        /// * 54936 "GB18030"   
        /// </param>   
        /// <param name="code">待加密的字符串</param>   
        /// <returns>加密后的字符串</returns>   
        public static string EncodeBase64(string code_type, string code)  
        {  
            string encode = "";  
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(code); //将一组字符编码为一个字节序列.   
            try  
            {  
                encode = Convert.ToBase64String(bytes); //将8位无符号整数数组的子集转换为其等效的,以64为基的数字编码的字符串形式.   
            }  
            catch  
            {  
                encode = code;  
            }  
            return encode;  
        }
        #endregion

        #region EncodeBase64_UTF8 Base64加密，采用utf8编码方式加密
        /// <summary> 
        /// Base64加密，采用utf8编码方式加密 
        /// </summary> 
        /// <param name="source">待加密的明文</param> 
        /// <returns>加密后的字符串</returns> 
        public static string EncodeBase64_UTF8(string source)
        {
            return EncodeBase64("utf-8", source);
        } 
        #endregion

        #region EncodeBase64 将字符串使用base64算法加密 采用ANSI编码方式加密
        public static string EncodeBase64(string code)  
        {  
            string encode = "";  
            byte[] bytes = Encoding.Default.GetBytes(code); //将一组字符编码为一个字节序列.   
            try  
            {  
                encode = Convert.ToBase64String(bytes); //将8位无符号整数数组的子集转换为其等效的,以64为基的数字编码的字符串形式.   
            }  
            catch  
            {  
                encode = code;  
            }  
            return encode;  
        }  
        #endregion

        #region DecodeBase64 将字符串使用base64算法解密   
        /// <summary>   
        /// 将字符串使用base64算法解密   
        /// </summary>   
        /// <param name="code_type">编码类型</param>   
        /// <param name="code">已用base64算法加密的字符串</param>   
        /// <returns>解密后的字符串</returns>   
        public static string DecodeBase64(string code_type, string code)  
        {  
            string decode = "";  
            byte[] bytes = Convert.FromBase64String(code); //将2进制编码转换为8位无符号整数数组.   
            try  
            {  
                decode = Encoding.GetEncoding(code_type).GetString(bytes); //将指定字节数组中的一个字节序列解码为一个字符串。   
            }  
            catch  
            {  
                decode = code;  
            }  
            return decode;  
        }  
        #endregion

        #region DecodeBase64 将字符串使用base64算法解密 采用ANSI编码方式
        public static string DecodeBase64(string code)  
        {  
            string decode = "";  
            byte[] bytes = Convert.FromBase64String(code); //将2进制编码转换为8位无符号整数数组.   
            try  
            {  
                decode = Encoding.Default.GetString(bytes); //将指定字节数组中的一个字节序列解码为一个字符串。   
            }  
            catch  
            {  
                decode = code;  
            }  
            return decode;  
        }
        #endregion

        #region DecodeBase64_UTF8  Base64解密，采用utf8编码方式解密
        /// <summary> 
        /// Base64解密，采用utf8编码方式解密 
        /// </summary> 
        /// <param name="result">待解密的密文</param> 
        /// <returns>解密后的字符串</returns> 
        public static string DecodeBase64_UTF8(string result)
        {
            return DecodeBase64("utf-8", result);
        } 
        #endregion

        #region IsBase64Formatted 是否为 Base64
        /// <summary>
        /// IsBase64Formatted 是否为 Base64
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsBase64Formatted(string input)
        {
            //if (!input.Trim().Equals(string.Empty) || input.Length != 0)
            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    Convert.FromBase64String(input);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
    }  
}  
