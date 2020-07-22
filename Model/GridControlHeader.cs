using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class GridControlHeader
    {
        public string FdName { get; set; }
        public string FdDesc { get; set; }
        // public string FdType { get; set; }
        // public int FdSize { get; set; }
        // public int FdDec { get; set; }
        public int Sort { get; set; }
        public string IsCheckBox { get; set; }
        public string IsVisible { get; set; }
        public string IsReadOnly { get; set; }
    }
}
