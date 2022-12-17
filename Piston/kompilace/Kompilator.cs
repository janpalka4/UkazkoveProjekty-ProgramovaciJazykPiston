using Piston.Balicky;
using Piston.Vstup;
using Piston.Vyjimky;
using Piston.Vyrazy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.kompilace
{
    public class Kompilator
    {
        private readonly SpravceTokenu spravceTokenu;
        private readonly PoskytovatelVyrazu poskytovatelVyrazu;
        private readonly SpravceBalicku spravceBalicku;
        private readonly SpravceSouboru spravceSouboru;

        public Kompilator(SpravceTokenu spravceTokenu, PoskytovatelVyrazu poskytovatelVyrazu, SpravceBalicku spravceBalicku,SpravceSouboru spravceSouboru) 
        { 
            this.spravceTokenu = spravceTokenu;
            this.poskytovatelVyrazu = poskytovatelVyrazu;
            this.spravceBalicku = spravceBalicku;
            this.spravceSouboru = spravceSouboru;
        }

        public UzelProgramu KompilujProgram(string program) => KompilujProgram(spravceTokenu.PrevedNaTokeny(spravceSouboru.PrectiTextovySoubor(program)));

        public UzelProgramu KompilujProgram(List<Token> tokeny)
        {
            List<UzelProgramu> ret = new List<UzelProgramu>();
            for(int i = 0; i < tokeny.Count; i++)
            {
                if (tokeny[i].Typ == TypTokenu.PODMINKA) {
                    if (tokeny[i + 2].Typ != TypTokenu.KONEC_RADKU)
                        throw new ChybaKompilace("Očekáván konec řádku");

                    Token vyrazToken = tokeny[i+1];
                    IClenVyrazu<bool> vyraz = (IClenVyrazu<bool>)poskytovatelVyrazu.ZiskejSpravce(vyrazToken.Hodnota!).VytvorVyraz(vyrazToken.Hodnota!);
                    List<Token> teloTokeny = new List<Token>();

                    i += 3;
                    while (tokeny[i].Typ != TypTokenu.KONEC)
                    {
                        teloTokeny.Add(tokeny[i]);
                        i++;
                    }

                    UzelProgramu teloUzel = KompilujProgram(teloTokeny);

                    UzelProgramu uzel = new UzelProgramu((instance, obj) =>
                    {
                        if (vyraz.Hodnota)
                            instance.Vetve.First().VykonatUzel();

                        return true;
                    })
                    {
                        Vetve = new UzelProgramu[] { teloUzel }
                    };
                    ret.Add(uzel);
                }
                else
                if (tokeny[i].Typ == TypTokenu.METODA)
                {
                    Token tokenMetoda = tokeny[i];
                    List<Token> argumenty = new List<Token>();
                    i++;

                    while (tokeny[i].Typ == TypTokenu.ARGUMENT)
                    {
                        argumenty.Add(tokeny[i]);
                        i++;
                    }

                    UzelProgramu uzel = new UzelProgramu((instance, obj) =>
                    {
                        return spravceBalicku.VolejMetodu(tokenMetoda.Hodnota, argumenty.Select(x => poskytovatelVyrazu.ZiskejSpravce(x.Hodnota).VytvorVyraz(x.Hodnota)).ToArray());
                    });
                    ret.Add(uzel);
                }
            }
            return new UzelProgramu((instance, obj) =>
            {
                foreach (UzelProgramu uzel in instance.Vetve)
                    uzel.VykonatUzel();

                return true;
            })
            { Vetve = ret.ToArray()};
        }
    }
}
