using Piston.Vyjimky;
using Piston.Vyrazy.Aritmenticke;
using System.Text;

namespace Piston.Vyrazy.Logicke
{
    public class SpravceLogickychVyrazu : SpravceVyrazu
    {
        private readonly PoskytovatelVyrazu poskytovatelVyrazu;

        public SpravceLogickychVyrazu(PoskytovatelVyrazu poskytovatelVyrazu)
        {
            this.poskytovatelVyrazu = poskytovatelVyrazu;
        }

        public override LogickyVyraz VytvorVyraz(string vyraz)
        {
            string chybovaHlaska = ZvalidujRetezec(vyraz);
            if (chybovaHlaska != string.Empty)
                throw new ChybaValidace(chybovaHlaska);

            StringBuilder tmp = new StringBuilder();
            int i = 0;
            int vnoreni = 0;
            int preskocit = 0;

            List<IClenVyrazu> cleni = new List<IClenVyrazu>();

            foreach (char c in vyraz.Replace(" ", ""))
            {
                if(preskocit > 0)
                {
                    preskocit--;
                    i++;
                    continue;
                }

                if(c == '(')
                {
                    if (vnoreni == 0)
                    {
                        vnoreni++;
                        i++;
                        tmp.Clear();
                        continue;
                    }
                    vnoreni++;
                }

                if (c == ')')
                {
                    vnoreni--;
                    if (vnoreni == 0)
                    {
                        i++;
                        cleni.Add(RozlisVyraz(tmp.ToString()));
                        tmp.Clear();
                        continue;
                    }
                }

                if(vnoreni > 0)
                {
                    tmp.Append(c);
                    i++;
                    continue;
                }

                string operace = $"{ZiskejZnak(i,vyraz)}{ZiskejZnak(i+1,vyraz)}{ZiskejZnak(i+2,vyraz)}";
                if(ZiskejTypOperatoru(operace, out LogickyOperator.TypyOperatoru typOperatoru,out int delkaSlova))
                {
                    if(tmp.Length > 0)
                        cleni.Add(RozlisVyraz(tmp.ToString()));
                    tmp.Clear();

                    cleni.Add(new LogickyOperator(typOperatoru));

                    preskocit = delkaSlova - 1;
                    i++;
                    continue;
                }

                tmp.Append(c);
                i++;
            }

            if (tmp.Length > 0)
                cleni.Add(RozlisVyraz(tmp.ToString()));
            tmp.Clear();

            return new LogickyVyraz(cleni);
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

        private bool ZiskejTypOperatoru(string s, out LogickyOperator.TypyOperatoru typ, out int delka)
        {
            if (s != "NOT" && s != "AND")
                s = s.Substring(0, 2);

            if ((s[0] == '>' || s[0] == '<') && s[1] != '=')
                s = s.Substring(0, 1);

            delka = s.Length;

            switch (s)
            {
                case "==":
                    typ = LogickyOperator.TypyOperatoru.ROVNO;
                    return true;
                case "NOT":
                    typ = LogickyOperator.TypyOperatoru.NOT;
                    return true;
                case "AND":
                    typ = LogickyOperator.TypyOperatoru.AND;
                    return true;
                case "OR":
                    typ = LogickyOperator.TypyOperatoru.OR;
                    return true;
                case ">":
                    typ = LogickyOperator.TypyOperatoru.VETSI;
                    return true;
                case "<":
                    typ = LogickyOperator.TypyOperatoru.MENSI;
                    return true;
                case ">=":
                    typ = LogickyOperator.TypyOperatoru.VETSIROVNO;
                    return true;
                case "<=":
                    typ = LogickyOperator.TypyOperatoru.MENSIROVNO;
                    return true;

                default:
                    typ = LogickyOperator.TypyOperatoru.ROVNO;
                    return false;
            }
        }

        private bool ZiskejTypOperatoru(string c)
        {
            LogickyOperator.TypyOperatoru typ;
            int d;
            return ZiskejTypOperatoru(c, out typ,out d);
        }

        public static bool JeVyrazValidni(string vyraz) => vyraz.Contains('>') || vyraz.Contains('<') || vyraz.Contains("==") || vyraz.Contains("AND") || vyraz.Contains("OR") || vyraz.Contains("NOT");
        private char ZiskejZnak(int index, string str) => index < str.Replace(" ","").Length && index >= 0 ? str.Replace(" ","")[index] : ' ';
        private IClenVyrazu RozlisVyraz(string vyraz)
        {
            string bezZavorek = vyraz.Replace("(", "").Replace(")", "");
            if (double.TryParse(bezZavorek, out double d))
                return new Cislo(d);
            if (bool.TryParse(bezZavorek, out bool b))
                return new LogickaHodnota() { Hodnota = b};

            if (SpravceAritmetickychVyrazu.JeVyrazValidni(vyraz))
                return poskytovatelVyrazu.ZiskejSpravce(vyraz).VytvorVyraz(vyraz);

            return VytvorVyraz(vyraz);
        }
    }
}
