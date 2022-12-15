using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyrazy.Stringove
{
    public class RetezcovyVyraz : IClenVyrazu<string>
    {
        public string Hodnota { get =>VyhodnotitVyraz(); set => throw new NotImplementedException(); }
        public List<IClenVyrazu<string>> Cleni { get; set; }

        public RetezcovyVyraz(List<IClenVyrazu<string>> cleni) {
            Cleni = cleni;
        }


        private string VyhodnotitVyraz()
        {
            if (Cleni.Count == 0)
                return string.Empty;

            if (Cleni.Count == 1)
                return Cleni[0].Hodnota;

            for(int i = 0; i < Cleni.Count; i++)
            {
                if (Cleni[i] is not RetezcovyOperator)
                    continue;

                IClenVyrazu<string> levy = Cleni[i-1];
                IClenVyrazu<string> pravy = Cleni[i + 1];

                Cleni[i] = new HodnotaRetezce(ProvedOperaci(levy, pravy, (RetezcovyOperator)Cleni[i]));

                Cleni.RemoveAt(i + 1);
                Cleni.RemoveAt(i - 1);

                break;
            }

            return VyhodnotitVyraz();
        }

        private string ProvedOperaci(IClenVyrazu<string> levy, IClenVyrazu<string> pravy, RetezcovyOperator retezcovyOperator)
        {
            switch (retezcovyOperator.TypOperatoru)
            {
                case RetezcovyOperator.TypyOperatoru.APPEND:
                    return levy.Hodnota + pravy.Hodnota;

                default:
                    return string.Empty;
            }
        }
    }
}
