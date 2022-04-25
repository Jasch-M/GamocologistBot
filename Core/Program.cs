using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranslatorBot.Modules.Translation;

namespace TranslatorBot
{
    internal class Program
    {
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}