using System;
using System.Text;

namespace Registered
{
    /// <summary>
    /// Summary description for CBase32.
    /// </summary>
    public class CBase32
    {
        #region 声明
        private const String DefaultBase32Map = "HDNGQ4LAU5FYKB6CE3IJ8MPRSTV7WX9Z";          
        private const Int32 Base32MapLength = 32;

        private static Char[] m_acBase32Map = null;
        #endregion

        #region CBase32 构造函数
        static CBase32()
        {
            m_acBase32Map = DefaultBase32Map.ToCharArray();
        }
        public CBase32()
        {
        }
        #endregion

        #region Base32Map
        public static String Base32Map
        {
            get
            {
                return m_acBase32Map.ToString();
            }
            set
            {
                if (value != null && value.Length >= Base32MapLength)
                {
                    m_acBase32Map = value.ToCharArray();
                }
                else
                {
                    m_acBase32Map = DefaultBase32Map.ToCharArray();
                }
            }
        }

        #endregion

        #region GetCharacter
        public static Char GetCharacter(Int32 dwIndex)
        {
            Char cMappingData = '0'; // '/0';

            if (m_acBase32Map != null && dwIndex >= 0 && dwIndex < m_acBase32Map.Length)
            {
                cMappingData = m_acBase32Map[dwIndex];
            }
            return cMappingData;
        }
        #endregion

        #region GetCharIndex
        public static Int32 GetCharIndex(Char cData)
        {
            Int32 dwIndex = -1, dwLoop = 0;

            if (m_acBase32Map != null)
            {
                for (dwLoop = 0; dwLoop < m_acBase32Map.Length; dwLoop++)
                {
                    if (m_acBase32Map[dwLoop] == cData)
                    {
                        dwIndex = dwLoop;
                        break;
                    }
                }
            }
            return dwIndex;
        }
        #endregion

        #region Encode加密
        public static String Encode(Byte[] abData)
        {
            Int32 dwLoop = 0, dwCharIndex = 0, dwCharCount;
            Char[] acPart = null;
            StringBuilder sbOutput = null;

            if (abData == null || m_acBase32Map == null || m_acBase32Map.Length < Base32MapLength)
                return null;

            try
            {
                dwCharCount = (Int32)(abData.Length / 5f * 8f) + 1;
                sbOutput = new StringBuilder(dwCharCount);
                acPart = new Char[8];

            }
            catch (Exception e)
            {
                throw new ArgumentException("Initialize buffer failed! " + e.Message);
            }
            if (acPart == null || sbOutput == null)
                return null;

            dwCharCount = 0;
            for (dwLoop = 0; dwLoop < abData.Length; dwLoop += 5)
            {
               
                switch (abData.Length - dwLoop)
                {
                    case 1:
                        dwCharIndex = abData[dwLoop] >> 3;    // AAAAA
                        acPart[0] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = (abData[dwLoop] & 0x7) << 2;  // BBB00
                        acPart[1] = m_acBase32Map[dwCharIndex];
                        dwCharCount = 2;
                        break;

                    case 2:
                        dwCharIndex = abData[dwLoop] >> 3;    // AAAAA
                        acPart[0] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = ((abData[dwLoop] & 0x7) << 2) + (abData[dwLoop + 1] >> 6); // BBBBB
                        acPart[1] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = (abData[dwLoop + 1] & 0x3F) >> 1; // CCCCC
                        acPart[2] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = abData[dwLoop + 1] & 0x1;   // D0000
                        acPart[3] = m_acBase32Map[dwCharIndex];
                        dwCharCount = 4;
                        break;

                    case 3:
                        dwCharIndex = abData[dwLoop] >> 3;    // AAAAA
                        acPart[0] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = ((abData[dwLoop] & 0x7) << 2) + (abData[dwLoop + 1] >> 6); // BBBBB
                        acPart[1] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = (abData[dwLoop + 1] & 0x3F) >> 1; // CCCCC
                        acPart[2] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = ((abData[dwLoop + 1] & 0x1) << 4) + (abData[dwLoop + 2] >> 4);// DDDDD
                        acPart[3] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = (abData[dwLoop + 2] & 0xF) << 1; // EEEE0
                        acPart[4] = m_acBase32Map[dwCharIndex];
                        dwCharCount = 5;
                        break;

                    case 4:
                        dwCharIndex = abData[dwLoop] >> 3;    // AAAAA
                        acPart[0] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = ((abData[dwLoop] & 0x7) << 2) + (abData[dwLoop + 1] >> 6); // BBBBB
                        acPart[1] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = (abData[dwLoop + 1] & 0x3F) >> 1; // CCCCC
                        acPart[2] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = ((abData[dwLoop + 1] & 0x1) << 4) + (abData[dwLoop + 2] >> 4);// DDDDD
                        acPart[3] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = ((abData[dwLoop + 2] & 0xF) << 1) + (abData[dwLoop + 3] >> 7);// EEEEE
                        acPart[4] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = (abData[dwLoop + 3] & 0x7F) >> 2; // FFFFF
                        acPart[5] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = (abData[dwLoop + 3] & 0x3) << 3; // GG000
                        acPart[6] = m_acBase32Map[dwCharIndex];
                        dwCharCount = 7;
                        break;

                    default:  // >= 5
                        dwCharIndex = abData[dwLoop] >> 3;    // AAAAA
                        acPart[0] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = ((abData[dwLoop] & 0x7) << 2) + (abData[dwLoop + 1] >> 6); // BBBBB
                        acPart[1] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = (abData[dwLoop + 1] & 0x3F) >> 1; // CCCCC
                        acPart[2] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = ((abData[dwLoop + 1] & 0x1) << 4) + (abData[dwLoop + 2] >> 4);// DDDDD
                        acPart[3] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = ((abData[dwLoop + 2] & 0xF) << 1) + (abData[dwLoop + 3] >> 7);// EEEEE
                        acPart[4] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = (abData[dwLoop + 3] & 0x7F) >> 2; // FFFFF
                        acPart[5] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = ((abData[dwLoop + 3] & 0x3) << 3) + (abData[dwLoop + 4] >> 5);// GGGGG
                        acPart[6] = m_acBase32Map[dwCharIndex];
                        dwCharIndex = abData[dwLoop + 4] & 0x1F;  // HHHHH
                        acPart[7] = m_acBase32Map[dwCharIndex];
                        dwCharCount = 8;
                        break;
                }

                sbOutput.Append(acPart, 0, dwCharCount);
            }

            return sbOutput.ToString();
        }
        #endregion

