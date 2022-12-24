using Banks.Domain.Contracts.BankAccountConfigurations;
using Banks.Domain.Contracts.Banks;
using Banks.Domain.Contracts.Banks.Tools;
using Banks.Domain.Contracts.ClientID;
using Banks.Domain.Contracts.Clients;
using Banks.Domain.Contracts.Clocks;
using Banks.Domain.Contracts.Receipts;
using Banks.Domain.Extensions;
using Banks.Domain.Implementations.AccountOperations;
using Banks.Domain.Implementations.BankAccounts;
using Banks.Domain.Implementations.ClientID;
using Banks.Domain.Implementations.ClientID.Clients;
using Banks.Domain.Implementations.Clocks;

namespace Banks.Controller;

public class BankController
{
    private CentralBank? _centralBank;

    public CentralBank CreateCentralBank(string capital, string dayDuration, string monthDuration)
    {
        IClock clock = new IntegerClock(int.Parse(dayDuration), int.Parse(monthDuration));

        _centralBank = new CentralBank(clock, decimal.Parse(capital));

        return _centralBank;
    }

    public CentralBank CreateCentralBank(string capital)
    {
        throw new NotImplementedException();
    }

    public Bank CreateBank(string bankName, string commonInterestRate, string commonChargeRate, IEnumerable<string> deposits, IEnumerable<string> interests)
    {
        var deps = new List<decimal>();
        var ints = new List<decimal>();

        deposits.ForEach(deposit => deps.Add(decimal.Parse(deposit)));
        interests.ForEach(interest => deps.Add(decimal.Parse(interest)));

        if (_centralBank is null)
            throw new InvalidOperationException("Create central bank first.");

        return _centralBank.AddBank(new Bank(_centralBank, new BankConfiguration(
            bankName,
            decimal.Parse(commonInterestRate),
            decimal.Parse(commonChargeRate),
            new DepositAccountConfiguration(
                deps,
                ints))));
    }

    public BankAccount CreateRussianBankAccount(
        string name,
        string userData,
        string accountType,
        string money,
        string? expirationDate,
        string? overdraft)
    {
        if (_centralBank is null)
            throw new InvalidOperationException("Create central bank first");

        Bank? bank = _centralBank.Banks.Where(b => b.ToString().Equals(name)).FirstOrDefault();

        if (bank is null)
            throw new InvalidOperationException("Bank with specified name does not exist.");

        IClient client = CreateRussianClient(userData);

        BankAccount? account = null;
        if (accountType.Equals("credit"))
            account = CreateCreditAccount(bank, client, money, expirationDate);

        if (accountType.Equals("deposit"))
            account = CreateDepositAccount(bank, client, money, overdraft, expirationDate);

        if (accountType.Equals("debit"))
            account = CreateDebitAccount(bank, client, money, overdraft, expirationDate);

        if (account is null)
            throw new InvalidOperationException("Specified account type is not supported.");

        return bank.AddBankAccount(account);
    }

    public IReceipt MakeTransaction(
        string clientBankName,
        IClientID client,
        string clientAccountType,
        string recepientBankName,
        IClientID recepient,
        string recepientAccountType,
        string total)
    {
        if (_centralBank is null)
            throw new InvalidOperationException("Create central bank first.");

        Bank? clientBank = _centralBank.Banks.Where(b => b.ToString().Equals(clientBankName)).FirstOrDefault();
        Bank? recepientBank = _centralBank.Banks.Where(b => b.ToString().Equals(recepientBankName)).FirstOrDefault();

        if (clientBank is null)
            throw new InvalidOperationException("Specified clients' bank does not exist.");

        BankAccount? clientAccount = clientBank
            .FindBankAccount(client)
            .SingleOrDefault(i => i.Account.GetType().ToString().Equals(clientAccountType))?.Account;

        if (recepientBank is null)
            throw new InvalidOperationException("Specified recepients' bank does not exist.");

        BankAccount? recepientAccount = recepientBank
            .FindBankAccount(recepient)
            .SingleOrDefault(i => i.Account.GetType().ToString() == recepientAccountType)?.Account;

        if (clientAccount is null || recepientAccount is null)
            throw new InvalidOperationException("Some user's hadn't found");

        clientBank.MakeTransaction(new MakeTransaction(new List<OperationOrder> { new OperationOrder(clientAccount, recepientAccount) }, decimal.Parse(total)));

        return clientAccount.AccountHistory.Last();
    }

