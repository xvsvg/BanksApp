using Spectre.Console;

namespace Banks.Console.SpectreConsoleAdapter
{
    public static class SpectreConsoleWriter
    {
        public static string WriteWithPrompt(string message)
        {
            return AnsiConsole.Ask<string>(message + "\n>");
        }

        public static void WriteSuccess(string message)
        {
            AnsiConsole.Write(new Markup($"[green]{message}[/]\n"));
        }

        public static void WriteError(string message)
        {
            AnsiConsole.Write(new Markup($"[red]{message}[/]\n"));
        }

        public static string Choice(string title, string text, string instruction, string choiceName, params string[] choices)
        {
            List<string> choice = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                .PageSize(10)
                .Title(title)
                .MoreChoicesText(text)
                .InstructionsText(instruction)
                .AddChoiceGroup(choiceName, choices));

            if (choice.Count != 1)
            {
                WriteWithPrompt("Provide [red]only 1[/] answer");
                return Choice(title, text, instruction, choiceName, choices);
            }
            else
            {
                return choice.First();
            }
        }

        public static string SecretPrompt(string message)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<string>(message)
                .PromptStyle("red")
                .Secret());
        }

        public static void Clear()
            => AnsiConsole.Clear();
    }
}