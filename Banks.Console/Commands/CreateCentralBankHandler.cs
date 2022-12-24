using Banks.Console.SpectreConsoleAdapter;
using Banks.Controller;

namespace Banks.Console.Commands
{
    public class CreateCentralBankHandler : CommandHandler
    {
        private readonly BankController _controller;

        public CreateCentralBankHandler(BankController controller)
        {
            _controller = controller;
        }

        public override void Handle(string command)
        {
            command = command.ToLower();

            if (command.Contains("create") && command.Contains("central bank"))
            {
                string answer = SpectreConsoleWriter.Choice(
                    "Provide local time",
                    "[grey](Move up and down to reveal more options)[/]",
                    "[grey](Press [blue]spacebar[/] to choose an option, [green]enter[/] to accept)[/]",
                    "Timers",
                    "Local time",
                    "Accelerated time");

                string prompt = string.Empty;
                if (answer.Equals("Accelerated time"))
                {
                    prompt = SpectreConsoleWriter
                        .WriteWithPrompt("[green]day duration[/] and [green]month duration[/] in milliseconds? Ex: [green] 1000 10000 [/]");
                }

                string capital = SpectreConsoleWriter
                    .WriteWithPrompt("provide bank [green]capital[/]");

                try
                {
                    if (prompt == string.Empty)
                    {
                        SpectreConsoleWriter.WriteSuccess(
                        $"{_controller.CreateCentralBank(capital)} was successfully created.");
                    }
                    else
                    {
                        SpectreConsoleWriter.WriteSuccess(
                        $"{_controller.CreateCentralBank(capital, prompt.Split(' ')[0], prompt.Split(' ')[1])} was successfully created.");
                    }
                }
                catch (Exception ex)
                {
                    SpectreConsoleWriter.WriteError($"Bank creation failed\n{ex.Message}");
                }
            }
            else
            {
                base.Handle(command);
            }
        }
    }
}