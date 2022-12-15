using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyrazy.Stringove
{
    public class RetezcovyOperator : IClenVyrazu<string>
    {
        public enum TypyOperatoru
        {
            APPEND = '.'
        }
        public string Hodnota { get; set; }
        public TypyOperatoru TypOperatoru { get; set; }

        public RetezcovyOperator(TypyOperatoru typ) 
        { 
            Hodnota = string.Empty; 
            TypOperatoru = typ;
        }
    }
}
