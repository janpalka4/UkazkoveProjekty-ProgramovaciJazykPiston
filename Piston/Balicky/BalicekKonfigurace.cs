using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Balicky
{
    [DisplayName("PistonPackage")]
    public class BalicekKonfigurace
    {
        [JsonProperty("Name")]
        public string? Nazev { get; set; }
        [JsonProperty("AssemblyName")]
        public string? NazevSestaveni { get; set; }
    }
}
