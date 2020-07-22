using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Util
{
    ///*************************************************************************/
    ///*
    ///* 文 件 名: PasswordTextBox.cs   
    ///* 命名空间: Util.FrameUtil
    ///* 功    能: textBox 输入密码 时间间隔显示星号
    ///* 内    容: 
    ///* 原创作者: lau 
    ///* 生成日期: 2018.08.08
    ///* 版 本 号: V1.0.0.0
    ///* 修改日期:
    ///* 版权说明:  Copyright 2018-2027 武汉飞宇益克科技有限公司
    ///*
    ///**************************************************************************/
    
    public class PasswordTextBox : TextBox
    {
        private Timer _Timer;

        public PasswordTextBox()
        {
            this.KeyUp += TextBoxKeyUp;

            _Timer = new Timer();
            _Timer.Tick += Tick;
            _Timer.Interval = 200;
        }

        private void TextBoxKeyUp(object sender, EventArgs e)
        {
            _Timer.Start();
        }

        public string text = "";

        private void Tick(object sender, EventArgs e)
        {
            _Timer.Stop();
            if (text == "")
            {
                text = this.Text;
            }
            else
            {
                if (Text.Length > text.Length)
                {
                    string a = this.Text;
                    text = text + a.Substring(text.Length);
                }
                if (Text.Length < text.Length)
                {
                    text = text.Substring(0, Text.Length);
                }
            }
            Text = new String('*', Text.Length);
            this.SelectionStart = Text.Length;

        }

    }
}

