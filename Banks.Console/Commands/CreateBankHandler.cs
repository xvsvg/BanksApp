using Banks.Console.SpectreConsoleAdapter;
using Banks.Controller;

namespace Banks.Console.Commands
{
    public class CreateBankHandler : CommandHandler
    {
        private readonly BankController _controller;

        public CreateBankHandler(BankController controller)
        {
            _controller = controller;
        }

        public override void Handle(string command)
        {
            command = command.ToLower();

            if (command.Contains("create") && command.Contains("bank"))
            {
                string name = SpectreConsoleWriter.WriteWithPrompt("Provide bank name");
                string commonInterestRate = SpectreConsoleWriter.WriteWithPrompt("Provide common interest rate");
                string commonChargeRate = SpectreConsoleWriter.WriteWithPrompt("Provide common charge rate");
                string deposits = SpectreConsoleWriter.WriteWithPrompt("Provide deposit bounds for deposit account, ex: 50000 100000 150000");
                string interests = SpectreConsoleWriter.WriteWithPrompt("Provide annual percent bounds for deposit account, ex: 3,5 4,5 5");

                try
                {
                    SpectreConsoleWriter.WriteSuccess(
                        $"Bank \'{_controller.CreateBank(name, commonInterestRate, commonChargeRate, deposits.Split(' '), interests.Split(' '))}\' was created.");
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