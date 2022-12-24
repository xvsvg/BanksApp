using Banks.Console.Commands;
using Banks.Console.SpectreConsoleAdapter;
using Banks.Controller;
using Spectre.Console;

namespace Banks.Console
{
    public class BanksCLI
    {
        private readonly CommandHandler _handler;
        private readonly BankController _controller;

        public BanksCLI()
        {
            _controller = new BankController();

            _handler = new CreateCentralBankHandler(_controller);
            _handler
                .SetNext(new CreateBankHandler(_controller))
                .SetNext(new CreateAccountHandler(_controller))
                .SetNext(new MakeTransactionHandler(_controller))
                .SetNext(new MakeDepositHandler(_controller))
                .SetNext(new MakeWithdrawHandler(_controller))
                .SetNext(new FindAccountHandler(_controller));
        }

        public void StartInInteractiveMode()
        {
            while (true)
            {
                PrintTable();

                string command = SpectreConsoleReader.GetLine();

                if (command.Equals("clear"))
                    SpectreConsoleWriter.Clear();
                else if (command.Equals("exit"))
                    return;
                else _handler.Handle(command);
            }
        }

        private void PrintTable()
        {
            Table table = new Table().Centered();
            table.Width = 100;

            table.AddColumn(new TableColumn("[white]Commands[/]").Centered());

            table
                .AddRow(new Markup("[teal]create central bank[/]").Centered())
                .AddRow(new Markup("[teal]create bank[/]").Centered())
                .AddRow(new Markup("[teal]create account[/]").Centered())
                .AddRow(new Markup("[teal]make transaction[/]").Centered())
                .AddRow(new Markup("[teal]make deposit[/]").Centered())
                .AddRow(new Markup("[teal]make withdraw[/]").Centered())
                .AddRow(new Markup("[teal]find account[/]").Centered())
                .AddRow(new Markup("[teal]clear[/]").Centered())
                .AddRow(new Markup("[teal]exit[/]").Centered());

            table.Border = TableBorder.Rounded;
            AnsiConsole.Write(table);
        }
    }
}