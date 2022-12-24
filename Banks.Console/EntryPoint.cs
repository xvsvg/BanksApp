namespace Banks.Console
{
    public class EntryPoint
    {
        private static readonly BanksCLI _cli;

        static EntryPoint()
        {
            _cli = new BanksCLI();
        }

        public static void Main()
        {
            _cli.StartInInteractiveMode();
        }
    }
}