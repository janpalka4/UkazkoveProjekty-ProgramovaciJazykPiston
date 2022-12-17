using Piston.kompilace;
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
        private readonly SpravceTokenu spravceTokenu;
        private readonly SpravceSouboru spravceSouboru;
        private readonly Kompilator kompilator;

        public SpravceVstupu(PoskytovatelVyrazu poskytovatelVyrazu,SpravceTokenu spravceTokenu, SpravceSouboru spravceSouboru, Kompilator kompilator)
        {
            this.poskytovatelVyrazu = poskytovatelVyrazu;
            this.spravceTokenu = spravceTokenu;
            this.spravceSouboru = spravceSouboru;
            this.kompilator = kompilator;
        }

        public void ZpracujVstup(string[] args)
        {
            VstupInfo info = ZiskejInfo(args);


            if (info.Options.Length == 0 && info.Argumenty.Length == 0)
                VyzadatVyraz();
            else
            if (info.Options.Contains("c"))
            {
                if (info.Argumenty.Length != 1)
                    throw new Exception("Nesprávný počet argumentů");

                UzelProgramu uzel = kompilator.KompilujProgram(info.Argumenty[0]);
                if (info.Options.Contains("r"))
                    uzel.VykonatUzel();

            }
        }

        private VstupInfo ZiskejInfo(string[] args)
        {
            List<string> options = new List<string>();
            List<string> argumenty = new List<string>();

            foreach (string arg in args)
            {
                if (arg.StartsWith("--"))
                    options.Add(arg);
                else if (arg.StartsWith("-"))
                    foreach (char opt in arg.Substring(1))
                        options.Add(opt.ToString());
                else
                    argumenty.Add(arg);
            }

            return new VstupInfo() { Argumenty = argumenty.ToArray(), Options = options.ToArray() };
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
