using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace Util
{
    public class B2bApi
    {
        #region DomainName Api 域名
        private string DomainName = string.Empty;
        /// <summary>
        ///Api 域名
        /// </summary>
        public string _DomainName
        {
            get { return this.DomainName ?? string.Empty; }
        }
        #endregion

        #region ServiceName Api 服务器
        private string ServiceName = string.Empty;
        /// <summary>
        /// Api 服务器
        /// </summary>
        public string _ServiceName
        {
            get { return this.ServiceName ?? string.Empty; }
        }
        #endregion

        #region InterfacePrefix Api 接口前缀
        private string InterfacePrefix = string.Empty;
        /// <summary>
        /// InterfacePrefix Api 接口前缀
        /// </summary>
        public string _InterfacePrefix
        {
            get { return this.InterfacePrefix ?? string.Empty; }
        }
        #endregion

        #region SignKey 
        private string SignKey = string.Empty;
        /// <summary>
        /// SignKey
        /// </summary>
        public string _SignKey
        {
            get { return this.SignKey ?? string.Empty; }
        }
        #endregion

        #region EncryptKey
        private string EncryptKey = string.Empty;
        /// <summary>
        /// EncryptKey
        /// </summary>
        public string _EncryptKey
        {
            get { return this.EncryptKey ?? string.Empty; }
        }
        #endregion


        #region B2bApi
        /// <summary>
        /// B2bApi
        /// </summary>
        /// <param name="domainName"></param>
        /// <param name="serviceName"></param>
        /// <param name="interfacePrefix"></param>
        /// <param name="signKey"></param>
        /// <param name="encryptKey"></param>
        public B2bApi(string domainName, string serviceName, string interfacePrefix,string signKey, string encryptKey)
        {
            this.DomainName = domainName;
            this.ServiceName = serviceName;
            this.InterfacePrefix = interfacePrefix;
            this.SignKey = signKey;
            this.EncryptKey = encryptKey;
        }
        #endregion


        #region B2bApiRequestByJson
        /// <summary>
        /// B2bApiRequestByJson
        /// </summary>
        /// <param name="moduleType">模板类型</param>
        /// <param name="requestType">请求类型</param>
        /// <param name="requestData">请求数据</param>
        /// <returns></returns>
        public string B2bApiRequestByJson(Log4netUtil.LogAppendToForms logAppendToForms, 
                                          Model.JobEntity jobInfo, string requestData)
        {
            //Log4netUtil.Log4NetHelper.Info(string.Format("SendPost FyykApi RequestData{0}", requestData), @"DYZS\FyykApi");
            Newtonsoft.Json.Linq.JObject jObject;
            string logMessage = string.Empty;
            string reqUrl = string.Format(@"{0}/{1}/{2}/{3}", _DomainName, _ServiceName, jobInfo.ApiModuleType, jobInfo.ApiRequestType);


            string stortJson = string.Empty;
            string content = string.Empty;

            //var jArray = Newtonsoft.Json.Linq.JArray.Parse(requestData);
            //jObject = Newtonsoft.Json.Linq.JObject.Parse(requestData);
            //string jsonData = JsonConvert.SerializeObject(jObject);
            stortJson = requestData;// Util.NewtonsoftCommon.StortJson(requestData, false); //排序
            content = requestData;// JsonConvert.SerializeObject(jObject);

            try
            {
                string sign = SignEncrypt(stortJson, _SignKey).ToLower();
                string jsonDataEN = AesEncrypt(content, _EncryptKey);
                //string jsonDataENReplace = jsonDataEN;//  jsonDataEN.Replace("+", "%2B");//注意加号（’+‘）的替换处理，否则由于加号经过Url传递后变成空格而得不到合法的Base64字符串
                //Dictionary<string, string> param = new Dictionary<string, string>();
                //jsonDataENReplace = string.Empty;
                var requestJObject = new Newtonsoft.Json.Linq.JObject();
                requestJObject.Add("data", jsonDataEN);
                requestJObject.Add("sign", sign);

                string result = SendPost(logAppendToForms, jobInfo,reqUrl, Util.NewtonsoftCommon.SerializeObjToJson(requestJObject));
                jObject = new Newtonsoft.Json.Linq.JObject();
                jObject.Add("returl", reqUrl);
                jObject.Add("request", requestData);
                jObject.Add("result", result);// JToken.FromObject(new {result}));// Newtonsoft.Json.Linq.JObject.Parse(result));

                Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(result);
                jObject.Add("code", resultJObject.Value<int>("code"));
                if (string.Equals(resultJObject.Value<int>("code"), 200))
                {
                    jObject.Add("msg", string.Format("B2bApiRequestByJson接口调用成功!", jobInfo.JobCode, jobInfo.JobName));
                    logMessage = string.Format("【{0}_{1}】 B2bApiRequestByJson接口调用成功!\n\r {2}", jobInfo.JobCode, jobInfo.JobName, Util.NewtonsoftCommon.SerializeObjToJson(jObject));
                    Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, jobInfo.IsDebug, logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
                }
                else
                {
                    jObject.Add("msg", string.Format("B2bApiRequestByJson 接口调用失败!", jobInfo.JobCode, jobInfo.JobName));
                    logMessage = string.Format("【{0}_{1}】 B2bApiRequestByJson 接口调用失败!\n\r {2}", jobInfo.JobCode, jobInfo.JobName, Util.NewtonsoftCommon.SerializeObjToJson(jObject));
                    Log4netUtil.Log4NetHelper.LogError(logAppendToForms, jobInfo.IsDebug, logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
                }
                return result;
            }catch(Exception ex)
            {
                logMessage = string.Format("【{0}_{1}】B2bApiRequestByJson接口失败 失败原因:{2}", jobInfo.JobCode,jobInfo.JobName, ex);
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, jobInfo.IsDebug,logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
                Newtonsoft.Json.Linq.JObject exJObject = new Newtonsoft.Json.Linq.JObject();
                exJObject.Add("code", 999);
                exJObject.Add("msg", ex.Message);
                exJObject.Add("data", string.Empty);
                exJObject.Add("requestUrl", reqUrl);
                exJObject.Add("request", Newtonsoft.Json.Linq.JObject.Parse(requestData));
                return Util.NewtonsoftCommon.SerializeObjToJson(exJObject);
            }
            
        }
        #endregion


        #region SendPost Post方式提交数据，返回网页的源代码
        /// <summary>
        /// Post方式提交数据，返回网页的源代码
        /// </summary>
        /// <param name="url">发送请求的 URL</param>
        /// <param name="param">请求的参数集合</param>
        /// <returns>远程资源的响应结果</returns>
        private string SendPost(Log4netUtil.LogAppendToForms logAppendToForms,
                                Model.JobEntity jobInfo,
                                string url, string postData)
        {
            string logMessage = string.Empty;
            string result = string.Empty;
            byte[] byteData = Encoding.GetEncoding("UTF-8").GetBytes(postData.ToString());
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";// "application/x-www-form-urlencoded";
                request.Referer = url;
                request.Accept = "*/*";
                request.Timeout = 30 * 1000;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                request.Method = "POST";
                request.ContentLength = byteData.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(byteData, 0, byteData.Length);
                stream.Flush();
                stream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream backStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(backStream, Encoding.GetEncoding("UTF-8"));
                result = sr.ReadToEnd();
                sr.Close();
                backStream.Close();
                response.Close();
                request.Abort();
            }
            catch (Exception ex)
            {
                logMessage = string.Format("【{0}_{1}】 SendPost b2bApi接口失败 失败原因:{2}", jobInfo.JobCode, jobInfo.JobName, ex.Message);
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, jobInfo.IsDebug, logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
                Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
                jObject.Add("code", 999);
                jObject.Add("msg", ex.Message);
                jObject.Add("data", string.Empty);
                result = Util.NewtonsoftCommon.SerializeObjToJson(jObject);
            }
            return result;
        }
        #endregion

        #region SendPost Post方式提交数据，返回网页的源代码
        /// <summary>
        /// Post方式提交数据，返回网页的源代码
        /// </summary>
        /// <param name="url">发送请求的 URL</param>
        /// <param name="param">请求的参数集合</param>
        /// <returns>远程资源的响应结果</returns>
        private string SendPost(Log4netUtil.LogAppendToForms logAppendToForms,
                                Model.JobEntity jobInfo, 
                                string url, Dictionary<string, string> param)
        {
            string result = string.Empty;
            string logMessage = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/json";// "application/x-www-form-urlencoded";  //"application/json";// "application/x-www-form-urlencoded";
                request.Referer = url;
                request.Accept = "*/*";
                request.Timeout = 30 * 1000;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                request.Method = "POST";

                #region 添加Post 参数
                StringBuilder builder = new StringBuilder();
                int i = 0;
                foreach (var item in param)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
                byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
                request.ContentLength = data.Length;
                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);
                    reqStream.Flush();
                    reqStream.Close();
                }
                #endregion

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream backStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(backStream, Encoding.GetEncoding("UTF-8"));
                result = sr.ReadToEnd();
                sr.Close();
                backStream.Close();
                response.Close();
                request.Abort();
            }
            catch (Exception ex)
            {
                logMessage = string.Format("【{0}_{1}】 SendPost b2bApi接口失败 失败原因:{2}", jobInfo.JobCode, jobInfo.JobName,ex.Message);
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, jobInfo.IsDebug, logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
                Newtonsoft.Json.Linq.JObject jObject = new Newtonsoft.Json.Linq.JObject();
                jObject.Add("code", 999);
                jObject.Add("msg", ex.Message);
                jObject.Add("data", string.Empty);
                result = Util.NewtonsoftCommon.SerializeObjToJson(jObject);
            }
            return result;
        }
        #endregion

        #region CheckValidationResult
        /// <summary>
        /// CheckValidationResult
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            throw new NotImplementedException();
        }
        #endregion 

        #region SignEncrypt Sign签名
        /// <summary>
        /// SignEncrypt Sign签名
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fyykKey"></param>
        /// <returns></returns>
        private string SignEncrypt(String content, string fyykKey)
        {
            return Util.FyykMD5FileUtil.GetMD5Hash(content + fyykKey);  //md5
        }
        #endregion

        #region AesEncrypt Aes加密
        /// <summary>
        /// AesEncrypt Aes加密
        /// </summary>
        /// <param name="content"></param>
        /// <param name="aesKey"></param>
        /// <returns></returns>
        private string AesEncrypt(String content, String aesKey)
        {
            return Util.AesClass.AesEncrypt(content, aesKey); //Aes

            //
        }
        #endregion

        #region MD5  字符串MD5加密
        ///<summary>
        /// 字符串MD5加密
        ///</summary>
        ///<param name="str">要加密的字符串</param>
        ///<param name="charset">编码方式</param>
        ///<returns>密文</returns>
        private string MD5(string str, string charset)
        {
            byte[] buffer = System.Text.Encoding.GetEncoding(charset).GetBytes(str);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider check;
                check = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] somme = check.ComputeHash(buffer);
                string ret = "";
                foreach (byte a in somme)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("X");
                    else
                        ret += a.ToString("X");
                }
                return ret.ToLower();
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region base64 base64编码
        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="charset">编码方式</param>
        /// <returns></returns>
        private string base64(String str, String charset)
        {
            return Convert.ToBase64String(System.Text.Encoding.GetEncoding(charset).GetBytes(str));
        }
        #endregion

      
    }

    #region  FyykMD5FileUtil
    public class FyykMD5FileUtil
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
    #endregion

    #region  FyykAesClass
    public class FyykAesClass
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

            // string Base64StrData = Convert.ToBase64String(buffer).Replace("+", "%2B");//注意加号（’+‘）的替换处理，否则由于加号经过Url传递后变成空格而得不到合法的Base64字符串
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion

        #endregion


    }
    #endregion
}
