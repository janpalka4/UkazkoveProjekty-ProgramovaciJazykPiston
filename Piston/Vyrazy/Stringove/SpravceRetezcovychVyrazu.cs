using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vyrazy.Stringove
{
    public class SpravceRetezcovychVyrazu : SpravceVyrazu
    {
        public override RetezcovyVyraz VytvorVyraz(string vyraz)
        {
            string _vyraz = vyraz.Replace(" ", "");
            List<IClenVyrazu<string>> cleni = new List<IClenVyrazu<string>>();
            StringBuilder tmp = new StringBuilder();

            for (int i = 0; i < _vyraz.Length; i++)
            {
                if (_vyraz[i] == '"')
                {
                    i++;
                    while (_vyraz[i] != '"')
                    {
                        tmp.Append(_vyraz[i]);
                        i++;
                        continue;
                    }
                    cleni.Add(new HodnotaRetezce(tmp.ToString()));
                    tmp.Clear();
                }

                if (_vyraz[i] == '.' && _vyraz[i + 1] == '.')
                    cleni.Add(new RetezcovyOperator(RetezcovyOperator.TypyOperatoru.APPEND));
            }

            return new RetezcovyVyraz(cleni);
        }

        public static bool JeVyrazValidni(string vyraz) => true;
    }
}
