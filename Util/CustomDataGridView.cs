using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Util
{
    public class CustomDataGridView : DataGridView
    {

        private bool isEnterKey;
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                return this.ProcessRightKey(keyData);
            }
            return base.ProcessDialogKey(keyData);
        }
        public new bool ProcessRightKey(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                //第一种情况：只有一行,且当光标移到最后一列时
                if ((base.CurrentCell.ColumnIndex == (base.ColumnCount - 1)) && (base.RowCount == 1))
                {
                    base.CurrentCell = base.Rows[base.RowCount - 1].Cells[0];
                    return true;
                }
                //第二种情况：有多行，且当光标移到最后一列时,移到下一行第一个单元

                if ((base.CurrentCell.ColumnIndex == (base.ColumnCount - 1)) && (base.CurrentCell.RowIndex < (base.RowCount - 1)))
                {
                    base.CurrentCell = base.Rows[base.CurrentCell.RowIndex + 1].Cells[0];
                    return true;
                }
                isEnterKey = true;
            }
            return base.ProcessRightKey(keyData);
        }
        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                isEnterKey = true;
                return this.ProcessRightKey(e.KeyData);
            }
            return base.ProcessDataGridViewKey(e);
        }
        protected override void OnCellEnter(DataGridViewCellEventArgs e)
        {
            if (isEnterKey && (!CurrentCell.Visible || CurrentCell.ReadOnly))
            {
                SendKeys.Send("{Enter}");
            }
            else
            {
                isEnterKey = false;
                base.OnCellEnter(e);
            }
        }
        /*
        protected override bool ProcessDialogKey(Keys keyData)
        {
            Keys key = (keyData & Keys.KeyCode);
            if (key == Keys.Enter)
            {
                return this.ProcessRightKey(keyData);
            }
            return base.ProcessDialogKey(keyData);
        }

        public new bool ProcessRightKey(Keys keyData)
        {
            Keys key = (keyData & Keys.KeyCode);
            if (key == Keys.Enter)
            {
                //第一种情况：只有一行,且当光标移到最后一列时
                if ((base.CurrentCell.ColumnIndex == (base.ColumnCount - 1)) && (base.RowCount == 1))
                {
                    base.CurrentCell = base.Rows[base.RowCount - 1].Cells[0];
                    return true;
                }
                //第二种情况：有多行，且当光标移到最后一列时,移到下一行第一个单元
                if ((base.CurrentCell.ColumnIndex == (base.ColumnCount - 1)) && (base.CurrentCell.RowIndex < (base.RowCount - 1)))
                {
                    base.CurrentCell = base.Rows[base.CurrentCell.RowIndex + 1].Cells[0];
                    return true;
                }

                return base.ProcessRightKey(keyData);
            }
            return base.ProcessRightKey(keyData);
        }

 

        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                return this.ProcessRightKey(e.KeyData);
            }
            //if (e.KeyCode == Keys.F4)
            //{
            //    return this.ProcessRightKey(e.KeyData);
            //}
            return base.ProcessDataGridViewKey(e);
        }
        */
    }
}


/*
       /// <summary>
       /// 设置编辑状态下按回车键，跳到指定单元格.
       /// </summary>
       protected override bool ProcessCmdKey(System.Windows.Forms.DataGridView dgv,ref Message msg, Keys keyData)
       {

           if (keyData == Keys.Enter)    //监听回车事件 
           {
               if (dgv.IsCurrentCellInEditMode)   //如果当前单元格处于编辑模式 
               {

                   if (dgv.CurrentCell.ColumnIndex != 12)
                   {
                       SendKeys.Send("{Up}");
                       SendKeys.Send("{Tab}");
                       SendKeys.Send("{Tab}");
                   }
                   else
                   {
                       SendKeys.Send("{LEFT}"); SendKeys.Send("{LEFT}"); SendKeys.Send("{LEFT}"); SendKeys.Send("{LEFT}"); SendKeys.Send("{LEFT}"); SendKeys.Send("{LEFT}");
                       SendKeys.Send("{LEFT}"); SendKeys.Send("{LEFT}"); SendKeys.Send("{LEFT}"); SendKeys.Send("{LEFT}");
                   }
               }
               else
               {
                   SendKeys.Send("{Up}");
                   SendKeys.Send("{Tab}");
                   SendKeys.Send("{Tab}");
                   SendKeys.Send("{Tab}");
               }


           }

           //继续原来base.ProcessCmdKey中的处理 
           return base.ProcessCmdKey(ref msg, keyData);
       }
       */
