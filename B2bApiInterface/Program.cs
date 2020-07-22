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
            string appId = "CONSZE";
            string passKey = "9A7OTA7JTAAJOE3153J17TEEAAJOTJO9";
            //var dtyApi = new Util.DtyApi("https://cwmsedi.dtw.com.cn/api", appId, passKey, "Test");  //https://cwmsedi.dtw.com.cn/api/
            string controllerType = "Product";
            string requestData = "{\"MasterCode\":\"samplestring1\",\"MasterName\":\"samplestring2\",\"SpecModel\":\"samplestring3\",\"PackingUnit\":\"samplestring4\",\"PackingQty\":5,\"MinMeasureUnit\":\"samplestring6\",\"Action\":7,\"ProdSort\":\"samplestring8\",\"MedicalSort\":1,\"MedicalCategory\":\"samplestring9\",\"Manufacturers\":\"samplestring10\",\"EnterpriseLicenseNo\":\"samplestring11\",\"EnterpriseRecordNo\":\"samplestring12\",\"StorageSpec\":1,\"StorageCon\":\"samplestring13\",\"StorageHumidity\":\"samplestring14\",\"TransCon\":\"samplestring15\",\"TBEDays\":16,\"ShelfLifeDays\":17,\"OwnerCode\":\"CONSZE\"}";
            //"{\"OwnerName\":\"sample string 1\",\"BusinessLicenseNo\":\"sample string 2\",\"LegalAgent\":\"sample string 3\",\"Alias\":\"sample string 4\",\"EngName\":\"sample string 5\",\"IndexCode\":\"sample string 6\",\"ShortName\":\"sample string 7\",\"Country\":\"sample string 8\",\"Prov\":\"sample string 9\",\"City\":\"sample string 10\",\"Addr\":\"sample string 11\",\"ContactPerson\":\"sample string 12\",\"ContactNo\":\"sample string 13\",\"Email\":\"sample string 14\",\"Remark\":\"sample string 15\",\"LicenseNo\":\"sample string 16\",\"RecordNo\":\"sample string 17\",\"BusinessScopes\":[{\"MedicalCategory\":\"sample string 1\",\"MedicalCategoryName\":\"sample string 2\"},{\"MedicalCategory\":\"sample string 1\",\"MedicalCategoryName\":\"sample string 2\"}],\"OwnerCode\":\"sample string 18\"}";
            //Model.Product product = Util.NewtonsoftCommon.DeserializeJsonToObj<Model.Product>(requestData);
            //requestData = Util.NewtonsoftCommon.SerializeObjToJson(product);
            //var result = dtyApi.DtyEDIApiByJson(controllerType, requestData);
            //return;
            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);
            mutex = new System.Threading.Mutex(true, "OnlyRun");
            if (mutex.WaitOne(0, false))
                Application.Run(new frmMain());
            else
                MessageBox.Show("程序已经在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
        }
    }
}
