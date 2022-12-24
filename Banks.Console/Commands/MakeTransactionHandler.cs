using Banks.Console.SpectreConsoleAdapter;
using Banks.Controller;
using Banks.Domain.Contracts.ClientID;
using Banks.Domain.Implementations.ClientID;

namespace Banks.Console.Commands
{
    public class MakeTransactionHandler : CommandHandler
    {
        private readonly BankController _controller;

        public MakeTransactionHandler(BankController controller)
        {
            _controller = controller;
        }

        public override void Handle(string command)
        {
            command = command.ToLower();

            if (command.Contains("make transaction"))
            {
                string clientBankName = SpectreConsoleWriter.WriteWithPrompt("Provide your bank name.");

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
                    throw new NotImplementedException("Non russian accounts is not supported");

                string id = SpectreConsoleWriter.SecretPrompt("Your ID, please.");
                IClientID client = new RussianClientID(int.Parse(id));

                string clientAccountType = SpectreConsoleWriter.Choice(
                "Specify desired account type",
                "[grey](Move up and down to reveal more options)[/]",
                "[grey](Press [blue]spacebar[/] to choose an option, [green]enter[/] to accept)[/]",
                "Account types",
                "deposit",
                "credit",
                "debit");

                string recepientBankName = SpectreConsoleWriter.WriteWithPrompt("Provide recepients' bank name.");

                string recepientCountry = SpectreConsoleWriter.Choice(
                    "Specify your nationality",
                    "[grey](Move up and down to reveal more options)[/]",
                    "[grey](Press [blue]spacebar[/] to choose an option, [green]enter[/] to accept)[/]",
                    "Countries",
                    "USA",
                    "RUS",
                    "ABH",
                    "AUS");

                if (!recepientCountry.Equals("RUS"))
                    throw new NotImplementedException("Non russian accounts is not supported");

                id = SpectreConsoleWriter.SecretPrompt("Recepient ID, please.");
                IClientID recepient = new RussianClientID(int.Parse(id));

                string recepientAccountType = SpectreConsoleWriter.Choice(
                "Specify desired account type",
                "[grey](Move up and down to reveal more options)[/]",
                "[grey](Press [blue]spacebar[/] to choose an option, [green]enter[/] to accept)[/]",
                "Account types",
                "deposit",
                "credit",
                "debit");

                string total = SpectreConsoleWriter.WriteWithPrompt("Specify total money to be transacted.");

                _controller.MakeTransaction(
                    clientBankName,
                    client,
                    clientAccountType,
                    recepientBankName,
                    recepient,
                    recepientAccountType,
                    total);
            }
            else
            {
                base.Handle(command);
            }
        }
    }
}