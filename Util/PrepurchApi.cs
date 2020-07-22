using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Util
{
    public class PrepurchApi
    {

        #region ReqURL
        private string ReqURL = string.Empty;
        /// <summary>
        /// ReqURL
        /// </summary>
        public string _ReqURL
        {
            get { return this.ReqURL ?? string.Empty; }    //  https://api.kdniao.com/api/EOrderService  
        }
        #endregion

        #region PrepurchApi
        /// <summary>
        /// PrepurchApi
        /// </summary>
        /// <param name="ReqURL"></param>
        public PrepurchApi(string ReqURL)
        {
            this.ReqURL = ReqURL;
        }
        #endregion

        #region SyncApiByJson
        /// <summary>
        /// SyncApiByJson Json
        /// </summary>
        /// <returns></returns>
        public string SyncApiByJson(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo,string controllerType, string requestData)
        {
            string reqUrl = string.Format("{0}/{1}", ReqURL, controllerType);
            Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
            Newtonsoft.Json.Linq.JArray jlistRequestData = Newtonsoft.Json.Linq.JArray.Parse(requestData);
            string logMessage = string.Empty;
            string result = string.Empty;
            string t = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);  //时间戳(格式：yyyy-MM-ddHH:mm:ss)
            string token =BuildToken();  //"nDWKKyRDMrS1M9zxIbTAQQ=="; // 
            if (string.IsNullOrEmpty(token))
            {
                logMessage = string.Format(" controllerType:{0} ;SendPost SyncApiByJson  接口调用 BuildToken token生成失败:token为空 ！", controllerType);
                Newtonsoft.Json.Linq.JObject requestDataObj = Newtonsoft.Json.Linq.JObject.Parse(requestData);
                resultJObject.Add("errMsg", new Newtonsoft.Json.Linq.JValue(logMessage));
                resultJObject.Add("rstFlag", new Newtonsoft.Json.Linq.JValue(10));
                resultJObject.Add("reqUrl", new Newtonsoft.Json.Linq.JValue(reqUrl));
                resultJObject.Add("t", new Newtonsoft.Json.Linq.JValue(t));
                resultJObject.Add("token", new Newtonsoft.Json.Linq.JValue(token));
                resultJObject.Add("items", new Newtonsoft.Json.Linq.JArray(jlistRequestData));

                result = NewtonsoftCommon.SerializeObjToJson(resultJObject);
                logMessage = string.Format("【{0}_{1}】 {2}", jobInfo.JobCode, jobInfo.JobName.ToString(), result);
                Log4netUtil.Log4NetHelper.Error(logMessage, @"PrepurchApi");
                Log4netUtil.LogDisplayHelper.LogError(logAppendToForms, logMessage);
                return result;
            }
            resultJObject = new Newtonsoft.Json.Linq.JObject();
           
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("t", t);
            param.Add("token", token);
            param.Add("items", HttpUtility.UrlEncode(requestData, Encoding.UTF8));//请求内容为 JSON 格式
            //param.Add("RequestData", HttpUtility.UrlEncode(requestJson, Encoding.UTF8));//请求内容为 JSON 格式
            result = sendPost(reqUrl, param);
            resultJObject = new Newtonsoft.Json.Linq.JObject();
            //logMessage = string.Format("SendPost SyncApiByJson  接口调用! controllerType:{0} ", controllerType);
            logMessage = string.Format(" controllerType:{0} ;SendPost SyncApiByJson  接口调用！",  controllerType);

            resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
            resultJObject.Add("reqUrl", new Newtonsoft.Json.Linq.JValue(reqUrl));
            resultJObject.Add("t", new Newtonsoft.Json.Linq.JValue(t));
            resultJObject.Add("token", new Newtonsoft.Json.Linq.JValue(token));
            //resultJObject.Add("order_code", new Newtonsoft.Json.Linq.JValue(orderCode));
            resultJObject.Add("items", new Newtonsoft.Json.Linq.JArray(jlistRequestData));
            resultJObject.Add("result", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(result)));
            logMessage = string.Format("【{0}_{1}】 {2}", jobInfo.JobCode, jobInfo.JobName.ToString(), NewtonsoftCommon.SerializeObjToJson(resultJObject));
            Log4netUtil.Log4NetHelper.Info(logMessage, @"PrepurchApi");
            Log4netUtil.LogDisplayHelper.LogMessage(logAppendToForms, logMessage);
            return result;
        }
        #endregion

        #region SyncApiByJson
        /// <summary>
        /// SyncApiByJson Json方式  
        /// </summary>
        /// <returns></returns>
        public string SyncApiByJson(Log4netUtil.LogAppendToForms logAppendToForms, Model.JobEntity jobInfo, string controllerType, string requestData,string orderCode)
        {
            string reqUrl = string.Format("{0}/{1}", ReqURL, controllerType);
            Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
            Newtonsoft.Json.Linq.JArray jlistRequestData = Newtonsoft.Json.Linq.JArray.Parse(requestData);
            string logMessage = string.Empty;
            string result = string.Empty;
            string t = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);  //时间戳(格式：yyyy-MM-ddHH:mm:ss)
            string token = BuildToken();
            if (string.IsNullOrEmpty(token))
            {
                logMessage = string.Format("【orderCode:{0} ;controllerType:{1} ;SendPost SyncApiByJson  接口调用 BuildToken token生成失败:token为空 ！",  orderCode,controllerType);
                
                resultJObject.Add("errMsg", new Newtonsoft.Json.Linq.JValue(logMessage));
                resultJObject.Add("rstFlag", new Newtonsoft.Json.Linq.JValue(10));
                resultJObject.Add("reqUrl", new Newtonsoft.Json.Linq.JValue(reqUrl));
                resultJObject.Add("t", new Newtonsoft.Json.Linq.JValue(t));
                resultJObject.Add("token", new Newtonsoft.Json.Linq.JValue(token));
                resultJObject.Add("order_code", new Newtonsoft.Json.Linq.JValue(orderCode));
                resultJObject.Add("items", new Newtonsoft.Json.Linq.JArray(jlistRequestData));
                result =NewtonsoftCommon.SerializeObjToJson(resultJObject);
                logMessage = string.Format("【{0}_{1}】 {2}", jobInfo.JobCode, jobInfo.JobName.ToString(), result);
                Log4netUtil.Log4NetHelper.Error(logMessage, @"PrepurchApi");
                Log4netUtil.LogDisplayHelper.LogError(logAppendToForms, logMessage);
                return result;
            }
            resultJObject = new Newtonsoft.Json.Linq.JObject();
            
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("t", t);
            param.Add("token", token);
            param.Add("order_code", orderCode);
            param.Add("items", HttpUtility.UrlEncode(requestData, Encoding.UTF8));//请求内容为 JSON 格式
            result = sendPost(reqUrl, param);
            resultJObject = new Newtonsoft.Json.Linq.JObject();
            logMessage = string.Format(" orderCode:{0}; controllerType:{1} ;SendPost SyncApiByJson  接口调用！", orderCode,controllerType);
            resultJObject.Add("msg", new Newtonsoft.Json.Linq.JValue(logMessage));
            resultJObject.Add("reqUrl", new Newtonsoft.Json.Linq.JValue(reqUrl));
            resultJObject.Add("t", new Newtonsoft.Json.Linq.JValue(t));
            resultJObject.Add("token", new Newtonsoft.Json.Linq.JValue(token));
            resultJObject.Add("order_code", new Newtonsoft.Json.Linq.JValue(orderCode));
            resultJObject.Add("items", new Newtonsoft.Json.Linq.JArray(jlistRequestData));
            resultJObject.Add("result", new Newtonsoft.Json.Linq.JObject(Newtonsoft.Json.Linq.JObject.Parse(result)));
            logMessage = string.Format("【{0}_{1}】 {2}", jobInfo.JobCode, jobInfo.JobName.ToString(), NewtonsoftCommon.SerializeObjToJson(resultJObject));
            Log4netUtil.Log4NetHelper.Info(logMessage, @"PrepurchApi");
            Log4netUtil.LogDisplayHelper.LogMessage(logAppendToForms, logMessage);
            return result;
        }
        #endregion

        



        #region sendPost Post方式提交数据，返回网页的源代码
        /// <summary>
        /// Post方式提交数据，返回网页的源代码
        /// </summary>
        /// <param name="url">发送请求的 URL</param>
        /// <param name="param">请求的参数集合</param>
        /// <returns>远程资源的响应结果</returns>
        private string sendPost(string url, Dictionary<string, string> param)
        {
            string result = "";
            StringBuilder postData = new StringBuilder();
            if (param != null && param.Count > 0)
            {
                foreach (var p in param)
                {
                    if (postData.Length > 0)
                    {
                        postData.Append("&");
                    }
                    postData.Append(p.Key);
                    postData.Append("=");
                    postData.Append(p.Value);
                }
            }

            byte[] byteData = Encoding.GetEncoding("UTF-8").GetBytes(postData.ToString());
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType ="application/x-www-form-urlencoded"; //"application/json"; //
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
                result = "9999";
                Log4netUtil.Log4NetHelper.Error("SendPost Api接口失败 失败原因:", ex, @"PrepurchApi");
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

        //token生成方式如下：
        private  string BuildToken()
        {
            string token = string.Empty;
            try
            {

                string curDay = string.Format("{0:yyyyMMdd}", DateTime.Now);   //DateUtil.formatDate(new Date(), "yyyyMMdd");
                string content = "YK0001-" + curDay;
                token  =  Util.DESEncryptHelper.EncryptString(content);
            }
            catch (Exception ex)
            {
                Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                string logMessage = string.Format("BuildToken token生成失败{0}", ex.Message);
                resultJObject.Add("errMsg", logMessage);
                resultJObject.Add("rstFlag", 10);
                string result = NewtonsoftCommon.SerializeObjToJson(resultJObject);
                Log4netUtil.Log4NetHelper.Error(result, @"PrepurchApi");
                return string.Empty;
            }
            return token;
        }


        #region encrypt 电商Sign签名
        ///<summary>
        ///电商Sign签名
        ///</summary>
        ///<param name="content">内容</param>
        ///<param name="keyValue">Appkey</param>
        ///<param name="charset">URL编码 </param>
        ///<returns>DataSign签名</returns>
        private string encrypt(String content, String keyValue, String charset)
        {
            if (keyValue != null)
            {
                return base64(MD5(content + keyValue, charset), charset);
            }
            return base64(MD5(content, charset), charset);
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
}