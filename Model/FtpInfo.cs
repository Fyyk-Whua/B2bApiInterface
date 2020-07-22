using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class FtpInfo
    {
        public string FtpHostIP { get; set; }
        public int FtpPort { get; set; }
        public string FtpUserName { get; set; }
        public string FtpPassword { get; set; }
        public string FtpRootPath { get; set; }
        public int FtpBuffLength { get; set; }
    }
}
