using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Registered
{
    class JsonParser
    {
        public struct JsonRegister
        {
            public string I { get; set; }
            public string R { get; set; }
            public string M { get; set; }
            public string N { get; set; }
            public string T { get; set; }
            public int Info { get; set; }

        }


        public struct JsonSign
        {
            public string sing { get; set; }
            public JsonRegister data { get; set; }
        }
    }
}
