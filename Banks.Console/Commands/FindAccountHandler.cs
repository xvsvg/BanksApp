using Banks.Console.SpectreConsoleAdapter;
using Banks.Controller;
using Banks.Domain.Contracts.ClientID;
using Banks.Domain.Implementations.ClientID;

namespace Banks.Console.Commands
{
    public class FindAccountHandler : CommandHandler
    {
        private readonly BankController _controller;

        public FindAccountHandler(BankController controller)
        {
            _controller = controller;
        }

        public override void Handle(string command)
        {
            command = command.ToLower();

            if (command.Contains("find"))
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

                try
                {
                    Domain.Contracts.BankAccountConfigurations.BankAccount account = _controller.FindUserAccount(bankName, accountType, id);
                    SpectreConsoleWriter.WriteSuccess($"Account of bank: {account.Bank}\nMoney: {account.Money}\nAccount expires on:{account?.ExpirationDate}\nAccount is suspicious: {account?.IsSuspicious}");
                }
                catch (Exception ex)
                {
                    SpectreConsoleWriter.WriteError($"User account was not found\n{ex.Message}");
                }
            }
            else
            {
                base.Handle(command);
            }
        }
    }
}