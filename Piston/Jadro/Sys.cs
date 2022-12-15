﻿using Piston.Balicky;
using Piston.Vyrazy;
using Piston.Vyrazy.Stringove;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piston.Balicky.System
{
    [Balicek("sys",JeStaticky = true)]
    public class Sys
    {
        [Metoda("println")]
        public static void println(string output) => Console.WriteLine(output);

        [Metoda("print")]
        public static void print(string output) => Console.WriteLine(output);

        [Metoda("cprint")]
        public static void cprint(string output)
        {
            StringBuilder tmp = new StringBuilder();
            for(int i = 0; i < output.Length;i++)
            {
                if (output[i] == '´' && output[i + 1] == '#')
                    while(tmp.Length == 0 || output[i] != '´')
                    {
                        if (output[i] != '´')
                            tmp.Append(output[i]);
                        if (output[i + 1] == '´')
                        {
                            Console.ForegroundColor = Enum.Parse<ConsoleColor>(tmp.ToString());
                            tmp.Clear();
                            break;
                        }
                        i++;
                    }

               Console.Write(output[i]);
            }
        }

        [Metoda("cprintln")]
        public static void cprintln(string output) => cprint(output + "\r\n");
        
        [Metoda("str")]
        public static HodnotaRetezce str(IClenVyrazu expr)
        {
            if (expr is IClenVyrazu<bool>)
                return new HodnotaRetezce(((IClenVyrazu<bool>)expr).Hodnota.ToString());
            if (expr is IClenVyrazu<double>)
                return new HodnotaRetezce(((IClenVyrazu<double>)expr).Hodnota.ToString());
            if (expr is IClenVyrazu<string>)
                return new HodnotaRetezce(((IClenVyrazu<string>)expr).Hodnota);

            return new HodnotaRetezce(string.Empty);
        }
    }
}
