using Piston.Vyrazy.Aritmenticke;
using Piston.Vyrazy.Logicke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.kompilace
{
    public class SpravceTokenu
    {
        public List<Token> PrevedNaTokeny(string Kod)
        {
            List<Token> ret = new List<Token>();

            string[] radky = Kod.Split("\r\n");
            StringBuilder klicoveSlovoBuilder = new StringBuilder();

            foreach(string radek in radky)
            {
                string _radek = radek.Replace(" ", "");

                if (_radek.StartsWith("var"))
                    ret.AddRange(ZpracujDeklaraciPromenne(radek));
                else
                if (_radek.StartsWith("if"))
                    ret.AddRange(ZpracujPodminku(radek));
                else
                if (_radek.StartsWith("end"))
                    ret.Add(new Token() { Typ = TypTokenu.KONEC });
                else //Pokud nezačíná na známý keyword
                {
                    if (SpravceAritmetickychVyrazu.JeVyrazAritmeticky(radek) || SpravceLogickychVyrazu.JeVyrazLogicky(radek))
                        ret.Add(new Token() { Typ = TypTokenu.VYRAZ, Hodnota = radek});
                    else //Jedná se o metodu
                    {
#warning TODO: implementovat definiční scope metod (zatím se použije pouze správce balíčků)

                    }
                }

                ret.Add(new Token() { Typ = TypTokenu.KONEC_RADKU });
            }

            return ret;
        }

        private List<Token> ZpracujDeklaraciPromenne(string radek)
        {
            List<Token> ret = new List<Token>();
            string[] strany = radek.Split('=');
            string nazev = strany[0].Split(" ").Last();

            ret.Add(new Token() { Typ = TypTokenu.VAR, Hodnota = "" });
            ret.Add(new Token() { Typ = TypTokenu.NAZEV, Hodnota = nazev });
            ret.Add(new Token() { Typ = TypTokenu.VYRAZ, Hodnota = strany[1] });

            return ret;
        }

        private List<Token> ZpracujPodminku(string radek)
        {
            List<Token> ret = new List<Token>();
            string vyraz = radek.Replace("if","").Replace("do","");

            ret.Add(new Token() { Typ = TypTokenu.PODMINKA, Hodnota = "" });
            ret.Add(new Token() { Typ = TypTokenu.VYRAZ, Hodnota = vyraz });

            return ret;
        }
    }
}
