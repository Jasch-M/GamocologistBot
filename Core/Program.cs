using System.Threading.Tasks;
using TranslatorBot;

namespace Template
{
    internal static class Program
    {
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}