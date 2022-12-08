using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyrazy.Logicke
{
    public class LogickyOperator : IClenVyrazu<bool>
    {
        public enum TypyOperatoru
        {
            ROVNO,VETSI,MENSI,VETSIROVNO,MENSIROVNO,OR,AND,NOT
        }

        public LogickyOperator(TypyOperatoru typ)
        {
            Typ = typ;
        }

        public bool Hodnota { get; set; }
        public TypyOperatoru Typ { get; set; }
    }
}
