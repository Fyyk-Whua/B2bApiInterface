using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Xml;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Registered
{

    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
      
    
        }

        /*
        string str = "你好";
        var buff = System.Text.Encoding.Unicode.GetBytes(str);
        string str2 = Convert.ToBase64String(buff);//加密str2
                                                   //=================下面是解码的部分
        buff =Convert.FromBase64String(str2);
            str=System.Text.Encoding.Unicode.GetString(buff);
            */
        /// <summary>
        /// 生成请求码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                string strendrq = DateTime.Now.ToString("yyyy-MM-dd");
                string strRandom = NewGuid.NextRandom(10000, 1).ToString();
                string i = this.textBox1.Text.ToString();
                string n= this.tbx1.Text.ToString();
                

                if (string.IsNullOrEmpty(i))
                {
                    MessageBox.Show("请填写企业Id！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    return;
                }

                if (string.IsNullOrEmpty(n))
                {
                    MessageBox.Show("请填写企业名称！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    return;
                }

                var buff = System.Text.Encoding.Unicode.GetBytes(n);
                string str2 = Convert.ToBase64String(buff);

                JsonParser.JsonRegister jsonRegister = new JsonParser.JsonRegister();
                jsonRegister.I = i;
                jsonRegister.R = strRandom;
                jsonRegister.M = GetModuleCode();
                jsonRegister.N = str2;
                jsonRegister.T = strendrq;

                var registerEntity = new JsonParser.JsonRegister
                {
                    I = EncodeBase32(jsonRegister.I.Trim()),
                    R = EncodeBase32("Y" + jsonRegister.R),
                    M = EncodeBase32(jsonRegister.M),

                    N = EncodeBase32(jsonRegister.N),
                    T = EncodeBase32("K" + jsonRegister.T),
                    Info = 0
                };
                string strCode = Newtonsoft.Json.JsonConvert.SerializeObject(registerEntity); //序列化成JSON  
                                                                                              //string p = DecodeBase32(registerEntity.P);
                string strEncode32 = EncodeBase32(strCode);
                string strEncode = RsaEncrypt(strEncode32);
                string strAesEncode = AesClass.AesEncrypt(strEncode);
                this.richTextBox1.Text = strAesEncode;
            }catch(Exception ex)
            {
                MessageBox.Show("操作失败！原因："+ ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return;
            }

        }

        /// <summary>
        /// 生成激活码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string strjsonRegister = this.richTextBox3.Text.ToString().Trim();
            if (strjsonRegister.Trim().Equals(string.Empty) || strjsonRegister.Length == 0)
            {
                MessageBox.Show("请选解析请求码！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return;
            }
            string strendrq = Start_dateTimePicker.Value.ToString("yyyy-MM-dd");
           
            string strRandom = NewGuid.NextRandom(10000, 1).ToString();
            JavaScriptSerializer js = new JavaScriptSerializer();
            JsonParser.JsonRegister jsonRegister = js.Deserialize<JsonParser.JsonRegister>(strjsonRegister);

            var buff = System.Text.Encoding.Unicode.GetBytes(jsonRegister.N.ToString().Trim());
            string str2 = Convert.ToBase64String(buff);//加密str2

            jsonRegister.I = EncodeBase32(jsonRegister.I.Trim());
            jsonRegister.R = EncodeBase32("Y"+strRandom.ToString().Trim());
            jsonRegister.M = GetModuleCode();
            jsonRegister.M = EncodeBase32(jsonRegister.M);//(jsonRegister.M.ToString().Trim());
           
            jsonRegister.N = EncodeBase32(str2);
            jsonRegister.T = EncodeBase32(strendrq.ToString().Trim());

            JsonParser.JsonSign jsonSign = new JsonParser.JsonSign();
            jsonSign.data = jsonRegister; //js.Serialize(jsonRegister).Trim(); //序列化成JSON  
            string registerStortJson = StortJson(js.Serialize(jsonRegister), false); //排序
            jsonSign.sing = Sign(registerStortJson);

            string strJsonCode = js.Serialize(jsonSign).Trim(); //序列化成JSON  
            //string strEncode32 = EncodeBase32(strJsonCode);
            string strEncode = RsaEncrypt(strJsonCode); //RSAFromPkcs8.BlockEncrypt(strEncode32);//privateKey 加密
            string strAesEncode = AesClass.AesEncrypt(strEncode);
            string strEncodeBase32Code = EncodeBase32(strAesEncode);
            this.richTextBox2.Text = strEncodeBase32Code;

            //BANLF7GEKN5MKJ7LK6SRDQIP6AJL367T6LV53Q4VYAALSSBEYNRPJ8GUYSV5UMJMCXY83IGCA67QM8BVCX7MQXJJCAMAL8JGDH7AUTJTKYULV383FTSRUXTSKYSM3TK3KT8PLTNQCI48KITTKNKAGVKJ6CAUQJGFKAF4HP4J6PTPUPGAKFSQ3PTIFPNM3INQKY7AJSKN6WT4UJN3DAAQD3B3KA3MW3BABNGUG3J3BAILWCN8ACYRD7NJUPWA38JYKFRQD8JEUTY83Q45AXR8DCVB63W56XG8CANMKMTBF3W8GJVQUT6RFXJA6YHRKBVE6AS4LIVSKPRQ9I4QBYW5GC74YA8GQIGKDXQMMD4EUATUVIKUCTCAD74AYP7RMQ86FXXUDQ4IFTBQLJJ4FIYR6TYSFXFQUWVAFCUAJ6TICY8RDMKFKPTLQSBNENA8LTJQEYIMW874UC4UFPYIAXT86XTTYT58G6BLENJADP4P6IBMKQ4SEAWRDKTEBYR83TTJYY5MQVJAYX4LVWYIDPJR63ISUXA5DTBK6CLUM6KEUI7MU8Y3FIPMF8JD6IFMSCJUUTBADXJNKXGUKI4A6CTPUDBCCY8QFJ78UTTL6XVI6ISMQXIPCNQA6Q4JUTPQWX8JKYPPHQY36WSUDSJEEAMPM745YN6AK385FI44K8VYKIVPJ7I3CC85GDBEUNI8KDGUDP4LDT46YMVMM7KKFTHP38KQYY5U99YPKIV4GMGYKTUQD3KVYNGP367CAX8A66856TWM9X8TUAUR6WJCYIJMFCJIUP78GWNMKH7L37G5ACQU3JKS6C7G9PBMCIAL3CGRCCHRUSB36CR8GCKNEAVMWSKICCV8UKTSBAVQFK7RUTA568NSCTKAD3JMDYRQ3I74AXQ5UTIEKCFPMS7FYYK5UPVVKY44LI8FKXP4L3YIFI7MVWNPYWVMD8V6UPI8KXBGCNVLK9KRKIWR6WITFASMV9K3FANMDP8DDY7M68VQEAM4JJGACAIAF7GUDXPGQJ85FXFL36KFCYPU6DG3UYSRJBV4DYPA3QBLEYQ4DIVIDN7PMMGICA74DK76CAJRMIG4YNNMUW7D6AR5UMY8ENNMLMIRUNPRJM4UKY7RJX4LDFV8JIBJKPY5LIJGCY7L67BVUXVUU9KQUHTLMWGCUIYL9V45CNP4FDBNCAI5HPYMYMVRG8JLENV5FTJ5D3PPK743YIW8F9IJCXKM3C7DDISGQWNJFXWLFXKRBNQRGDK4FCT4U3YTKY7A3W7VCXWAHJJ4DHVM6WVNY3WLGX8AYXR4G7BMY6VMLQKJFS7Q6T8LUIFMGS7VDY7QUMVPCIJ5KVTMELVG98K36I3QWJ75CNAAJV8BFX783CJLDF7LUPVRU3RQ3IVDYY6QMVVVCAQL96G4FAMAJTJAUPGAGCGPKPI5LTK8KPP5LT85DC6UDIY3DC78J8JGKYS8KW4YFAUQJI8B6T65FKTQYPT83QKSDYBU6DBVFXFLFQNSBAJPLSBCDXML3SBY6CR4KQJ6YT6QU3BYUIYALXYE6CBLJ884BATQ3BV4BYYQDJ4NKTVLFXJS6PNMUCJ4YX4L68NIYN5L6C44CY7Q38NJ6XHMV7GU6Y5R66JYDSRRGQ8T6SSQ3TI8DCWMFW8AUIIAL7YT5NRR66KQYFWRDCVMDSVMFTNSFYYMUWGEYAMQGK7TYPGPFXNSYXUU69IBUNW5LBVMYXKQDWKNDYUAGSYIYX8UWXVSUAURDC4UDAGUM64SCX3R36VTCXUMJSY3YCFAUDNMCAI5JPJU6IIU39GAFCW537I3CWRPFMNM6I6MSMBGUAQ43TTS6P7QVQBPKNVUMMBLKTNLQXGSKIRRMSBDUNHRM9BGUANUGVIJUALQG98SACB5MCNQKIUQDDBJUNG43WK4FTK43884KYIQ6645KLRP6XB36PNM6C8NYMTAM68PDPLLK9BDBN5UJW7REAAQDQ4FCXJQVT83Y3WPMTYSBYYLKV8Y6AFUD7IB6PBMJCGQBL7A6W73ENQ53W88FILQSTBUBN4A3T7PEAH8G68YEAFLF9YTDXWLSPJ86WSRHPJKDW7PD6TJDPWLUXVRUWS4UJYIDN7LDS7CYNFQM38F6TAULX8YFMTRHPJSKIYQFVGJDCWUDTVVACHQD6GTYF7G967QCMTPGMVFFYYGQWK5FIRLU6BSUM7PJIBKKNGR6W4EUYQ5FW8K6I3Q6J8CYX8LQWIPFXMMJSKNU3TAJQ8SDXS46MGPKNCAM9B3AXG43X8VDNP4F3KCFAL5F3TM6TKAHI4MKLTU6VIIACBAFC4K6XJADDY36PALSQ8JDFWM6X8DEFPRDXNIUSRRK74VCARM9SKP6Y7AJQ4QAX8PLWVFYXPRDPVYUIHPUPGC6WTL66VJF6WR6Q4J5A8PMJJNFPRUSX8VFTKUG6BKYHTR6XJMKPQ4HMT8YFWL9CGB6Y4L6XJLDNV8GJ8VYCFUQCNTYTKAH3VR5HWU678JDXN53IJKDSTLM7TTUSSMQITRUXCUUMIMFAKQQ38DKPPMV88CUAHMLJ7CYAXUQD4SDPM5HVNMUCU4LQGACXLAHIKKYAKJQQKJCNB4JIGNUCWU98YBCAA5F8VA6PNMJDYSCI4UFSJJBAF56V7JUMW8GC74DPX4JC4PF6WALINBA678HCNRKXLQUXIBBAYP6XKNFCLR6MYSFXNAUXBTFAILLIBG6PSQ9XK5CLTAUMGGDHWALQJ8FCWMS8G5UXVM38GD5NLQQ3B8DTI5L98TYMVPLV7DKTNQQ6GS6CAUUDG46Y4UU8K6FTTLVIVNCTYULW7YAXP4JW4PCXMLGCJFYPC4DCKIUNQ5M68G63RM6CGLDYP4UW8NYYVQK94CDT54FCBAAC44HKTQFCPMLC88KH7RG88FDYKAFMT3DAQ83KVSCCPU6M7ACPTAK88TBAM5GSB4DCGAM3BRCPQAK68PDSWPGMKCUT55HPVYUY6A3V7GYSS8FWIS6AMLGJB3BYY5KIGPCAGMKMNTKASRLTVYDX5A68T3YYUQ3J7DUNYPHXN35NJQFJ7RUCTMM3B8FYIAUVV3UP4UVQGG5LTQS9GTFI7QSDBGCFP8GW8AUPLMVJGQCABRG3YPKA5QLMNP5NM5K67BUI7UKWKKYMR83CG8DP55K9IBYPMUJMJ4FSVQ66JLULV5L38DFALU9SIMAXYPH7T3CCVU3CIRKCAMGCNSCWVRD7YIAXS5KCVECITRU77CC3PMV9YPF6VMU7KIFY6L384JDY6RM8NM5AFP698KDYAMFCGECMV8KTVADSVPFB7DBY5QMJ8JEN7UFW85UFRM6QNRU3TLUSBC6TTMW9BLFCU4FQBTFMVLQJVLCMVAJM8QCAKQ69YEUTUAH6JFCNG4LXV4CAMUV6BVCWSGQT4CFXPLKBTSKNKPJCGDFXX5HVTTUNT4FPVRUAM4UQ8EFT38HSKDCYCAM7B6YPMAG9B6YN746VJRAX54H9YQCCGMKWVPDF75H88IEA4QUXI3CWPMV6VUYNGUKW74FXI53PBYDCFJQ77T6PMQF67MY6VAMM7J6CWPLP4RYP5UQCNMKIKAMI7Q6IP56XBJ6CTP668PKLRJQ8NEBA6QSSJ3FATUMTT8YXFQG9NIUSS43VTRFPLL9D4V5ARUKB7DYP75DWBIYXAQUJ8NFF7ULQBAFILQ9JB5YSW4JWJFEAWLWQKSACMMQ7KGDWVLKTV85HSR6PBGKH7PDQJUKPCU3XVLEAS863736MRRLP4Q63W8D84Y6XT8G9IIFYAQKINPCCVMD8JDKX5MFTK6EAQMVJBPDPALQ7JNDYSRLSJ6EN6LSWV3BHTUMVVYKAVUJJVDCPHQS3IEDWWLGS88DAJLDVVPUFTPLM4VBYVU3BV3UTSAFXIBUAUM3XJ4FXRQW37ADHRPLI4JYTPR6VJEKP6RGTG5FY7US9JFCNBMF9BG6CUMVVVJKLVQDCGCDMTLWWJQCP7UU7KAYTPLVQG3UNVAFVISFI6Q36G3CYVQMCJMUWS4L8GE6TFUKWKVEY653MBYUATU3CGVKMTLDX7KFYS4K9IIDSV8H38ADYVUJTGC6AVML78TKHTUVDGYCWW8DMTBCLSMQP4KCPT4J3JDFCKU9DG6DLVMWVJ4DAKAFJV4UTNRGVKAEAU5KQ4DBA8QGTVRDPR56I75CWS5FJTI6FPRUCBB5A74JDYJ63TPLVB3CMSMGM7LCHSU3B7VBYUUD9G6CYC5GC83FTJMFPKVDYS8DJVECL7UGWNTKSWPFC44FCQ4FDKD6XWUFMNS6FPQMXITUPX4FCTRENLRH8VICXBQWKTJKXQRJ7J66T4ULQ86FPVALT4SFY3QGXNEUCVU9JGIUCY56QYICCMMVW4NYTKLDP7CUIPAUV46YAYLLXI36IFH
        }

        /// <summary>
        /// 解析请求码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string FeedbackCode = this.richTextBox1.Text.ToString().Trim();
            if (FeedbackCode.Trim().Equals(string.Empty) || FeedbackCode.Length == 0)
            {
                MessageBox.Show("无反馈码，请填写反馈码！" , "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);  
                return;
            }

            JavaScriptSerializer js = new JavaScriptSerializer();
            string strAesDecode = AesClass.AesDecrypt(FeedbackCode);
            string strRSADecode = RsaDecrypt(strAesDecode);
            string strBase32Decode = DecodeBase32(strRSADecode);

            string str = GetLastStr(strBase32Decode,1);
            if (str != "}")
            {
                strBase32Decode = RemoveLastChar(strBase32Decode, ",\"Info\":");
                strBase32Decode += ",\"Info\":0}";
            }

            JsonParser.JsonRegister jsonRegister = js.Deserialize<JsonParser.JsonRegister>(strBase32Decode);
            string jsonRegister_I = DecodeBase32(jsonRegister.I.ToString().Trim());
            string jsonRegister_R = DecodeBase32(jsonRegister.R.ToString().Trim());
            string jsonRegister_M = DecodeBase32(jsonRegister.M.ToString().Trim());
           
            string jsonRegister_N = DecodeBase32(jsonRegister.N.ToString().Trim());
            string jsonRegister_T = DecodeBase32(jsonRegister.T.ToString().Trim());

            var buff = Convert.FromBase64String(jsonRegister_N);
            string str2 = System.Text.Encoding.Unicode.GetString(buff);


            jsonRegister.I = jsonRegister_I;
            jsonRegister.R = jsonRegister_R;
            jsonRegister.M = jsonRegister_M;

            jsonRegister.N = str2; // jsonRegister_N;
            jsonRegister.T = jsonRegister_T;

            this.richTextBox3.Text = ConvertJsonString(js.Serialize(jsonRegister));

        }

       
        /// <summary>
        /// 解析激活码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string FeedbackCode = this.richTextBox2.Text.ToString().Trim();
            if (FeedbackCode.Trim().Equals(string.Empty) || FeedbackCode.Length == 0)
            {
                MessageBox.Show("无反馈码，请填写反馈码！", "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
               // return;
            }
            try
            {
                string strDecodeBase32Code = DecodeBase32(FeedbackCode);
                
                JavaScriptSerializer js = new JavaScriptSerializer();
                //strDecodeBase32Code = "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";
                //string strDecodeBase32Code1 = DeleteControlChar(strDecodeBase32Code);
                string strAesDecode = AesClass.AesDecrypt(strDecodeBase32Code);
                string strRSADecode = RsaDecrypt_(strAesDecode);//RSAFromPkcs8.BlockDecrypt(strAesDecode);
                string strBase32Decode = strRSADecode;// DecodeBase32(strRSADecode);

                string str = GetLastStr(strBase32Decode, 1);
                if (str != "}")
                {
                    //strBase32Decode = RemoveLastChar(strBase32Decode, "\"}");
                    strBase32Decode = RemoveLastChar(strBase32Decode, ",\"Info\":");
                    //int a = strBase32Decode.IndexOf("\"Info\":");
                    //strBase32Decode += "\"}}";
                    strBase32Decode += ",\"Info\":0}}";

                }

                JsonParser.JsonSign jsonSign = js.Deserialize<JsonParser.JsonSign>(strBase32Decode);
                string registerStortJson = StortJson(js.Serialize(jsonSign.data), false); //排序

                if (Verify(registerStortJson, jsonSign.sing))
                {
                    //JsonParser.JsonRegister jsonRegister = js.Deserialize<JsonParser.JsonRegister>(strBase32Decode);
                    JsonParser.JsonRegister jsonRegister = new JsonParser.JsonRegister();
                    jsonRegister = jsonSign.data;
                    string jsonRegister_I = DecodeBase32(jsonRegister.I.ToString().Trim());
                    string jsonRegister_R = DecodeBase32(jsonRegister.R.ToString().Trim());
                    string jsonRegister_M = DecodeBase32(jsonRegister.M.ToString().Trim());
                  
                    string jsonRegister_N = DecodeBase32(jsonRegister.N.ToString().Trim());
                    string jsonRegister_T = DecodeBase32(jsonRegister.T.ToString().Trim());

                    var buff = Convert.FromBase64String(jsonRegister_N);
                    string str2 = System.Text.Encoding.Unicode.GetString(buff);

                    jsonRegister.I = jsonRegister_I;
                    jsonRegister.R = jsonRegister_R;
                    jsonRegister.M = jsonRegister_M;
                  
                    jsonRegister.N = str2;
                   
                    jsonRegister.T = jsonRegister_T;
                    string jsonCode = ConvertJsonString(js.Serialize(jsonRegister));
                    this.richTextBox3.Text = jsonCode;
                }
            }catch(Exception ex)
            {
                MessageBox.Show("错误 ！"+ ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        #region Base32
        #region DecodeBase32
        /// <summary>
        /// DecodeBase32
        /// </summary>
        /// <param name="strEncode32"></param>
        /// <returns></returns>
        private string DecodeBase32(string strEncode32)
        {
            if (string.IsNullOrEmpty(strEncode32))
                return string.Empty;
            byte[] arrayDecode = new byte[1];
            arrayDecode = CBase32.Decode(strEncode32);
            string strDecode32 = Convert.ToString(System.Text.Encoding.ASCII.GetString(arrayDecode));
            strDecode32 = strDecode32.Replace("\0", string.Empty);
            return strDecode32;
        }
        #endregion

        #region EncodeBase32
        /// <summary>
        /// EncodeBase32
        /// </summary>
        /// <param name="strDecode32"></param>
        /// <returns></returns>
        private string EncodeBase32(string strDecode32)
        {
            byte[] arrayEncode = new byte[1];
            arrayEncode = System.Text.Encoding.ASCII.GetBytes(strDecode32);
            string strEncode32 = CBase32.Encode(arrayEncode);
            return strEncode32;
        }
        #endregion

        #endregion

        #region ConvertJsonString
        /// <summary>
        /// ConvertJsonString
        /// </summary>
        private string ConvertJsonString(string str)
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

        #region RSA

        #region 声明变量
        private const string _RegSignPublicKey = @"<RSAKeyValue><Modulus>y8Nq6ZG2FLQLc8NoSSWXcyr3pVWH2+IULoaB6H0OIe9iZwaVqBHm36pGQF6afT+2o5RxtWlV74IbRaP70CvD5SFQDcNCU6K95rB/MPnekGbQ4N2mQq9YFu0HxlEV6Dufb63Rd6Rew51dx5b4nju76aRMjQ4RQKHfcE18bJalq0bMS6t0NK2ZSfGuS0cISA/FVZ4CUXyewATphLJ4YecFtr32/Xdin5Sm5EoQxWPRsYPgehnQEc/WPeTCZSZEmRS2f+QWI6sjoh0mBpr6/opWnaYDEHUJ976bhFB5liY1Nra2u1Pg00JYJUxjCIyOQXqQUNXsRRa7ZUZquCUIuT9AsHq+KR90oKAWfrdcT5oF3XRa9ZusdvuYwtXhqYhMDDDjV3aznv/6cO6igPVhkNBRDDBlAKzhBopBm0dtejhSlCTpCS+rQvUE1ceGlJYqEPH/RnjQb7UVRfJFkJ4YxfMHNNEdm7yEenZamFKo0HibxXbhXlfSWb6kKnoUoM4ehA1Zjvijql+m+mU1QdkZDwXIVxNp2ZP4nAmCa9z3MMmYjewcsQ7WzgXwAoXdtcNKEF8RWjvyQatSSOGo/enz/dGjNXmX+2nWz/c/y11GVCXETe8J6ZgkaoU8YPCsz0sxmRpEc8vvY5qMx0AJtrxbnRTz0WoT09Rb23no4gTr+tR2CJc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string _RegisteredSignPrivateKey = @"<RSAKeyValue><Modulus>y8Nq6ZG2FLQLc8NoSSWXcyr3pVWH2+IULoaB6H0OIe9iZwaVqBHm36pGQF6afT+2o5RxtWlV74IbRaP70CvD5SFQDcNCU6K95rB/MPnekGbQ4N2mQq9YFu0HxlEV6Dufb63Rd6Rew51dx5b4nju76aRMjQ4RQKHfcE18bJalq0bMS6t0NK2ZSfGuS0cISA/FVZ4CUXyewATphLJ4YecFtr32/Xdin5Sm5EoQxWPRsYPgehnQEc/WPeTCZSZEmRS2f+QWI6sjoh0mBpr6/opWnaYDEHUJ976bhFB5liY1Nra2u1Pg00JYJUxjCIyOQXqQUNXsRRa7ZUZquCUIuT9AsHq+KR90oKAWfrdcT5oF3XRa9ZusdvuYwtXhqYhMDDDjV3aznv/6cO6igPVhkNBRDDBlAKzhBopBm0dtejhSlCTpCS+rQvUE1ceGlJYqEPH/RnjQb7UVRfJFkJ4YxfMHNNEdm7yEenZamFKo0HibxXbhXlfSWb6kKnoUoM4ehA1Zjvijql+m+mU1QdkZDwXIVxNp2ZP4nAmCa9z3MMmYjewcsQ7WzgXwAoXdtcNKEF8RWjvyQatSSOGo/enz/dGjNXmX+2nWz/c/y11GVCXETe8J6ZgkaoU8YPCsz0sxmRpEc8vvY5qMx0AJtrxbnRTz0WoT09Rb23no4gTr+tR2CJc=</Modulus><Exponent>AQAB</Exponent><P>94N+/7ggiIJiIT4rEgH6OGlrXFO1l2m+l8Zp4fVqWBoO8My5+Nk1qGQN0gnyh9o2wvw7P0dTqEu93UjiousIcqdDu3S0HgI+jviIHNRR6zsYBBy4K5NBQHJGRpgmKA5lA2/2kHYW4W33zyGiB0YS/LOw1i5S0lytr8kuY5kuD4egEyjc6jFcmCUjC5P4F+t97GdDSMUJdfSa7VMr1dgIdS3M3ltmR/4vhGxGqObDv6rJqpn/oLtn/PK1DhLYjaxGQRW62V61iy+1jLjs/DSziFVZL2/5N30tRJfEiwSaUb7gfFgT1ddO2Z4ia0mZR83JmB0XadeGY5+bolp/1nuo2w==</P><Q>0r/pXG2rUs1wReNc2vg17NIYY0n/saTShyY7jtl/vinIKzrx/UR/Q6AZei0nOFUayMEUX+ZARKrSLgO/fdUQSlIPVSSKM+uxKi4AOOaOO5uIxagXkqMg8PMAXCM/WHWrd1p+JZULO+Z3j6S2cQ7UXLEi4OAZuDxcH5egT8HkaAKSMTd6ifTUEGDpeNJz66hXIxmm5kt85zmuCKiu6xCIikv50vxvytLdqyiFrpDvBZ2r4MBh3KCTrEadFPNx8EmDnmPsJVPrGBaO62dWh6vKVpiC/f3xATEy+2APjsEc3Io9ATDkLM19XasYQPwb6ZZa3e0xS+1IO5rw34aNyMf99Q==</Q><DP>B8euWkNjYmcWxoy5tdsyDkviAAjxkEzWnNazxVJ9gT9wcMk+nz/Um/JpLMz7PqHxTre29Qo86vFWinocBZr1rQTs8Bt+/eJ8LOpK/Pz/hjFZU+fDMjtytZ/h7Z4itOee7Ti7u1a66WMXgv8/pJLjTeYoDNNv7wTSwM/GEYNjG0HcGj4Sk5nxmyavr1F7XuUcFC46wzLOVVLW+9a9bf9YZLaH1gVxdZnbzIHKxsxaItAvfplQm7DIV/8ZCdQ10l4z5x/Tu7lqY3Ggd0foyxStAAAOyZrvbnsUzS8oEmaWozMowz/Rf8tAwz5hPpYVp1gkmg9wCPepVcBSmAvYMNm6OQ==</DP><DQ>yMIkLPYTxBcLoqfJppXX2LbyoHK3bqQSIMhc5+Fs/NuUYQoPxzHfAa6bVnV47QK1NxQmsowGIOOQwGC1o8q5b/LnxDXAqWEWLZYQhCOszj+FdLSBcCCRmrYBW8P/7eZ55oJ/tJFcWD1dG6rOWLjFt17OWOVh2s00/KtV/WQ4jpQUa2nsA0sEUG3hOkVQQ+biyv7+rFawrxuVG46EwkvHpeZmH4R1ggKJQyig4AAUkYb3WmwpTSByTCQgMvsNSNbe2J1bMNvWEeY7Uyfnl+ogH4m9DvM/B+G0LR3+9AAl3DibkGzgj4VYrUf1HMKMXGHsQYhX83of2xfn6SamHePVmQ==</DQ><InverseQ>dwNvHZyTqo29WecIhGqmVta46xGSDf4yiV0+F5JqmOtnRwSrpZnBtKg6uUTrexp0eXVRObtds1sHNIxIsgIlitEOfTGpcj7xYgdSAz16prLbn3niSoXIkkArUpla2bZ5p/fbK5HRiTmaIU/N466ic8wPz0oT86zqShg6UxE01BP/XuWNhEUmRIAVM0q26MGWhtqqvRwFj0plMh7ny0Xg9QSW3csSRihg5WoF+81WJ/cV6d642u1J4KwVEmtRWoQwLB3XKI9v3EEeMTL1X2Kej4WlL3mBty7xWWrgTyT7RnxAZKq7QGwKYTfv8ma89nJgXu3w467lpUF2qWmcYV6IEQ==</InverseQ><D>hrjPAF1R+QBNrh2d3vcW2pOnJ06UxCIHW/ec/t1oMbG36wxkeLpVXr6TMk4acQNmO2OThvF9Wx038OSKQsoc/Gr5JhBa0zd/vX4mqnga2njQVEzYd7C9WnMft9S22lRJhypym1s3OLjcX3GHMf+mr8TsxDpv177vH65rvNQh90uZGdLjw0ygVE6SAb2WUSb0PzZ4q+3sfGMDDrR9eaWolnmlS1LbB03exoPd8NSduXPLQI++jXhQW53blcsmgdw7CfYBWVX2+mGxWZ6wowlOWW+BCSmJDJ+e7W3T5h2fA2ztBpBfzu5Hn2mQ9P8Rs2NY9clrLQbpof1b5CLAUNLsblPK5ZZ8kZsFDLsDEjBszwrIbJyluRXV3ybreugY2SqnGNNaaxezNS+6COgbZpto4aGqKhs38WjkTCpWQOnc9PD0ggKrBBvcsQECKjeUDLvWRS2mTv6HwigtY1zqETHFWYol8mCKbIp30y3lJ5hvQr5WMMY4pKXLUa7rs9f0Ly5w6wdYmAOTpE63vVghwDteAjSZl2atOF83RdhYhzq5OU4wLSriHwhFCMsi7ApZDX/3XpUP5a4d8lKFKxsc4HtL5yAAO6qXQx+PlMv/TfHr0Q5Fre1Hey6imPw8vPShMr+27wppF2xZehYARkbQZQCFZ0H26V+xWVNg/rLkz49jn1k=</D></RSAKeyValue>";
        private const string _RegSignPublicKey1 = @"-----BEGIN PUBLIC KEY-----"
+"MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAy8Nq6ZG2FLQLc8NoSSWX"
+"cyr3pVWH2+IULoaB6H0OIe9iZwaVqBHm36pGQF6afT+2o5RxtWlV74IbRaP70CvD"
+"5SFQDcNCU6K95rB/MPnekGbQ4N2mQq9YFu0HxlEV6Dufb63Rd6Rew51dx5b4nju7"
+"6aRMjQ4RQKHfcE18bJalq0bMS6t0NK2ZSfGuS0cISA/FVZ4CUXyewATphLJ4YecF"
+"tr32/Xdin5Sm5EoQxWPRsYPgehnQEc/WPeTCZSZEmRS2f+QWI6sjoh0mBpr6/opW"
+"naYDEHUJ976bhFB5liY1Nra2u1Pg00JYJUxjCIyOQXqQUNXsRRa7ZUZquCUIuT9A"
+"sHq+KR90oKAWfrdcT5oF3XRa9ZusdvuYwtXhqYhMDDDjV3aznv/6cO6igPVhkNBR"
+"DDBlAKzhBopBm0dtejhSlCTpCS+rQvUE1ceGlJYqEPH/RnjQb7UVRfJFkJ4YxfMH"
+"NNEdm7yEenZamFKo0HibxXbhXlfSWb6kKnoUoM4ehA1Zjvijql+m+mU1QdkZDwXI"
+"VxNp2ZP4nAmCa9z3MMmYjewcsQ7WzgXwAoXdtcNKEF8RWjvyQatSSOGo/enz/dGj"
+"NXmX+2nWz/c/y11GVCXETe8J6ZgkaoU8YPCsz0sxmRpEc8vvY5qMx0AJtrxbnRTz"
+"0WoT09Rb23no4gTr+tR2CJcCAwEAAQ=="
+"-----END PUBLIC KEY-----";
 
        private const string _RegisteredDePublicKey = @"<RSAKeyValue><Modulus>z96OSCdZS3HAUk1ikqbcNjJbycwmRVgrmiufixprxKWl8XNPUwc5eVkkuh5Aiw+ENDM2dkW/edx73aKopMWFgj7HA7l08orGhJMj7BhvKkJjOu773n3ayoLfX9heh+/CAOAHK+qDoNG7EZ9urS0kq/Y5xUKe3/c1vuVqx1vPVdi+MVbJADIWLoN62WcpwyEHI2MJH75EKTYjpRQVTdV8MiWcltlOPcF1rrZZbMNG4KVaJzz5XmbUQsmAiAQSyOCiFYfikT0agqIWM5d7Jh08qyUGjix32tE4a2OAbsyNaWVJdue3XNXfLznYGhvCPC2+2H99l79gqPt55ACZgwwIIysIg51kCKKd3wmp0AJoh/SCEPKYn+C/11cIPMZDIx36Vo7DfRQkj9tAy0iu82bw1Qbdm1yQQQZagpci6Fgloa+OCB5aXEf1WdDRxJi1+CLbCoRjFhezVDfFvDNSNaUZUiIYE0iy22zWOOWMDOlRwLOVeGpunzgMRV+FzAPdF+O8Ifz/yqB4E11oL9JW0BwDQ4VCFx6EYlP5353bXnT6hAchk/h4OomJ4nUKZu/L82Xu7/R41cKzA805MLUQSzmZqfLDy01WzSP41MPTKqiI9tVoUcqBH6vUju1tRMa6JmD8aq7otW9l3Tel4KljRBdHN9SuKCk1OaUmsYnYYse/99c=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string _RegDePrivateKey = @"<RSAKeyValue><Modulus>z96OSCdZS3HAUk1ikqbcNjJbycwmRVgrmiufixprxKWl8XNPUwc5eVkkuh5Aiw+ENDM2dkW/edx73aKopMWFgj7HA7l08orGhJMj7BhvKkJjOu773n3ayoLfX9heh+/CAOAHK+qDoNG7EZ9urS0kq/Y5xUKe3/c1vuVqx1vPVdi+MVbJADIWLoN62WcpwyEHI2MJH75EKTYjpRQVTdV8MiWcltlOPcF1rrZZbMNG4KVaJzz5XmbUQsmAiAQSyOCiFYfikT0agqIWM5d7Jh08qyUGjix32tE4a2OAbsyNaWVJdue3XNXfLznYGhvCPC2+2H99l79gqPt55ACZgwwIIysIg51kCKKd3wmp0AJoh/SCEPKYn+C/11cIPMZDIx36Vo7DfRQkj9tAy0iu82bw1Qbdm1yQQQZagpci6Fgloa+OCB5aXEf1WdDRxJi1+CLbCoRjFhezVDfFvDNSNaUZUiIYE0iy22zWOOWMDOlRwLOVeGpunzgMRV+FzAPdF+O8Ifz/yqB4E11oL9JW0BwDQ4VCFx6EYlP5353bXnT6hAchk/h4OomJ4nUKZu/L82Xu7/R41cKzA805MLUQSzmZqfLDy01WzSP41MPTKqiI9tVoUcqBH6vUju1tRMa6JmD8aq7otW9l3Tel4KljRBdHN9SuKCk1OaUmsYnYYse/99c=</Modulus><Exponent>AQAB</Exponent><P>6Pu48LZM/eNBpVnSDUCm1K/rig5vl7vw/angmAxydEiZxbFKKA9JpGdTmblgtQTQ/ivDMe2B56QjY8caw9UASo3oxd6rwexZUn36atxvGB+EJ51YM0i0p+lfFp05mkva3CjsbymOBN5SNrcOeYwkLXrqTsoWrTpIWXreZLBVftAUgmJ1c7Ia2m0b7ifp1ZAsThB+eKzApACqMH2UYXkyLLxJhGsM6F3GjZ4eKLz5ZruPd9M2r8xeAoITEvhAMkmgQ7qQJ+kI3D+ypDB0o1hX0bFX5JRfRgLL35I00/vpm6pFZHTwl+JkmX12XfsS9RwmNMZ/a8cmxsWhLgdA2EILow==</P><Q>5GewIDIGKI2GjvWB41oJZfI0gtbZjlIxsVxZCPWYRF0gDIHwETKH9VK4DvqCvoykvAyMFCBIbAHI4/n48lq+Tq8/7tKVVScKY956p+87qxa5gk3uNypsNMwotnCBTN0JjACCfwIwPKWP8hCA37aYLiF8R6cQYUYbKQp8Q8JMLt5uQRWJDgw+z2LWUjaxsa+9XmeJcFHuss8+0Xme1coqXDFu6SXbg/K6muCuMs9QtQs4OLhIlo8k/4fLl7vkvFI3hnJ/Lw9OLbWaMSMUyJGGrzcxVs8uwS7m+Uq5IYWOECCn0S30cW17yrM7tyzs0bXjk5lSKoXgR9LqfDJYaSMmPQ==</Q><DP>E0ARRNpbPDMVznrAb1XjMvmiJZMRx2DBBcSOiSGmJ1OEWSBP90VkGVBsSOxXQD24ovestihgrmoSfoEKBhpIXuCg1hCS8n/71WQRV9kE2OJpwfgvPHWKb8FJmQ2+n7Aa0kwTVRAC6wYPlvPDH2nj51obmAz8mK2TIsmTLJChT8wTlb5a5AdYTqnrP99OY9X4wy57tK7Zb/OaHE2UAAXKjoW0MVvDAkQVTsg8x7LtjH582TK7dwUU03I57zxR2ZXZxx7YIGQR1ljxAr36NTDseKgFkh5sTNWYUM28zbMn1zPXbfh3lKUhGMmUCSngpB4CTiQEjTw0SQI1Uh9JTVUPrw==</DP><DQ>ZC7+2ABZJyx8mvQg1uJFQQwt8D3hC0YOOedxvjZLZaEbT6Em9cQeUoLH7PoAoyf5kepG/wTx/z4BKc4ZXeRjmQvRlSWVDtai/g816bdLis3a7MbV+CiJcdci/HL4pAhICbqngqIpGlDchKasgHQM6B8T7jHfQ2uGuke5Hdd5pw01eyLBDQJeAoUt0L3gzzlwbJopdLTbaF7zBNq9yrR0RCACsA1E7eln5Ess3WiF1ANp06cxX6jF57dem910hQ3jAPvzwWaLOg1v5qGmmhsK4ovo/lS+A0pZUXtvHL8CAxzvvxbTI3WMWOqpBL2V2p5XhgQ4QCKcr6RZ6cQDd4pNWQ==</DQ><InverseQ>EEz9SftuZ3HWL6XsHUFQQPhHKg3fj49yh282fdAAT3MwPkDO/BOYMB/AmRW2tzijY0US7Pn0j/l/jR1OEe+21u7jm7tHmnnUYkjqDEtnNQ4KUj1bG+cnXTfO5c7Z2wLC7nt9yL20OfOO6PDNjmJ2dhfVBaRAwm1LG5K68VSNCYCm49lw8WuV2gZXPcpd4UDMtgr0w+LppUZkX89/6K/J6CdoRwEo6vAu7QR1vOsJDSoSK55q8dibq3Qiplhx2PzHLloZF2A/iPG29+RBSplIaRTJ0Ljz42897wQnNLmjokGLgvO2J8sQLiyybRgApzK/mcUKnUbYlz0u30Zp8pJzdg==</InverseQ><D>sqQvXu8CHIY7o/+BUUs5QRJyM7DQyxOFFU+cIy2npC4/uItChrZUvGbR22mYSmohUcMZcPdsIMxNXyIlEMX3gQF2g0rkqHR/OvxBCOvOzWCUatdrecBrQVLLqVEHnId+EMZ3I1S9nn3f6Ls9oHKFa4uGBnLEmvGXLOF4rK/INZy5hylwQEzLJ1ozP5cbGujNe6nm83LOnSQ76eiijmuD+oy8UB+c0BHskyN/IquHxBQWsFYEcQ6qKGOHpFzrz9rNMPfAzNTYbZ/iuJ/cY9sIgoSlZ5Xrww+/DvtKKu58MTGsuxVUTeI3lhx+DaIBgTpHI0QdqUX9Sfwkur+RLkAu1I3oEDLfNg7rqQ3eX3XlkxnYzOx6/XQguIMSh6WClfaviqu6xtnZt2wU46YXW5PqSLpT7FoD8QKPlAXQq64s5OYOxpHFlOqjpp8CZj9OJwSlWq/9+2qHHBuMh5vECkAl2gbKglCSgYzzjDtaGhat8Suxj5LB6Hy8DHWjpcfMbJ7zgf0tKUzWjDM+CNvjlTyrzhEzRXLX3gUYBSngIxE80CHzax+qwLUA1Q+1gjkZpOFoNYJ6+7KMeNElb7obi4UzMWNUd2C5uYGNu0pKQUUZ7QzhbHkiDkKQ+9ZoXVQdBHgPeie4yeqRxt/rewZFEOacbeCBioFiL+eXbhEKI2Llq/E=</D></RSAKeyValue>";


        #region RsaEncrypt
        /// <summary>
        /// RsaEncrypt
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string RsaEncrypt(string content)
        {
            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                string strpublicKey = _RegisteredDePublicKey;
                var inputBytes = Encoding.UTF8.GetBytes(content);
                rsaProvider.FromXmlString(strpublicKey);
                int bufferSize = (rsaProvider.KeySize / 8) - 11;
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes),
                     outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        int readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var encryptedBytes = rsaProvider.Encrypt(temp, false);
                        //rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
                        outputStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }
                    return Convert.ToBase64String(outputStream.ToArray());
                }
            }
        }
        #endregion

        #region RsaDecrypt
        /// <summary>
        /// RsaDecrypt
        /// </summary>
        /// <param name="encryptedInput"></param>
        /// <returns></returns>
        private static string RsaDecrypt(string encryptedInput)
        {
            if (string.IsNullOrEmpty(encryptedInput))
            {
                return string.Empty;
            }

            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                var inputBytes = Convert.FromBase64String(encryptedInput);
                rsaProvider.FromXmlString(_RegDePrivateKey); // (_RegisteredDePrivateKey);
                int bufferSize = rsaProvider.KeySize / 8;
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes),
                     outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        int readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var rawBytes = rsaProvider.Decrypt(temp, false);
                        outputStream.Write(rawBytes, 0, rawBytes.Length);
                    }
                    return Encoding.UTF8.GetString(outputStream.ToArray());
                }
            }
        }
        #endregion

        #region RsaDecrypt_ 
        /// <summary>
        /// RsaDecrypt
        /// </summary>
        /// <param name="encryptedInput"></param>
        /// <returns></returns>
        private static string RsaDecrypt_(string encryptedInput)
        {
            if (string.IsNullOrEmpty(encryptedInput))
            {
                return string.Empty;
            }

            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                var inputBytes = Convert.FromBase64String(encryptedInput);
                rsaProvider.FromXmlString(_RegDePrivateKey);
                int bufferSize = rsaProvider.KeySize / 8;
                var buffer = new byte[bufferSize];
                using (MemoryStream inputStream = new MemoryStream(inputBytes),
                     outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        int readSize = inputStream.Read(buffer, 0, bufferSize);
                        if (readSize <= 0)
                        {
                            break;
                        }

                        var temp = new byte[readSize];
                        Array.Copy(buffer, 0, temp, 0, readSize);
                        var rawBytes = rsaProvider.Decrypt(temp, false);
                        outputStream.Write(rawBytes, 0, rawBytes.Length);
                    }
                    return Encoding.UTF8.GetString(outputStream.ToArray());
                }
            }
        }
        #endregion

        #region sign 签名
        /// <summary>  
        /// 签名  
        /// </summary>  
        /// <param name="content">待签名字符串</param>  
        /// <param name="privateKey">私钥</param>  
        /// <param name="input_charset">编码格式</param>  
        /// <returns>签名后字符串</returns>  
        public static string Sign(string content)
        {
            byte[] Data = Encoding.GetEncoding("UTF-8").GetBytes(content);
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.FromXmlString(_RegisteredSignPrivateKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            byte[] signData = rsaProvider.SignData(Data, sh);
            return Convert.ToBase64String(signData);
        }
        #endregion


        #region Verify 验签
        /// <summary>  
        /// 验签  
        /// </summary>  
        /// <param name="content">待验签字符串</param>  
        /// <param name="signedString">签名</param>  
        /// <param name="publicKey">公钥</param>  
        /// <param name="input_charset">编码格式</param>  
        /// <returns>true(通过)，false(不通过)</returns>  
        private static bool Verify(string content, string signedString)
        {
            bool result = false;
            byte[] Data = Encoding.GetEncoding("UTF-8").GetBytes(content);
            byte[] data = Convert.FromBase64String(signedString);
           // RSAParameters paraPub = ConvertFromPublicKey(_RegSignPublicKey);  
            RSACryptoServiceProvider rsaPub = new RSACryptoServiceProvider();
            //rsaPub.ImportParameters(paraPub);
            rsaPub.FromXmlString(_RegSignPublicKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            result = rsaPub.VerifyData(Data, sh, data);
            return result;
        }
        #endregion

        

        #endregion
        #endregion

        #region 获取后几位数 public string GetLastStr(string str,int num)
        /// <summary>
        /// 获取后几位数
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="num">返回的具体位数</param>
        /// <returns>返回结果的字符串</returns>
        public string GetLastStr(string str, int num)
        {
            int count = 0;
            if (str.Length > num)
            {
                count = str.Length - num;
                str = str.Substring(count, num);
            }
            return str;
        }
        #endregion

        #region RemoveLastChar
        /// <summary>
        /// 移除字符串末尾指定字符
        /// </summary>
        /// <param name="str">需要移除的字符串</param>
        /// <param name="value">指定字符</param>
        /// <returns>移除后的字符串</returns>
        public  string RemoveLastChar(string str, string value)
        {
            int _finded = str.LastIndexOf(value);
            if (_finded != -1)
            {
                return str.Substring(0, _finded);
            }
            return str;
        }
        #endregion

        #region
        private string GetModuleCode()
        {
            string  moduleCode = "";
            if (this.cbxF11.Checked)
                moduleCode = "C11";
            if (this.cbxF12.Checked)
                moduleCode = "C12";
            if (this.cbxF13.Checked)
                moduleCode = "C13";
            if (this.cbxF14.Checked)
                moduleCode = "C14";
            if (this.cbxF15.Checked)
                moduleCode = "C15";
           
            return moduleCode;
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


        /// <summary>
        /// 过滤不可见字符
        /// </summary>
        /// <param name="sourceString">原始字符</param>
        /// <returns>删除后结果</returns>

        public string DeleteControlChar(string sourceString)
        {
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < sourceString.Length; i++)
            {
                int Unicode = sourceString[i];
                if (Unicode > 31 && Unicode != 127)
                {
                    sBuilder.Append(sourceString[i].ToString());
                }
            }
            return sBuilder.ToString();
        }

        /*
        public static string StortJson(string json)
        {
            var jo = JObject.Parse(json);
            var target = KeySort(jo);//排序
            //var s = string.Join("", GetValue(jo));
            return JsonConvert.SerializeObject(target);
            //return s;
        }*/
        #endregion


    }
}



