using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.kompilace
{
    public class UzelProgramu
    {
        public Func<UzelProgramu,object?, object> Akce { get; set; }
        public UzelProgramu? Rodic { get; set; }
        public UzelProgramu[]? Vetve { get; set; }

        public UzelProgramu(Func<UzelProgramu,object?, object> Akce, UzelProgramu? rodic = null, UzelProgramu[]? vetve = null)
        {
            this.Akce = Akce;
            Rodic = rodic;
            Vetve = vetve;
        }

        public object VykonatUzel() 
        {
            return Akce.Invoke(this, Vetve);
        }
    }
}
