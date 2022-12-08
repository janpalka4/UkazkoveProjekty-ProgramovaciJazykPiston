using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyrazy.Aritmenticke
{
    public class Operator : IClenVyrazu<double>
    {
        public enum TypyOperatoru
        {
            PLUS = '+', MINUS = '-', KRAT = '*', DELENO = '/', MOCNINA = '^', ODMOCNINA = 'ˇ', MODULO = '%'
        }

        public double Hodnota { get; set; }
        public int Poradi { get => ZiskejPoradi(); }
        public TypyOperatoru Typ { get; set; }

        public Operator(TypyOperatoru typ) => Typ = typ;

        private int ZiskejPoradi()
        {
            switch (Typ)
            {
                case TypyOperatoru.PLUS:
                    return 5;
                case TypyOperatoru.MINUS:
                    return 6;
                case TypyOperatoru.KRAT:
                    return 2;
                case TypyOperatoru.DELENO:
                    return 3;
                case TypyOperatoru.MOCNINA:
                    return 0;
                case TypyOperatoru.ODMOCNINA:
                    return 1;
                case TypyOperatoru.MODULO:
                    return 4;
                default:
                    return 6;
            }
        }
    }
}
