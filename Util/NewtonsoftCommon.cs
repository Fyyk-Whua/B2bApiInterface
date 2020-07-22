using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Web;
using System.Text.RegularExpressions;

namespace Util
{
    public class NewtonsoftCommon
    {

        #region 格式化
        /// <summary>
        /// 格式化json字符串
        /// </summary>
        public static string ConvertJsonString(string str)
        {
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }
        #endregion 

        #region 序列化

        /// <summary>
        /// 将对象(包含集合对象)序列化为Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObjToJson(object obj)
        {
            string strRes = string.Empty;
            try
            {
                strRes = JsonConvert.SerializeObject(obj);
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Fatal(string.Format("SerializeObjToJson序列化失败,语句：{0}", obj.ToString()), ex, @"Frame\Newtonsoft.Json");

            }

            return strRes;
        }

        /// <summary>
        /// 将xml转换为json
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string SerializeXmlToJson(System.Xml.XmlNode node)
        {
            string strRes = string.Empty;
            try
            {
                strRes = JsonConvert.SerializeXmlNode(node);

                //xml 转json
                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(xml);
                //string jsontext = JsonConvert.SerializeXmlNode(doc);
            }
            catch
            { }

            return strRes;
        }

        /// <summary>
        /// 支持Linq格式的xml转换
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string SerializeXmlToJson(System.Xml.Linq.XNode node)
        {
            string strRes = string.Empty;
            try
            {
                strRes = JsonConvert.SerializeXNode(node);
            }
            catch
            { }

            return strRes;
        }
        #endregion

        #region 反序列化
        /// <summary>
        /// 将json反序列化为实体对象(包含DataTable和List<>集合对象)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>  
        public static T DeserializeJsonToObj<T>(string strJson)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            T oRes = default(T);
            try
            {
                oRes = js.Deserialize<T>(strJson); // JsonConvert.DeserializeObject<T>(strJson);
            }
            catch
            {
                Log4netUtil.Log4NetHelper.Fatal(string.Format("DeserializeJsonToObj反序列化失败,语句：{0}", strJson), @"Frame\Newtonsoft.Json");
            }

            return oRes;
        }


        /// <summary>
        /// 将json反序列化为实体对象(包含DataTable和List<>集合对象)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T JsonConvertDeserializeJsonToObj<T>(string strJson)
        {
            T oRes = default(T);
            try
            {
                oRes = JsonConvert.DeserializeObject<T>(strJson);
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Fatal(string.Format("JsonConvertDeserializeJsonToObj反序列化失败,语句：{0}", strJson), ex, @"Frame\Newtonsoft.Json");
            }

            return oRes;
        }

