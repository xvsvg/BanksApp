using Banks.Domain.Contracts.BankAccountConfigurations;
using Banks.Domain.Contracts.Banks;
using Banks.Domain.Contracts.Banks.Tools;
using Banks.Domain.Contracts.Clients;
using Banks.Domain.Contracts.Clocks;
using Banks.Domain.Exceptions;
using Banks.Domain.Implementations.AccountOperations;
using Banks.Domain.Implementations.BankAccounts;
using Banks.Domain.Implementations.ClientID;
using Banks.Domain.Implementations.ClientID.Clients;
using Banks.Domain.Implementations.Clocks;
using Xunit;

namespace Banks.Test
{
    public class BanksTest
    {
        private readonly Domain.Contracts.Banks.CentralBank _centralBank;
        private readonly Bank _bank;
        private readonly IClock? _clock;
        private readonly IClient? _clientA;
        private readonly Dictionary<IClient, List<BankAccountInfo>> _accounts;

        public BanksTest()
        {
            _clock = new IntegerClock(1000, 10);

            _centralBank = new CentralBank(_clock, 100_000);

            _accounts = new Dictionary<IClient, List<BankAccountInfo>>();

            _bank = new Bank(
                _centralBank,
                new BankConfiguration(
                    "Tinkoff Bank",
                    commonInterestRate: 3.65m,
                    commonChargeRate: 3.65m,
                    new DepositAccountConfiguration(
                        new List<decimal> { 50_000m, 100_000m },
                        new List<decimal> { 3m, 3.5m, 4m })));

            _clientA = Client.Builder
                .WithFirstName("John")
                .WithSecondName("Doe")
                .WithId(new RussianClientID(334803))
                .WithAddress("Kronverskiy 49")
                .Build();

            _clock.StartClockAsync();
        }

        [Fact]
        public void BankAccountTest()
        {
            BankAccountAssert();

            Assert.Single(_bank.Clients);
            Assert.Single(_bank.Accounts);
            Assert.Equal(365, _bank.Accounts.First().Account.Money);
            Assert.False(_bank.Clients.First().IsSuspicious);
            Assert.Empty(_bank.Accounts.First().Account.AccountHistory);
        }

        [Fact]
        public void InterestRateAndChargesTest()
        {
            IClient clientB;
            BankAccount? clientAAccount, clientBAccount;
            BankAccount? creditAccount, debitAccount;

            BankAccountAssert();
            BankAccountOperationAssert1(out clientB, out clientAAccount, out clientBAccount);
            BankAccountOperationAssert2(clientB, clientAAccount, clientBAccount);
            BankAccountOperationAssert3(clientAAccount);
            InterestRateAndChargesAssert(out creditAccount, out debitAccount);

            Assert.Equal(-50_045, creditAccount?.Money);
            Assert.Equal(365.36M, debitAccount?.Money);
        }

        internal void BankAccountOperationsTest()
        {
            BankAccountAssert();
            IClient clientB;
            BankAccount? clientAAccount, clientBAccount;

            BankAccountOperationAssert1(out clientB, out clientAAccount, out clientBAccount);

            Assert.NotNull(clientAAccount);
            Assert.NotNull(clientBAccount);

            Assert.Throws<AccountOperationException>(() =>
            _bank.MakeTransaction(new MakeTransaction(new List<OperationOrder> { new OperationOrder(clientAAccount!, clientBAccount!) }, 400)));

            BankAccountOperationAssert2(clientB, clientAAccount, clientBAccount);

            Assert.Equal(-35, clientAAccount?.Money);
            Assert.Equal(400, clientBAccount?.Money);
            Assert.Single(clientAAccount?.AccountHistory);
            Assert.Single(clientBAccount?.AccountHistory);

            BankAccountOperationAssert3(clientAAccount);

            Assert.Equal(365, clientAAccount?.Money);
            Assert.Equal(0, clientBAccount?.Money);
            Assert.True(clientAAccount?.AccountHistory.Last().IsCancelled);
            Assert.True(clientBAccount?.AccountHistory.Last().IsCancelled);
        }

        private void BankAccountAssert()
        {
            var acc = new DebitAccount(_bank, _clientA!, 365, true, null);

            _bank!.AddBankAccount(acc);

            if (_accounts.ContainsKey(_clientA!) is false)
                _accounts.Add(_clientA!, new List<BankAccountInfo> { _bank.Accounts.Last() });
            else _accounts[_clientA!].Add(_bank.Accounts.Last());
        }

        private void BankAccountOperationAssert3(BankAccount? clientAAccount)
        {
            _bank!.FoldTransaction(clientAAccount!.AccountHistory.Last());
        }

        private void BankAccountOperationAssert2(IClient clientB, BankAccount? clientAAccount, BankAccount? clientBAccount)
        {
            clientB.Address = "Lomonosova 9";
            clientB.ID = new RussianClientID(334113);
            clientB.IsSuspicious = false;

            _bank.MakeTransaction(new MakeTransaction(
                new List<OperationOrder> { new OperationOrder(clientAAccount!, clientBAccount!) }, 400));
        }

        private void BankAccountOperationAssert1(out IClient clientB, out BankAccount? clientAAccount, out BankAccount? clientBAccount)
        {
            clientB = Client.Builder
                        .WithFirstName("Mellory")
                        .WithSecondName("Clinton")
                        .Build();
            var acc = new DebitAccount(_bank, clientB, 0, false, null);

            _bank!.AddBankAccount(acc);

            if (_accounts.ContainsKey(clientB!) is false)
                _accounts.Add(clientB!, new List<BankAccountInfo> { _bank.Accounts.Last() });
            else _accounts[clientB!].Add(_bank.Accounts.Last());

            clientAAccount = _accounts.Values.SelectMany(i => i).First().Account;
            clientBAccount = _accounts.Values.SelectMany(i => i).Last().Account;
        }

        private void InterestRateAndChargesAssert(out BankAccount? creditAccount, out BankAccount? debitAccount)
        {
            var acc = new CreditAccount(_bank, _clientA!, 50_000m, null);
            _bank!.AddBankAccount(acc);

            if (_accounts.ContainsKey(_clientA!) is false)
                _accounts.Add(_clientA!, new List<BankAccountInfo> { _bank.Accounts.Last() });
            else _accounts[_clientA!].Add(_bank.Accounts.Last());

            creditAccount = acc;
            debitAccount = _accounts.Values.SelectMany(i => i).ToList()[0].Account;

            if (creditAccount is null || debitAccount is null)
                throw new Exception();

            _bank.MakeTransaction(new Withdraw(
                 creditAccount, 100_000));

            Thread.Sleep(11_000);
        }
    }
}