/*//封装XML获取接口调用结果
XmlDocument requestDoc = new XmlDocument();
string xmldoc = HaDrugService.franchServiceHaDrug(username, password, sn, "{}", "HY_ZYS4DATA_007");
requestDoc.LoadXml(xmldoc);
XmlDocument resultDoc = HaDrugService.httpPostForService("UTF-8", url, requestDoc);

//实例化XML命名空间管理器
XmlNamespaceManager nsMgr = new XmlNamespaceManager(resultDoc.NameTable);
nsMgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
nsMgr.AddNamespace("ns2", "http://www.pts.com.cn/ptsWebservice/IFranchisessWebService.asmx");

//获取接口调用结果JSON字符串
XmlNode xn = resultDoc.SelectSingleNode("soap:Envelope", nsMgr).SelectSingleNode("soap:Body", nsMgr);
try
{
    string innerJson = HaDrugService.franchiseData(url, username, password, sn, "{}", "HY_ZYS4DATA_007");
    JavaScriptSerializer js = new JavaScriptSerializer();
    JsonParserCorp.DataResult_007 list_007 = js.Deserialize<JsonParserCorp.DataResult_007>(innerJson);
    string resultStauts_007 = list_007.resultStauts.ToString().Trim();
    if (resultStauts_007 == "SYS000001")  //返回成功代码
    {
        string _ServerTime = list_007.resultData.datetime.Substring(0, 10); //list_007.resultData.datetime;
    }
    else
    {
        string _ErrorMsg = list_007.resultDesc.ToString().Trim();
        MessageBox.Show(" 获取服务器当前时间失败，" + _ErrorMsg, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
    }
}
catch (Exception ex)
{
    MessageBox.Show(" 调用WebService接口方法失败，" + ex, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
} */
