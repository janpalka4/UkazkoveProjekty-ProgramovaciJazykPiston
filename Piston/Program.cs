using Piston.Balicky;
using Piston.Konfigurace;
using Piston.Vyjimky;
using Piston.Vyrazy.Aritmenticke;
using Piston.Vyrazy.Logicke;
using Piston.Vyrazy.Stringove;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Hosting;
using Piston.kompilace;
using Piston.Vstup;
using Piston.Vyrazy;

public static class Program
{
    private enum TypyLogu
    {
        ERROR,INFO,WARNING
    }
    public static int Main(string[] args)
    {
        //Dependency Injection
        IHost host = VytvorHostBuilder().Build();

        //Načtení app.json
        KonfiguraceAplikace konfiguraceAplikace = KonfiguraceAplikace.NactiKonfiguraci()
            ?? throw new Exception("chyba při načítání konfigurace aplikace");

        //Načtení balíčků
        SpravceBalicku? spravceBalicku = host.Services.GetService<SpravceBalicku>();
        if (spravceBalicku is null)
            throw new ChybaInicializace("Nepodařilo se načíst správce balíčků");
        spravceBalicku!.NactiBalicky(konfiguraceAplikace);

        try
        {
            SpravceVstupu? spravceVstupu = host.Services.GetService<SpravceVstupu>();
            if (spravceVstupu is null) throw new ChybaInicializace("Nepodařilo se načíst správce vstupu");

            spravceVstupu.ZpracujVstup(args);

            return 0;
        }
        catch(Exception ex)
        {
            VypisLog(TypyLogu.ERROR, ex.Message);
            return -1;
        }
    }


#warning TODO: vytvořit třídu logger
    private static void VypisLog(TypyLogu typ,string zprava)
    {
        ConsoleColor barva = Console.ForegroundColor;
        ConsoleColor predchoziBarva = Console.ForegroundColor;

        switch (typ)
        {
            case TypyLogu.ERROR:
                barva = ConsoleColor.Red;
                break;
            case TypyLogu.INFO:
                barva = ConsoleColor.Cyan;
                break;
            case TypyLogu.WARNING:
                barva = ConsoleColor.Yellow;
                break;
        }

        Console.Write("[");
        Console.ForegroundColor = barva;
        Console.Write(typ.ToString());
        Console.ForegroundColor = predchoziBarva;
        Console.Write("]: ");
        Console.Write(zprava + "\r\n");
    }

   
    /// <summary>
    /// Nastavení dependency injection
    /// </summary>
    /// <returns></returns>
    private static IHostBuilder VytvorHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices((_, service) =>
            {
                service.AddSingleton<SpravceTokenu>();
                service.AddSingleton<SpravceVstupu>();
                service.AddSingleton<SpravceBalicku>();
                service.AddSingleton<SpravceRetezcovychVyrazu>();
                service.AddSingleton<SpravceLogickychVyrazu>();
                service.AddSingleton<SpravceAritmetickychVyrazu>();
                service.AddSingleton<PoskytovatelVyrazu>();
                service.BuildServiceProvider();
            });
    }
}
