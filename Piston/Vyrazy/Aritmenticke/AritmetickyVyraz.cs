using Piston.Vyjimky;
using System.Linq;

namespace Piston.Vyrazy.Aritmenticke
{
    public class AritmetickyVyraz : IClenVyrazu<double>
    {
        public double Hodnota { get => VyhodnotitVyraz(); set => throw new Exception("Nelze nastavit hodnota vyrazu"); }
        public List<IClenVyrazu<double>> Cleni { get; set; }

        public AritmetickyVyraz(List<IClenVyrazu<double>> CleniVyrazu) => Cleni = CleniVyrazu;

        private double VyhodnotitVyraz()
        {
            //Pokud má výraz pouze jeden člen nehledat další operace(výsledek nebo samotný výraz k vyhodnocení)
            if (Cleni.Count == 1)
                return Cleni[0].Hodnota;

            //Podmínky za kterých nemá smysl zpracovávat výraz
            if (Cleni.Count == 0 || Cleni.Count(x => x is Operator) == 0)
            {
                throw new ChybaValidace("Prázdný výraz nebo chybějící operátor");
            }

            //Získání indexu operace která je právě na řadě
            int AktualniOperace = 0;
            int minPoradi = 10;

            for (int i = 0; i < Cleni.Count; i++)
            {
                if (Cleni[i] is not Operator)
                    continue;

                int poradi = ((Operator)Cleni[i]).Poradi;

                if (poradi < minPoradi)
                    AktualniOperace = i;
            }

            //Provedení operace
            IClenVyrazu<double> LevyClen = Cleni[AktualniOperace - 1];
            IClenVyrazu<double> PravyClen = Cleni[AktualniOperace + 1];
            Operator Operace = (Operator)Cleni[AktualniOperace];

            double vysledekOperace = ProvedOperaci(LevyClen, PravyClen, Operace);

            Cleni[AktualniOperace] = new Cislo(vysledekOperace);
            Cleni.RemoveAt(AktualniOperace - 1);
            Cleni.RemoveAt(AktualniOperace);

            //Pokračovat dalším krokem
            return VyhodnotitVyraz();
        }

        private double ProvedOperaci(IClenVyrazu<double> levyClen, IClenVyrazu<double> pravyClen, Operator operace)
        {
            switch (operace.Typ)
            {
                case Operator.TypyOperatoru.PLUS:
                    return levyClen.Hodnota + pravyClen.Hodnota;

                case Operator.TypyOperatoru.MINUS:
                    return levyClen.Hodnota - pravyClen.Hodnota;

                case Operator.TypyOperatoru.KRAT:
                    return levyClen.Hodnota * pravyClen.Hodnota;

                case Operator.TypyOperatoru.DELENO:
                    if (pravyClen.Hodnota == 0)
                        throw new ChybaOperace("Pokus o dělení nulou.");

                    return levyClen.Hodnota / pravyClen.Hodnota;

                case Operator.TypyOperatoru.MOCNINA:
                    return Math.Pow(levyClen.Hodnota, pravyClen.Hodnota);

                case Operator.TypyOperatoru.ODMOCNINA:
                    if (levyClen.Hodnota < 0)
                        throw new ChybaOperace("Pokus o odmocnění záporného čísla.");

                    return Math.Pow(levyClen.Hodnota, 1.0 / pravyClen.Hodnota);

                case Operator.TypyOperatoru.MODULO:
                    return levyClen.Hodnota % pravyClen.Hodnota;

                default:
                    return 0;
            }
        }
    }
}
