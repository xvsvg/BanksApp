using Banks.Console.SpectreConsoleAdapter;
using Banks.Controller;
using Banks.Domain.Contracts.ClientID;
using Banks.Domain.Implementations.ClientID;

namespace Banks.Console.Commands
{
    public class MakeDepositHandler : CommandHandler
    {
        private readonly BankController _controller;

        public MakeDepositHandler(BankController controller)
        {
            _controller = controller;
        }

        public override void Handle(string command)
        {
            command = command.ToLower();

            if (command.Contains("make deposit"))
            {
                string clientCountry = SpectreConsoleWriter.Choice(
                    "Specify your nationality",
                    "[grey](Move up and down to reveal more options)[/]",
                    "[grey](Press [blue]spacebar[/] to choose an option, [green]enter[/] to accept)[/]",
                    "Countries",
                    "USA",
                    "RUS",
                    "ABH",
                    "AUS");

                if (!clientCountry.Equals("RUS"))
                    throw new NotImplementedException("Only RUS users provided yet.");

                string bankName = SpectreConsoleWriter.WriteWithPrompt("Specify your bank name.");

                string accountType = SpectreConsoleWriter.WriteWithPrompt("Specify your account type.");

                string userId = SpectreConsoleWriter.SecretPrompt("Provide your [green]ID number[/]");
                IClientID id = new RussianClientID(int.Parse(userId));

                string total = SpectreConsoleWriter.WriteWithPrompt("Provide amount of money, you want to deposit");

                try
                {
                    SpectreConsoleWriter.WriteSuccess($"Deposit {_controller.MakeDeposit(id, bankName, accountType, total)} was successfully made");
                }
                catch (Exception ex)
                {
                    SpectreConsoleWriter.WriteError($"Transaction was failed\n{ex.Message}");
                }
            }
            else
            {
                base.Handle(command);
            }
        }
    }
}