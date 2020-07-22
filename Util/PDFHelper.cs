using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using O2S.Components.PDFRender4NET;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Util
{
    public class PDFHelper
    {
        public enum Definition
        {
            One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10
        }
        /// <summary>
        /// 将PDF文档转换为图片的方法
        /// </summary>
        /// <param name="pdfInputPath">PDF文件路径</param>
        /// <param name="imageOutputPath">图片输出路径</param>
        /// <param name="imageName">生成图片的名字</param>
        /// <param name="startPageNum">从PDF文档的第几页开始转换</param>
        /// <param name="endPageNum">从PDF文档的第几页开始停止转换</param>
        /// <param name="imageFormat">设置所需图片格式</param>
        /// <param name="definition">设置图片的清晰度，数字越大越清晰</param>
        public static bool ConvertPDFToImage(Log4netUtil.LogAppendToForms logAppendToForms, string pdfInputPath, string imageOutputPath,
            string imageName, int startPageNum, int endPageNum, ImageFormat imageFormat, Definition definition, bool isDebug)
        {
            return convertPDFToImage(logAppendToForms,pdfInputPath, imageOutputPath, imageName, startPageNum, endPageNum, imageFormat, definition, isDebug);
        }
        private static bool convertPDFToImage(Log4netUtil.LogAppendToForms logAppendToForms, string pdfInputPath, string imageOutputPath,
            string imageName, int startPageNum, int endPageNum, ImageFormat imageFormat, Definition definition, bool isDebug)
        {

            PDFFile pdfFile = PDFFile.Open(pdfInputPath);
            try
            {
                if (!Directory.Exists(imageOutputPath))
                    Directory.CreateDirectory(imageOutputPath);
                if (startPageNum <= 0)
                    startPageNum = 1;
                if (endPageNum > pdfFile.PageCount)
                    endPageNum = pdfFile.PageCount;
                if (startPageNum > endPageNum)
                {
                    int tempPageNum = startPageNum;
                    startPageNum = endPageNum;
                    endPageNum = startPageNum;
                }
                for (int i = startPageNum; i <= endPageNum; i++)
                {
                    Bitmap pageImage = pdfFile.GetPageImage(i - 1, 56 * (int)definition);
                    string extension = imageFormat.ToString();
                    if (string.Equals(extension.ToLower(), "jpeg"))
                        extension = "jpg";
                    pageImage.Save(imageOutputPath + imageName + "." + extension, imageFormat);
                    //pageImage.Save(imageOutputPath + imageName + i.ToString() + "." + extension, imageFormat);
                    pageImage.Dispose();
                }
                return true;
            }

            catch (Exception ex)
            {
                string logMessage = string.Format("【随货同行下载任务】 PdfInputPath {0} 转换图片失败！原因,{1}", pdfInputPath, ex.Message);
                Log4netUtil.Log4NetHelper.LogMessage(logAppendToForms, isDebug, logMessage, @"KJJ\DownloadDataBusiness");
                return false;
            }
            finally
            {
                pdfFile.Dispose();
            }
        }
    }
}

