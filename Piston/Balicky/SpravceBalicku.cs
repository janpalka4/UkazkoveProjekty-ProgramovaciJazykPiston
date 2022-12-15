using Piston.Konfigurace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Balicky
{
    public class SpravceBalicku
    {
        private Dictionary<string, MethodInfo> RegistrMetod = new Dictionary<string, MethodInfo>();
        public void NactiBalicky(KonfiguraceAplikace konfigurace)
        {
            if (konfigurace.Balicky is not null)
                foreach (BalicekKonfigurace balicek in konfigurace.Balicky)
                {
                    Assembly sestaveni = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.Split(',')[0] == balicek.NazevSestaveni)
                        ?? throw new Exception("Balicek nenalezen");

                    List<Type> tridy = sestaveni.GetTypes().Where(x => x.IsDefined(typeof(Balicek), false)).ToList();

                    foreach(Type trida in tridy)
                    {
                        List<MethodInfo> metody = trida.GetMethods().Where(x => x.IsDefined(typeof(Metoda), false)).ToList();

                        foreach(MethodInfo metoda in metody)
                        {
                            string nazev = metoda.GetCustomAttribute<Metoda>()!.Jmeno;
                            RegistrMetod.Add(nazev, metoda);
                        }
                    }
                }
        }

        public bool JeMetodaZaregistrovana(string jmeno) => RegistrMetod.ContainsKey(jmeno);

        public object? VolejMetodu(string jmeno,params object[] args)
        {
            if (!JeMetodaZaregistrovana(jmeno))
                throw new Exception("Metoda nebyla zaregistrovana");

            MethodInfo info = RegistrMetod[jmeno];

            return info.Invoke(null,parameters: args);
        }
    }
}
