using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Util
{
   
    public class NewGuid
    {
        public static string GetSnowflakeIdWorkerToString()
        {
            return IdWorkerHelper.GenId64().ToString();
        }

        public static long GetSnowflakeIdWorkerToLong()
        {
            return IdWorkerHelper.GenId64();
        }


        public static string GetIdentityGeneratorToString(string codePrefix)
        {
            Util.IdentityGenerator ig = new Util.IdentityGenerator();
            long identity = ig.GetIdentity(Util.TimestampStyle.SecondTicks, 2);//同时设置上面两种精度
            return string.Format("{0}{1}", codePrefix.Trim(),identity.ToString());
        }


        /// <summary>
        /// 唯一订单号生成
        /// </summary>
        /// <returns></returns>
        public static string GenerateOrderNumber()
        {
            string strDateTimeNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
            string strRandomResult = NextRandom(10, 1).ToString();
            return "GZ" + strDateTimeNumber + strRandomResult;
        }

        /// <summary>
        /// 唯一订单号生成
        /// </summary>
        /// <returns></returns>
        public static string GenerateOrderNumber(string Code)
        {
            string strDateTimeNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
            string strRandomResult = NextRandom(10, 1).ToString();
            return Code + strDateTimeNumber + strRandomResult;
        }

        public static string GenerateOrderNumber(string NSRSBH,string JQH)
        {
            string strDateTimeNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
            string strRandomResult = NextRandom(9, 1).ToString();
            return NSRSBH + JQH+ strDateTimeNumber + strRandomResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numSeeds"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int NextRandom(int numSeeds, int length)
        {
            // Create a byte array to hold the random value.  
            byte[] randomNumber = new byte[length];
            // Create a new instance of the RNGCryptoServiceProvider.  
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            // Fill the array with a random value.  
            rng.GetBytes(randomNumber);
            // Convert the byte to an uint value to make the modulus operation easier.  
            uint randomResult = 0x0;
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)randomNumber[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds) + 1;
        }


        #region GetNetDateTime 获取网络日期时间
        /// <summary>  
        /// 获取网络日期时间  
        /// </summary>  
        /// <returns></returns>  
        public static string GetNetDateTime()
        {
            WebRequest request = null;
            WebResponse response = null;
            WebHeaderCollection headerCollection = null;
            string datetime = string.Empty;
            try
            {
                request = WebRequest.Create("https://www.baidu.com");
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultCredentials;
                response = (WebResponse)request.GetResponse();
                headerCollection = response.Headers;
                foreach (var h in headerCollection.AllKeys)
                { if (h == "Date") { datetime = headerCollection[h]; } }
                return datetime;
            }
            catch (Exception) { return datetime; }
            finally
            {
                if (request != null)
                { request.Abort(); }
                if (response != null)
                { response.Close(); }
                if (headerCollection != null)
                { headerCollection.Clear(); }
            }
        }
        #endregion

        #region GetGuid
        /// <summary>
        /// 由连字符分隔的32位数字
        /// </summary>
        /// <returns></returns>
        public static string GetGuid()
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            return guid.ToString();
        }
        #endregion

        #region GuidTo16String
        /// <summary>  
        /// 根据GUID获取16位的唯一字符串  
        /// </summary>  
        /// <param name=\"guid\"></param>  
        /// <returns></returns>  
        public static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        #endregion

        #region GuidToLongID
        /// <summary>  
        /// 根据GUID获取19位的唯一数字序列  
        /// </summary>  
        /// <returns></returns>  
        public static long GuidToLongID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        } 
        #endregion
    }
}
