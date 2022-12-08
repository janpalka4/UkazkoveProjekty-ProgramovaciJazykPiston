using Piston.Vyjimky;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyrazy.Logicke
{
    public class LogickyVyraz : IClenVyrazu<bool>
    {
        public bool Hodnota { get => VyhodnotitVyraz(); set => throw new Exception("Nelze nastavit hodnota výrazu"); }

        private List<IClenVyrazu> Cleni { get; set; }

        public LogickyVyraz(List<IClenVyrazu> cleni)
        {
            Cleni = cleni;
        }

        private bool VyhodnotitVyraz()
        {
            if (Cleni.Count == 1 && Cleni[0] is IClenVyrazu<bool>)
                return ((IClenVyrazu<bool>)Cleni[0]).Hodnota;

            int i = 0;

            foreach(IClenVyrazu clen in Cleni)
            {

                if(clen is LogickyOperator)
                {
                    if(((LogickyOperator)clen).Typ == LogickyOperator.TypyOperatoru.NOT)
                    {
                        IClenVyrazu _pravy = Cleni[i + 1];
                        Cleni[i] = new LogickaHodnota() { Hodnota = ProvedOperaci(new LogickaHodnota(), _pravy, (LogickyOperator)clen) };
                        Cleni.RemoveAt(i + 1);

                        break;
                    }

                    if (i == 0)
                        throw new ChybaOperace("Logická operace nemá levou stranu");

                    IClenVyrazu levy = Cleni[i - 1];
                    IClenVyrazu pravy = Cleni[i + 1];

                    Cleni[i] = new LogickaHodnota() { Hodnota = ProvedOperaci(levy, pravy, (LogickyOperator)clen) };
                    Cleni.RemoveAt(i + 1);
                    Cleni.RemoveAt(i - 1);

                    break;
                }

                i++;
            }

            return VyhodnotitVyraz();
        }

        private bool ProvedOperaci(IClenVyrazu levy,IClenVyrazu pravy,LogickyOperator logickyOperator)
        {
            if (!((levy is IClenVyrazu<bool> && pravy is IClenVyrazu<bool>) || (levy is IClenVyrazu<double> && pravy is IClenVyrazu<double>)))
                throw new ChybaOperace("Operace nelze provést mezi různými typy.");

            switch (logickyOperator.Typ)
            {
                case LogickyOperator.TypyOperatoru.ROVNO:
                    return levy is IClenVyrazu<bool> ? ZiskejHodnotuClena<bool>(levy) == ZiskejHodnotuClena<bool>(pravy) : ZiskejHodnotuClena<double>(levy) == ZiskejHodnotuClena<double>(pravy);
                case LogickyOperator.TypyOperatoru.VETSI:
                    return ZiskejHodnotuClena<double>(levy) > ZiskejHodnotuClena<double>(pravy);
                case LogickyOperator.TypyOperatoru.MENSI:
                    return ZiskejHodnotuClena<double>(levy) < ZiskejHodnotuClena<double>(pravy);
                case LogickyOperator.TypyOperatoru.VETSIROVNO:
                    return ZiskejHodnotuClena<double>(levy) >= ZiskejHodnotuClena<double>(pravy);
                case LogickyOperator.TypyOperatoru.MENSIROVNO:
                    return ZiskejHodnotuClena<double>(levy) <= ZiskejHodnotuClena<double>(pravy);
                case LogickyOperator.TypyOperatoru.OR:
                    return ZiskejHodnotuClena<bool>(levy) || ZiskejHodnotuClena<bool>(pravy);
                case LogickyOperator.TypyOperatoru.AND:
                    return ZiskejHodnotuClena<bool>(levy) && ZiskejHodnotuClena<bool>(pravy);
                case LogickyOperator.TypyOperatoru.NOT:
                    return !ZiskejHodnotuClena<bool>(pravy);
            }
            return false;
        }

        private T ZiskejHodnotuClena<T>(IClenVyrazu clen)
        {
            return ((IClenVyrazu<T>)clen).Hodnota;
        }
    }
}
