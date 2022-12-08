using Piston.Balicky;
using Piston.Konfigurace;
using Piston.Vyjimky;
using Piston.Vyrazy.Aritmenticke;
using Piston.Vyrazy.Logicke;
using System;


public static class Program
{
    private enum TypyLogu
    {
        ERROR,INFO,WARNING
    }
    struct VstupInfo
    {
        public string[] Options;
        public string[] Argumenty;
    }
    public static int Main(string[] args)
    {
        VstupInfo info = ZpracujVstup(args);

        KonfiguraceAplikace konfiguraceAplikace = KonfiguraceAplikace.NactiKonfiguraci()
            ?? throw new Exception("chyba při načítání konfigurace aplikace");

        SpravceBalicku.NactiBalicky(konfiguraceAplikace);

        try
        {
            if (info.Options.Length == 0 && info.Argumenty.Length == 0)
                VyzadatVyraz();

            return 0;
        }
        catch(Exception ex)
        {
            VypisLog(TypyLogu.ERROR, ex.Message);

            //Pokud nejsou zadány žádné argumenty pokračovat v uživatelském vstupu
            return info.Options.Length == 0 && info.Argumenty.Length == 0 ? Main(args) : -1;
        }
    }

    public static string VyzadatVyraz()
    {
        Console.Write("Piston>> ");

        string? vyraz = Console.ReadLine();

        //V budoucnu spravce prikazu
        if (vyraz == ":q")
            return "";

        if (vyraz is null)
            return VyzadatVyraz();

        if (SpravceAritmetickychVyrazu.JeVyrazAritmeticky(vyraz!))
            Console.WriteLine(SpravceAritmetickychVyrazu.VytvorVyraz(vyraz!).Hodnota);
        else
            Console.WriteLine(SpravceLogickychVyrazu.VytvorVyraz(vyraz!).Hodnota);

        return VyzadatVyraz();
    }

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

    private static VstupInfo ZpracujVstup(string[] args)
    {
        string[] options = new string[0];
        string[] argumenty = new string[0];

        foreach(string arg in args)
        {
            if(arg.StartsWith("--"))
                options.Append(arg);
            else if(arg.StartsWith("-"))
                foreach(char opt in arg.Substring(1))
                    options.Append(opt.ToString());
            else
                argumenty.Append(arg);
        }

        return new VstupInfo() { Argumenty = argumenty, Options = options };
    }
}
