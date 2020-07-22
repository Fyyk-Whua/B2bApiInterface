using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HebfdeaInterface.Common
{
    public delegate void ToolStripClickDelegate(object sender, EventArgs e);

    public class FormsToolHelper
    {
        #region CreateFormsTool
        /// <summary>
        /// CreateFormsTool
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="tsbText"></param>
        /// <param name="tsbImage"></param>
        /// <param name="tsbName"></param>
        /// <param name="tsbClickEvent"></param>
        public static void CreateFormsTool(System.Windows.Forms.ToolStrip ts, string tsbText, System.Drawing.Image tsbImage, string tsbName, ToolStripClickDelegate tsbClickEvent)
        {
            createFormsTool(ts, tsbText, tsbImage, tsbName, tsbClickEvent);
        }

        private static void createFormsTool(System.Windows.Forms.ToolStrip ts, string tsbText, System.Drawing.Image tsbImage, string tsbName, ToolStripClickDelegate tsbClickEvent)
        {
            System.Windows.Forms.ToolStripButton item = new System.Windows.Forms.ToolStripButton();
            item.Text = tsbText;
            item.Size = new System.Drawing.Size(83, 35);
            item.Image = tsbImage; // global::FYYK.UI.Forms.Properties.Resources.保存_32;
            item.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            item.ImageTransparentColor = System.Drawing.Color.Magenta;
            item.Name = tsbName;// "tsbtnSave";
            item.RightToLeft = System.Windows.Forms.RightToLeft.No;
            //item.CheckOnClick = true;
            item.Click += new System.EventHandler(tsbClickEvent);
            ts.Items.Add(item);
            System.Windows.Forms.ToolStripSeparator tss = new System.Windows.Forms.ToolStripSeparator();
            ts.Items.Add(tss);
        }
        #endregion

    }
}
