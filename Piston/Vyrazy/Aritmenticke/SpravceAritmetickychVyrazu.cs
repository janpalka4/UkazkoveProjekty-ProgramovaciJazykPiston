using Piston.Vyjimky;
using Piston.Vyrazy.Logicke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyrazy.Aritmenticke
{
    public class SpravceAritmetickychVyrazu : SpravceVyrazu
    {

        public SpravceAritmetickychVyrazu()
        {
        }

        public override AritmetickyVyraz VytvorVyraz(string vyraz)
        {
            string chybovaHlaska = ZvalidujRetezec(vyraz);
            if (chybovaHlaska != string.Empty)
                throw new ChybaValidace(chybovaHlaska);

            StringBuilder tmp = new StringBuilder();
            int i = 0;
            int vnoreni = 0;
            char predchoziZnak = ' ';

            List<IClenVyrazu<double>> cleni = new List<IClenVyrazu<double>>();

            foreach (char c in vyraz.Replace(" ", ""))
            {
                if (c == '(')
                {
                    //Zkouška jestli nechybí operátor
                    if (tmp.Length > 0)
                        throw new ChybaValidace("Očekáván operátor");
                    predchoziZnak = '(';
                    if (vnoreni > 0)
                        tmp.Append(c);
                    vnoreni++;
                    i++;
                    continue;
                }

                if (predchoziZnak == '(')
                {
                    if (c == ')')
                    {
                        vnoreni--;
                        i++;
                        if (vnoreni == 0)
                        {
                            predchoziZnak = ' ';
                            cleni.Add((IClenVyrazu<double>)VytvorVyraz(tmp.ToString()));
                            tmp.Clear();
                        }
                        else
                            tmp.Append(')');
                        continue;
                    }
                    i++;
                    tmp.Append(c);
                    continue;
                }

                Operator.TypyOperatoru typOperatoru;
                if (ZiskejTypOperatoru(c, out typOperatoru) && !ZiskejTypOperatoru(predchoziZnak) && i != 0)
                {
                    if (tmp.Length > 0)
                        cleni.Add(new Cislo(tmp.ToString()));
                    tmp.Clear();
                    cleni.Add(new Operator(typOperatoru));
                }
                else
                {
                    /* if (c == '.')
                         tmp.Append(',');
                     else*/
                    tmp.Append(c);
                }

                predchoziZnak = c;
                i++;
            }
            if (tmp.Length > 0)
                cleni.Add(new Cislo(tmp.ToString()));
            tmp.Clear();

            return new AritmetickyVyraz(cleni);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vyraz"></param>
        /// <returns>Chybová hláška</returns>
        private string ZvalidujRetezec(string vyraz)
        {
            if (vyraz.Count(x => x == '(') > vyraz.Count(x => x == ')'))
                return "Očekáváno ')'";

            if (vyraz.Count(x => x == '(') < vyraz.Count(x => x == ')'))
                return "Očekáváno '('";

            return string.Empty;
        }

        private bool ZiskejTypOperatoru(char c, out Operator.TypyOperatoru typ)
        {
            int id = c;
            typ = Operator.TypyOperatoru.PLUS;

            if (Enum.IsDefined(typeof(Operator.TypyOperatoru), id))
            {
                typ = (Operator.TypyOperatoru)id;
                return true;
            }
            return false;
        }
        private bool ZiskejTypOperatoru(char c)
        {
            Operator.TypyOperatoru typ;
            return ZiskejTypOperatoru(c, out typ);
        }

        public static bool JeVyrazValidni(string vyraz) => (vyraz.Contains('+') || vyraz.Contains('-') || vyraz.Contains('/') || vyraz.Contains('*') || vyraz.Contains('^') || vyraz.Contains('ˇ') || vyraz.Any(x => char.IsDigit(x))) && !vyraz.Contains("==") && !vyraz.Contains('"');
    }
}
