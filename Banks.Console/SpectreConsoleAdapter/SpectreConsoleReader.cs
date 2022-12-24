using Spectre.Console;

namespace Banks.Console.SpectreConsoleAdapter
{
    public static class SpectreConsoleReader
    {
        public static string GetLine()
        {
            return AnsiConsole.Ask<string>("> ");
        }
    }
}