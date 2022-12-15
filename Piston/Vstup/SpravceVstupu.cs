using Piston.Vyrazy;
using Piston.Vyrazy.Aritmenticke;
using Piston.Vyrazy.Logicke;
using Piston.Vyrazy.Stringove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Vstup
{
    public class SpravceVstupu
    {
        private readonly PoskytovatelVyrazu poskytovatelVyrazu;

        public SpravceVstupu(PoskytovatelVyrazu poskytovatelVyrazu)
        {
            this.poskytovatelVyrazu = poskytovatelVyrazu;
        }

        public void ZpracujVstup(string[] args)
        {
            VstupInfo info = ZiskejInfo(args);


            if (info.Options.Length == 0 && info.Argumenty.Length == 0)
                VyzadatVyraz();
        }

        private VstupInfo ZiskejInfo(string[] args)
        {
            string[] options = new string[0];
            string[] argumenty = new string[0];

            foreach (string arg in args)
            {
                if (arg.StartsWith("--"))
                    options.Append(arg);
                else if (arg.StartsWith("-"))
                    foreach (char opt in arg.Substring(1))
                        options.Append(opt.ToString());
                else
                    argumenty.Append(arg);
            }

            return new VstupInfo() { Argumenty = argumenty, Options = options };
        }

        private string VyzadatVyraz()
        {
            Console.Write("Piston>> ");

            string? vyraz = Console.ReadLine();

            //V budoucnu spravce prikazu
            if (vyraz == ":q")
                return "";

            if (vyraz is null)
                return VyzadatVyraz();

             SpravceVyrazu spravce = poskytovatelVyrazu.ZiskejSpravce(vyraz);

            if (spravce is SpravceLogickychVyrazu)
                Console.WriteLine(((SpravceLogickychVyrazu)spravce).VytvorVyraz(vyraz).Hodnota);
            else
            if(spravce is SpravceAritmetickychVyrazu)
                Console.WriteLine(((SpravceAritmetickychVyrazu)spravce).VytvorVyraz(vyraz).Hodnota);
            else
            if(spravce is SpravceRetezcovychVyrazu)
                Console.WriteLine(((SpravceRetezcovychVyrazu)spravce).VytvorVyraz(vyraz).Hodnota);

            //Console.WriteLine(spravceRetezcovychVyrazu.VytvorVyraz(vyraz!).Hodnota);

            return VyzadatVyraz();
        }
    }

    struct VstupInfo
    {
        public string[] Options;
        public string[] Argumenty;
    }
}