        #region Decode解密
        public static Byte[] Decode(String sData)
        {
            Int32 dwLoop = 0, dwLength = 0;
            Int32[] dwCharIndex = null;
            Byte[] abOutput = null;
            Char[] acInput = null;

            if (sData == null || sData == String.Empty)
                return null;

            acInput = sData.ToCharArray();
            if (acInput == null)
                return null;

            try
            {
                dwLength = (Int32)(acInput.Length / 8f * 5f) + 1;
                abOutput = new Byte[dwLength];
                dwCharIndex = new Int32[8];
            }
            catch (Exception e)
            {
                throw new ArgumentException("Initialize buffer failed! " + e.Message);
            }
            if (acInput == null)
                return null;

            dwLength = 0;
            for (dwLoop = 0; dwLoop < acInput.Length; dwLoop += 8)
            {
                Array.Clear(dwCharIndex, 0, dwCharIndex.Length);
                switch (acInput.Length - dwLoop)
                {
                    case 1:
                        dwCharIndex[0] = GetCharIndex(acInput[dwLoop]);

                        abOutput[dwLength] = (Byte)(dwCharIndex[0] << 3);
                        break;

                    case 2:
                        dwCharIndex[0] = GetCharIndex(acInput[dwLoop]);
                        dwCharIndex[1] = GetCharIndex(acInput[dwLoop + 1]);

                        abOutput[dwLength] = (Byte)((dwCharIndex[0] << 3) + (dwCharIndex[1] >> 2));
                        abOutput[dwLength + 1] = (Byte)((dwCharIndex[1] & 0x3) << 6);
                        break;

                    case 3:
                        dwCharIndex[0] = GetCharIndex(acInput[dwLoop]);
                        dwCharIndex[1] = GetCharIndex(acInput[dwLoop + 1]);
                        dwCharIndex[2] = GetCharIndex(acInput[dwLoop + 2]);

                        abOutput[dwLength] = (Byte)(dwCharIndex[0] << 3 + dwCharIndex[1] >> 2);
                        abOutput[dwLength + 1] = (Byte)((dwCharIndex[1] & 0x3) << 6 + dwCharIndex[2] << 1);
                        break;


                    case 4:
                        dwCharIndex[0] = GetCharIndex(acInput[dwLoop]);
                        dwCharIndex[1] = GetCharIndex(acInput[dwLoop + 1]);
                        dwCharIndex[2] = GetCharIndex(acInput[dwLoop + 2]);
                        dwCharIndex[3] = GetCharIndex(acInput[dwLoop + 3]);

                        abOutput[dwLength] = (Byte)((dwCharIndex[0] << 3) + (dwCharIndex[1] >> 2));
                        abOutput[dwLength + 1] = (Byte)(((dwCharIndex[1] & 0x3) << 6) + (dwCharIndex[2] << 1) + (dwCharIndex[3] >> 4));
                        abOutput[dwLength + 2] = (Byte)((dwCharIndex[3] & 0xF) << 4);
                        break;

                    case 5:
                        dwCharIndex[0] = GetCharIndex(acInput[dwLoop]);
                        dwCharIndex[1] = GetCharIndex(acInput[dwLoop + 1]);
                        dwCharIndex[2] = GetCharIndex(acInput[dwLoop + 2]);
                        dwCharIndex[3] = GetCharIndex(acInput[dwLoop + 3]);
                        dwCharIndex[4] = GetCharIndex(acInput[dwLoop + 4]);

                        abOutput[dwLength] = (Byte)((dwCharIndex[0] << 3) + (dwCharIndex[1] >> 2));
                        abOutput[dwLength + 1] = (Byte)(((dwCharIndex[1] & 0x3) << 6) + (dwCharIndex[2] << 1) + (dwCharIndex[3] >> 4));
                        abOutput[dwLength + 2] = (Byte)(((dwCharIndex[3] & 0xF) << 4) + (dwCharIndex[4] >> 1));
                        abOutput[dwLength + 3] = (Byte)(((dwCharIndex[4] & 0x1) << 7));
                        break;
                    case 6:
                        dwCharIndex[0] = GetCharIndex(acInput[dwLoop]);
                        dwCharIndex[1] = GetCharIndex(acInput[dwLoop + 1]);
                        dwCharIndex[2] = GetCharIndex(acInput[dwLoop + 2]);
                        dwCharIndex[3] = GetCharIndex(acInput[dwLoop + 3]);
                        dwCharIndex[4] = GetCharIndex(acInput[dwLoop + 4]);
                        dwCharIndex[5] = GetCharIndex(acInput[dwLoop + 5]);

                        abOutput[dwLength] = (Byte)(dwCharIndex[0] << 3 + dwCharIndex[1] >> 2);
                        abOutput[dwLength + 1] = (Byte)((dwCharIndex[1] & 0x3) << 6 + dwCharIndex[2] << 1 + dwCharIndex[3] >> 4);
                        abOutput[dwLength + 2] = (Byte)((dwCharIndex[3] & 0xF) << 4 + dwCharIndex[4] >> 1);
                        abOutput[dwLength + 3] = (Byte)((dwCharIndex[4] & 0x1) << 7 + dwCharIndex[5] << 2);
                        break;


                    case 7:
                        dwCharIndex[0] = GetCharIndex(acInput[dwLoop]);
                        dwCharIndex[1] = GetCharIndex(acInput[dwLoop + 1]);
                        dwCharIndex[2] = GetCharIndex(acInput[dwLoop + 2]);
                        dwCharIndex[3] = GetCharIndex(acInput[dwLoop + 3]);
                        dwCharIndex[4] = GetCharIndex(acInput[dwLoop + 4]);
                        dwCharIndex[5] = GetCharIndex(acInput[dwLoop + 5]);
                        dwCharIndex[6] = GetCharIndex(acInput[dwLoop + 6]);

                        abOutput[dwLength] = (Byte)((dwCharIndex[0] << 3) + (dwCharIndex[1] >> 2));
                        abOutput[dwLength + 1] = (Byte)(((dwCharIndex[1] & 0x3) << 6) + (dwCharIndex[2] << 1) + (dwCharIndex[3] >> 4));
                        abOutput[dwLength + 2] = (Byte)(((dwCharIndex[3] & 0xF) << 4) + (dwCharIndex[4] >> 1));
                        abOutput[dwLength + 3] = (Byte)(((dwCharIndex[4] & 0x1) << 7) + (dwCharIndex[5] << 2) + (dwCharIndex[6] >> 3));
                        abOutput[dwLength + 4] = (Byte)((dwCharIndex[6] & 0x7) << 5);
                        break;

                    default:
                        dwCharIndex[0] = GetCharIndex(acInput[dwLoop]);
                        dwCharIndex[1] = GetCharIndex(acInput[dwLoop + 1]);
                        dwCharIndex[2] = GetCharIndex(acInput[dwLoop + 2]);
                        dwCharIndex[3] = GetCharIndex(acInput[dwLoop + 3]);
                        dwCharIndex[4] = GetCharIndex(acInput[dwLoop + 4]);
                        dwCharIndex[5] = GetCharIndex(acInput[dwLoop + 5]);
                        dwCharIndex[6] = GetCharIndex(acInput[dwLoop + 6]);
                        dwCharIndex[7] = GetCharIndex(acInput[dwLoop + 7]);

                        abOutput[dwLength] = (Byte)((dwCharIndex[0] << 3) + (dwCharIndex[1] >> 2));
                        abOutput[dwLength + 1] = (Byte)(((dwCharIndex[1] & 0x3) << 6) + (dwCharIndex[2] << 1) + (dwCharIndex[3] >> 4));
                        abOutput[dwLength + 2] = (Byte)(((dwCharIndex[3] & 0xF) << 4) + (dwCharIndex[4] >> 1));
                        abOutput[dwLength + 3] = (Byte)(((dwCharIndex[4] & 0x1) << 7) + (dwCharIndex[5] << 2) + (dwCharIndex[6] >> 3));
                        abOutput[dwLength + 4] = (Byte)(((dwCharIndex[6] & 0x7) << 5) + dwCharIndex[7]);
                        break;
                }
                dwLength += 5;
            }

            return abOutput;
        }
        #endregion

    }

}






        