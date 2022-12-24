using Banks.Console.SpectreConsoleAdapter;
using Banks.Controller;
using System.Text;

namespace Banks.Console.Commands
{
    public class CreateAccountHandler : CommandHandler
    {
        private readonly BankController _controller;

        public CreateAccountHandler(BankController controller)
        {
            _controller = controller;
        }

        public override void Handle(string command)
        {
            command = command.ToLower();

            if (command.Contains("create") && command.Contains("account"))
            {
                string answer = SpectreConsoleWriter.Choice(
                    "Specify your nationality",
                    "[grey](Move up and down to reveal more options)[/]",
                    "[grey](Press [blue]spacebar[/] to choose an option, [green]enter[/] to accept)[/]",
                    "Countries",
                    "USA",
                    "RUS",
                    "ABH",
                    "AUS");

                if (!answer.Equals("RUS"))
                    throw new NotImplementedException("Non russian accounts is not supported");

                string name, accountType, money;
                string? expirationDate, overdraft;
                StringBuilder userData;

                CreateRussianAccountHandler(
                    out name,
                    out userData,
                    out accountType,
                    out money,
                    out expirationDate,
                    out overdraft);

                try
                {
                    SpectreConsoleWriter
                        .WriteSuccess($"{_controller.CreateRussianBankAccount(name, userData.ToString(), accountType, money, expirationDate, overdraft)} was created");
                }
                catch (Exception ex)
                {
                    SpectreConsoleWriter
                        .WriteError($"Bank account \'{_controller.CreateRussianBankAccount(name, userData.ToString(), accountType, money, expirationDate, overdraft)}\' was failed\n{ex.Message}");
                }
            }
            else
            {
                base.Handle(command);
            }
        }

        private static void CreateRussianAccountHandler(out string name, out StringBuilder userData, out string accountType, out string money, out string? expirationDate, out string? overdraft)
        {
            name = SpectreConsoleWriter.WriteWithPrompt("Specify [green]bank name[/]");
            userData = new StringBuilder();
            userData.Append(SpectreConsoleWriter.WriteWithPrompt("Specify your [green]first name, second name[/]") + " ");

            string address = SpectreConsoleWriter.WriteWithPrompt("Specify your [green]address[/] ( you can skip this step )");
            if (!address.Equals("skip"))
                userData.Append(address + " ");

            string id = SpectreConsoleWriter.SecretPrompt("Specify your [green] ID number[/] ( you can skip this step )");
            if (!id.Equals("skip"))
                userData.Append(id + " ");

            accountType = SpectreConsoleWriter.Choice(
                "Specify desired account type",
                "[grey](Move up and down to reveal more options)[/]",
                "[grey](Press [blue]spacebar[/] to choose an option, [green]enter[/] to accept)[/]",
                "Account types",
                "deposit",
                "credit",
                "debit");

            money = SpectreConsoleWriter.WriteWithPrompt("Choose account's [green]money/loan[/]");

            if (!accountType.Equals("deposit"))
            {
                expirationDate = SpectreConsoleWriter
                    .WriteWithPrompt("Choose account's [green]expiration date[/] m/d/y ( you can skip this step)");
                if (expirationDate.Equals("skip"))
                    expirationDate = null;
            }
            else
            {
                expirationDate = SpectreConsoleWriter
                    .WriteWithPrompt("Choose account's [green]expiration date[/] m/d/y");
            }

            if (!accountType.Equals("credit"))
            {
                overdraft = SpectreConsoleWriter
                    .WriteWithPrompt($"Should we enable [yellow]overdraft[/]? {bool.TrueString}/{bool.FalseString} ( you can skip this step)");
                if (overdraft.Equals("skip"))
                    overdraft = bool.FalseString;
            }
            else
            {
                overdraft = bool.TrueString;
            }
        }
    }
}