    public IReceipt MakeDeposit(IClientID id, string bankName, string accountType, string total)
    {
        if (_centralBank is null)
            throw new InvalidOperationException("Create central bank first.");

        Bank bank = _centralBank.Banks.Single(i => i.ToString().Equals(bankName));

        BankAccount? clientAccount = bank
            .FindBankAccount(id).FirstOrDefault(i => i.Account.GetType().ToString().Equals(accountType))
            ?.Account;

        if (clientAccount is null)
            throw new InvalidOperationException("User not found.");

        bank.MakeTransaction(new MakeDeposit(clientAccount, decimal.Parse(total)));

        return clientAccount.AccountHistory.Last();
    }

    public IReceipt MakeWithdraw(IClientID id, string bankName, string accountType, string total)
    {
        if (_centralBank is null)
            throw new InvalidOperationException("Create central bank first.");

        Bank bank = _centralBank.Banks.Single(i => i.ToString().Equals(bankName));

        BankAccount? clientAccount = bank
            .FindBankAccount(id).FirstOrDefault(i => i.Account.GetType().ToString() == accountType)
            ?.Account;

        if (clientAccount is null)
            throw new InvalidOperationException("User not found.");

        bank.MakeTransaction(new Withdraw(clientAccount, decimal.Parse(total)));

        return clientAccount.AccountHistory.Last();
    }

    public BankAccount FindUserAccount(string bankName, string accountType, IClientID id)
    {
        if (_centralBank is null)
            throw new InvalidOperationException("Create central bank first.");

        Bank bank = _centralBank.Banks.Single(i => i.ToString().Equals(bankName));

        BankAccount? clientAccount = bank
            .FindBankAccount(id).FirstOrDefault(i => i.Account.GetType().ToString().Contains(accountType))
            ?.Account;

        if (clientAccount is null)
            throw new InvalidOperationException("User not found.");

        return clientAccount;
    }

    private IClient CreateRussianClient(string userData)
    {
        IClient client = Client.Builder
            .WithFirstName(userData.Split(' ')[0])
            .WithSecondName(userData.Split(' ')[1])
            .Build();

        if (userData.Split(' ').Length > 2)
        {
            client.Address = userData.Split(" ")[2];
            client.ID = new RussianClientID(int.Parse(userData.Split(" ")[3]));
        }

        return client;
    }

    private BankAccount CreateCreditAccount(Bank bank, IClient client, string money, string? expirationDate)
    {
        if (expirationDate is not null)
            return new CreditAccount(bank, client, decimal.Parse(money), DateTime.Parse(expirationDate));
        else
            return new CreditAccount(bank, client, decimal.Parse(money), null);
    }

    private BankAccount CreateDepositAccount(Bank bank, IClient client, string money, string? overdraft, string? expirationDate)
    {
        ArgumentNullException.ThrowIfNull(overdraft);
        ArgumentNullException.ThrowIfNull(expirationDate);

        return new DepositAccount(bank, client, decimal.Parse(money), bool.Parse(overdraft), DateTime.Parse(expirationDate));
    }

    private BankAccount CreateDebitAccount(Bank bank, IClient client, string money, string? overdraft, string? expirationDate)
    {
        ArgumentNullException.ThrowIfNull(overdraft);

        if (expirationDate is not null)
            return new DebitAccount(bank, client, decimal.Parse(money), bool.Parse(overdraft), DateTime.Parse(expirationDate));
        else
            return new DebitAccount(bank, client, decimal.Parse(money), bool.Parse(overdraft), null);
    }
}