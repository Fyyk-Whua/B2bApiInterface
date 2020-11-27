using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Windows.Forms;

namespace B2bApiInterface
{
    static class Program
    {
        private static System.Threading.Mutex mutex;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            //  string result = "{\"customerId\":\"fcc50855b5fa4d58acb9cce2a1157047\",\"loginToken\":\"b20347ddd95b48478ed76355b698281d\",\"password\":\"123456\",\"phone\":\"17786139501\",\"sign\":\"\",\"versionCode\":\"1.1.5\"}";
            string result = "{\"loginToken\":\"b20347ddd95b48478ed76355b698281d\",\"password\":\"123456\",\"phone\":\"17786139501\",\"sign\":\"\",\"customerId\":\"fcc50855b5fa4d58acb9cce2a1157047\",\"versionCode\":\"1.1.5\"}";
            Newtonsoft.Json.Linq.JObject resultJObject = Newtonsoft.Json.Linq.JObject.Parse(result);
            string json = Util.NewtonsoftCommon.SerializeObjToJson(resultJObject);

            string key = "FYYK.PSY.Inv"; //key
            string orderJson = json;// Util.NewtonsoftCommon.StortJson(json, false); //排序
            string sign = Util.FyykMD5FileUtil.GetMD5Hash(orderJson + key);  //md5

            string b = "";
            /*string path = @"D:\器械图片";
            System.IO.DirectoryInfo root = new System.IO.DirectoryInfo(path);
            System.IO.FileInfo[] files = root.GetFiles();

            System.Data.DataTable dataTable = new System.Data.DataTable();
            if (!dataTable.Columns.Contains("erpGoodsId"))
                dataTable.Columns.Add("erpGoodsId", typeof(string));
            if (!dataTable.Columns.Contains("fileName"))
                dataTable.Columns.Add("fileName", typeof(string));
            if (!dataTable.Columns.Contains("imageType"))
                dataTable.Columns.Add("imageType", typeof(string));
            foreach (var item in files)
            {
                string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(item.FullName.Trim());// 没有扩展名的文件名 
                System.Data.DataRow dr2 = dataTable.NewRow();
                string filename1 = fileNameWithoutExtension.Substring(0, 11);
                dr2[0] = filename1;// fileNameWithoutExtension.Substring(0, 11);
                dr2[1] = fileNameWithoutExtension.Trim();
                dr2[2] = fileNameWithoutExtension.Replace(filename1, "").Replace("-","");
                dataTable.Rows.Add(dr2);

            }*/


            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);
            mutex = new System.Threading.Mutex(true, "优药汇电商平台同步接口");
            if (mutex.WaitOne(0, false))
                Application.Run(new frmMain());
            else
                MessageBox.Show("程序已经在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
        }

        
    }
}