        /// <summary>
        /// 将Json数组转换为实体集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstJson"></param>
        /// <returns></returns>
        public static List<T> JsonLstToObjs<T>(List<string> lstJson)
        {
            List<T> lstRes = new List<T>();
            try
            {
                foreach (var strObj in lstJson)
                {
                    //将json反序列化为对象
                    var oRes = JsonConvert.DeserializeObject<T>(strObj);
                    lstRes.Add(oRes);
                }
            }
            catch
            { }
            return lstRes;
        }



        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                StringReader sr = new StringReader(json);
                object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
                T t = o as T;
                return t;
            }
            catch (Exception ex)
            {
                Log4netUtil.Log4NetHelper.Fatal(string.Format("DeserializeJsonToObject反序列化失败,语句：{0}", json), ex, @"Frame\Newtonsoft.Json");
                return null;
            }
        }


        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }


        #endregion

        #region StortJson 排序
        /// <summary>
        /// json 排序
        /// </summary>
        /// <param name="json"></param>
        /// <param name="isDescending"></param>
        /// <returns></returns>
        public static string StortJson(string json, bool isDescending)
        {
            var dic = JsonConvert.DeserializeObject<SortedDictionary<string, object>>(json);
            SortedDictionary<string, object> keyValues = new SortedDictionary<string, object>(dic);
            if (isDescending)
                keyValues.OrderByDescending(m => m.Key);//降序
            else
                keyValues.OrderBy(m => m.Key);//升序 把Key换成Value 就是对Value进行排序
            return JsonConvert.SerializeObject(keyValues);
        }

        public static string StortJson(string json)
        {
            var jo = JObject.Parse(json);
            var target = KeySort(jo);//排序
            //var s = string.Join("", GetValue(jo));
            return JsonConvert.SerializeObject(target);
            //return s;
        }
        #endregion

        #region GetValue 排序并取值
        /// <summary>
        /// json 排序并取值
        /// </summary>
        /// <param name="jo">JObject</param>
        /// <returns></returns>
        public static List<string> GetValue(object jo)
        {
            var res = new List<string>();
            var sd = new SortedDictionary<string, object>();
            switch (jo.GetType().Name)
            {
                case "JValue":
                    res.Add(string.Format("{0}", (jo as JValue).Value));
                    break;
                case "JObject":
                    foreach (var x in jo as JObject)
                    {
                        sd.Add(x.Key, x.Value);
                    }
                    foreach (var x in sd)
                    {
                        res.Add(x.Key);//只取排序后的值需注释掉
                        res.AddRange(GetValue(x.Value));
                    }
                    break;
                case "JArray":
                    foreach (var x in jo as JArray)
                    {
                        res.AddRange(GetValue(x));
                    }
                    break;
            }
            return res;
        }

        /// <summary>
        /// json 排序
        /// </summary>
        /// <param name="jo">JObject</param>
        /// <returns></returns>
        public static SortedDictionary<string, object> KeySort(JObject obj)
        {
            var res = new SortedDictionary<string, object>();
            foreach (var x in obj)
            {
                if (x.Value is JValue) res.Add(x.Key, x.Value);
                else if (x.Value is JObject) res.Add(x.Key, KeySort((JObject)x.Value));
                else if (x.Value is JArray)
                {
                    var tmp = new SortedDictionary<string, object>[x.Value.Count()];
                    for (var i = 0; i < x.Value.Count(); i++)
                    {
                        tmp[i] = KeySort((JObject)x.Value[i]);
                    }
                    res.Add(x.Key, tmp);
                }
            }
            return res;
        }


        public static string GetJsonval(string input, string key)
        {
            if (String.IsNullOrEmpty(input) || String.IsNullOrEmpty(key)) return String.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Dictionary<string, object> json = serializer.DeserializeObject(input) as Dictionary<string, object>;
            object value;
            json.TryGetValue(key, out value);
            return value as String;
        }
        #endregion

        #region GetJObjectValue
        /// <summary>
        /// GetJObjectValue
        /// </summary>
        /// <param name="jObject"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string GetJObjectValue(Newtonsoft.Json.Linq.JObject jObject, string keyName)
        {
            string value = string.Empty;
            value = string.IsNullOrEmpty(jObject.Value<string>(keyName)) ? string.Empty : jObject.Value<string>(keyName);
            if (string.IsNullOrEmpty(value))
                value = string.IsNullOrEmpty(jObject.Value<string>(keyName.ToUpper())) ? string.Empty : jObject.Value<string>(keyName.ToUpper());
            if (string.IsNullOrEmpty(value))
                value = string.IsNullOrEmpty(jObject.Value<string>(keyName.ToLower())) ? string.Empty : jObject.Value<string>(keyName.ToLower());
            return value;
        }
        #endregion

        #region JsonToDataTable
        /// <summary>
        /// JsonToDataTable
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public System.Data.DataTable JsonToDataTable(string strJson)
        {
            System.Data.DataTable dt = null;
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(strJson);
                int code = (int)jo["code"];
                if (!int.Equals(code, 200))
                {
                    return null;
                }
                JArray ja = (JArray)jo["data"];
                dt = ToDataTable(ja.ToString());
            }
            catch (Exception ex)
            {
                return null;
            }
            return dt;
        }
        #endregion 

        #region ToDataTable
        /// <summary>
        /// ToDataTable
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public System.Data.DataTable ToDataTable(string json)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();  //实例化
            System.Data.DataTable result;
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
            if (arrayList.Count > 0)
            {
                foreach (Dictionary<string, object> dictionary in arrayList)
                {
                    //if (dictionary.Keys.Count<string>() == 0)
                    if (dictionary.Keys.Count == 0)
                    {
                        result = dataTable;
                        return result;
                    }
                    if (dataTable.Columns.Count == 0)
                    {
                        foreach (string current in dictionary.Keys)
                        {
                            dataTable.Columns.Add(current, dictionary[current].GetType());
                        }
                    }
                    System.Data.DataRow dataRow = dataTable.NewRow();
                    foreach (string current in dictionary.Keys)
                    {
                        dataRow[current] = dictionary[current];
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }
            result = dataTable;
            return result;
        }
        #endregion 

        #region ConvertJArrayToDataTable
        /// <summary>
        /// ConvertJArrayToDataTable
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public static System.Data.DataTable ConvertJArrayToDataTable(Log4netUtil.LogAppendToForms logAppendToForms,
                                                               Model.JobEntity jobInfo, JArray dataArr)
        {
            if (dataArr == null || dataArr.Count <= 0)
            {
                string logMessage = string.Format("【{0}_{1}】 ConvertJArrayToDataTable转换失败!失败原因：JArray为空 ", jobInfo.JobCode, jobInfo.JobName);
                Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                resultJObject.Add("code", 999);
                resultJObject.Add("msg", logMessage);
                resultJObject.Add("data", dataArr.ToString());
                logMessage = string.Format("【{0}_{1}】 ConvertJArrayToDataTable转换失败! ", jobInfo.JobCode, jobInfo.JobName);
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, jobInfo.IsDebug, logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
                return null;
            }
            System.Data.DataTable resultDt = new System.Data.DataTable();
            try
            {
                var colnames = ((JObject)(dataArr.First)).Properties();
                List<string> columnNames = new List<string>();
                if (colnames == null || colnames.Count() <= 0)
                {
                    string logMessage = string.Format("【{0}_{1}】 ConvertJArrayToDataTable转换失败!失败原因：JArray Colnames为空 ", jobInfo.JobCode, jobInfo.JobName);
                    Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                    resultJObject.Add("code", 999);
                    resultJObject.Add("msg", logMessage);
                    resultJObject.Add("data", dataArr.ToString());
                    logMessage = string.Format("【{0}_{1}】 ConvertJArrayToDataTable转换失败! ", jobInfo.JobCode, jobInfo.JobName);
                    Log4netUtil.Log4NetHelper.LogError(logAppendToForms, jobInfo.IsDebug, logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
                    return null;
                }
                foreach (var item in colnames)
                {
                    if (!columnNames.Contains(item.Name))
                        columnNames.Add(item.Name);
                    resultDt.Columns.Add(item.Name, typeof(string));
                }
                foreach (JObject data in dataArr)
                {
                    JObject jo = JObject.Parse(data.ToString());
                    System.Data.DataRow row = resultDt.NewRow();
                    foreach (var columnName in columnNames)
                    {
                        if (jo.Property(columnName) == null)
                        {
                            data.Add(columnName, "");
                            row[columnName] = data[columnName].ToString();
                        }
                        else
                            row[columnName] = data[columnName].ToString();
                    }
                    resultDt.Rows.Add(row);
                }
                return resultDt;
            }
            catch (Exception ex)
            {
                string logMessage = string.Format("【{0}_{1}】 ConvertJArrayToDataTable转换失败!失败原因：{2} ", jobInfo.JobCode, jobInfo.JobName, ex.Message);
                Newtonsoft.Json.Linq.JObject resultJObject = new Newtonsoft.Json.Linq.JObject();
                resultJObject.Add("code", 999);
                resultJObject.Add("msg", logMessage);
                resultJObject.Add("data", dataArr.ToString());
                logMessage = string.Format("【{0}_{1}】 ConvertJArrayToDataTable转换失败! ", jobInfo.JobCode, jobInfo.JobName);
                Log4netUtil.Log4NetHelper.LogError(logAppendToForms, jobInfo.IsDebug, logMessage, string.Format(@"Api\{0}", jobInfo.JobCode));
                return null;
            }

        }
        #endregion

        #region ConvertJsonToJArray
        /// <summary>
        /// ConvertJsonToJArray
        /// </summary>
        /// <param name="logAppendToForms"></param>
        /// <param name="jobInfo"></param>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static JArray ConvertJsonToJArray(Log4netUtil.LogAppendToForms logAppendToForms,
                                                               Model.JobEntity jobInfo, string strJson)
        {
            JObject jObject = JObject.Parse(strJson);
            IEnumerable<JProperty> property = jObject.Properties();
            JArray jArray = new JArray();
            foreach (JProperty item in property)
            {
                JObject child = JObject.Parse(item.Value.ToString());
                jArray.Add(child);
            }
            return jArray;
        }
        #endregion

    }
}
