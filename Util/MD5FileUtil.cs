
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{


    public class MD5FileUtil
    {
        public static string GetMD5Hash(string pathName)
        {
            return getMD5Hash(pathName);
        }


        /// <summary>
        /// getMD5Hash
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        private static string getMD5Hash(string pathName)
        {
            string strResult = "";
            string strHashData = "";
            byte[] arrbytHashValue;
            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            try
            {

                byte[] fromData = System.Text.Encoding.UTF8.GetBytes(pathName);
                arrbytHashValue = oMD5Hasher.ComputeHash(fromData);//计算指定Stream 对象的哈希值
                //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
                strHashData = System.BitConverter.ToString(arrbytHashValue);
                //替换-
                strHashData = strHashData.Replace("-", "");
                strResult = strHashData;
            }
            catch (System.Exception ex)
            {
                string aa = ex.Message;
                return "";
            }

            return strResult;
        }
    }

}

