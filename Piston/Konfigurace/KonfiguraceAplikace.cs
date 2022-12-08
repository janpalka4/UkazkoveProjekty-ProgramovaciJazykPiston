using Newtonsoft.Json;
using Piston.Balicky;
using System.Reflection;

namespace Piston.Konfigurace
{
    public class KonfiguraceAplikace
    {
        [JsonProperty("PistonPackages")]
        public BalicekKonfigurace[]? Balicky { get; set; }

        public static KonfiguraceAplikace? NactiKonfiguraci()
        {
            string cfg = "";
            Assembly sestaveni = Assembly.GetExecutingAssembly();
            string cesta = sestaveni.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith("app.json"))
                ?? throw new Exception("Konfigurační soubor nenalezen");

            cfg = new StreamReader(sestaveni.GetManifestResourceStream(cesta)!).ReadToEnd();

            return JsonConvert.DeserializeObject<KonfiguraceAplikace>(cfg);
        }
    }
}
