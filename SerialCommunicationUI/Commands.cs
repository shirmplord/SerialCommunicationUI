using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialCommunicationUI
{
    /*
     * Class containing all the commands and the corresponding command codes
     * The table does not contain all available command
     */
    class Commands
    {
        IDictionary<string, byte> codes = new Dictionary<string, byte>
        {
            {"alive"        ,   100},
            {"reset"        ,   101},
            {"status"       ,   102},
            {"version"      ,   103},
            {"inputs"       ,   105},
            {"param"        ,   106},
            {"times"        ,   107},
            {"aux_param"    ,   108},
            {"output_ctrl"  ,   140},
            {"led_ctrl"     ,   141},
            {"simulate"     ,   200 }
        };
        public Commands() { }
        public byte GetCode(string key)
        {
            return codes[key];
        }
    }
}
