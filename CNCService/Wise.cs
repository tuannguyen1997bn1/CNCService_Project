using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNCService
{
    public class Wise1
    {
        public List<chanel> DIVal { get; set; }
        public class chanel
        {
            public byte Ch { get; set; }
            public byte Md { get; set; }
            public int Val { get; set; }
            public byte Stat { get; set; }
            public byte Cnting { get; set; }
            public byte OvLch { get; set; }
        }
    }
}
