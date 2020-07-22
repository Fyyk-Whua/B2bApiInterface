//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using log4net.Appender;
using log4net.Filter;
using log4net.Util;
using log4net.Layout;
using log4net.Core;

 
namespace Log4netUtil
{

    public class ReadParamAppender : log4net.Appender.AppenderSkeleton
    {

        public string File { get; set; }
        public int MaxSizeRollBackups { get; set; }
        public bool AppendToFile { get; set; }
        public string MaximumFileSize { get; set; }
        public string LayoutPattern { get; set; }
        public string DatePattern { get; set; }
        public string Level { get; set; }

        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            StringWriter writer = new StringWriter();
            this.Layout.Format(writer, loggingEvent);
            // 已经得到了按照自己设置的格式的日志消息内容了，就是writer.toString()。然后你想把这句话显示在哪都可以了。。我是测试就直接控制台了。
            Console.Write(writer.ToString());
        }
        
        /*
        private string _file;
        public string File
        {
            get { return this._file; }
            set { _file = value; }
        }
 
        private int _maxSizeRollBackups;
        public int MaxSizeRollBackups
        {
            get { return this._maxSizeRollBackups; }
            set { _maxSizeRollBackups = value; }
        }
 
        private bool _appendToFile = true;
        public bool AppendToFile
        {
            get { return this._appendToFile; }
            set { _appendToFile = value; }
        }
 
        private string _maximumFileSize;
        public string MaximumFileSize
        {
            get { return this._maximumFileSize; }
            set { _maximumFileSize = value; }
        }
 
        private string _layoutPattern;
        public string LayoutPattern
        {
            get { return this._layoutPattern; }
            set { _layoutPattern = value; }
        }
 
        private string _datePattern;
        public string DatePattern
        {
            get { return this._datePattern; }
            set { _datePattern = value; }
        }
 
        private string _level;
        public string Level
        {
            get { return this._level; }
            set { _level = value; }
        }
        */


        
    }
}


