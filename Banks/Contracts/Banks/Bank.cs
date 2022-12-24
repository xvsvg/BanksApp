using Banks.Domain.Contracts.BankAccountConfigurations;
using Banks.Domain.Contracts.BankAccountOperations;
using Banks.Domain.Contracts.Banks.Tools;
using Banks.Domain.Contracts.ClientID;
using Banks.Domain.Contracts.Clients;
using Banks.Domain.Contracts.Receipts;
using Banks.Domain.Exceptions;
using Banks.Domain.Extensions;
using Banks.Domain.Implementations.AccountOperations;
using Banks.Domain.Implementations.BankAccounts;
using Banks.Domain.Implementations.Receipts;

namespace Banks.Domain.Contracts.Banks
{
    public class Bank
    {
        private readonly Dictionary<IClient, List<BankAccountInfo>> _accounts;

        public Bank(CentralBank centralBank, BankConfiguration bankConfiguration)
        {
            ArgumentNullException.ThrowIfNull(bankConfiguration);
            ArgumentNullException.ThrowIfNull(centralBank);

            BankConfiguration = bankConfiguration;
            CentralBank = centralBank;

            Id = Guid.NewGuid();
            _accounts = new Dictionary<IClient, List<BankAccountInfo>>();

            centralBank.NotifyAboutDateChange += CalculateInterest;
            centralBank.NotifyAboutDateChange += CalculateFee;
            centralBank.NotifyAboutMonthChange += ChargeFee;
            centralBank.NotifyAboutMonthChange += AccrueInterest;
        }

        public Guid Id { get; }
        public BankConfiguration BankConfiguration { get; }
        public CentralBank CentralBank { get; }
        public IEnumerable<IClient> Clients => _accounts.Keys;
        public IEnumerable<BankAccountInfo> Accounts => _accounts.Values.SelectMany(i => i);

        public override string ToString()
            => BankConfiguration.Name;

        /// <summary>
        /// Adds provided bank account to the bunch
        /// of accounts in the bank.
        /// </summary>
        /// <param name="bankAccount">Bank account, you want to add ( register ).</param>
        /// <returns>Provided bank account.</returns>
        /// <exception cref="ArgumentNullException">When account reference is null.</exception>
        public BankAccount AddBankAccount(BankAccount bankAccount)
        {
            ArgumentNullException.ThrowIfNull(bankAccount);

            if (_accounts.ContainsKey(bankAccount.Client))
            {
                _accounts[bankAccount.Client].Add(new BankAccountInfo(new PercentsAndFees(0, 0), bankAccount));
            }
            else
            {
                _accounts.Add(bankAccount.Client, new List<BankAccountInfo>
        {
            new BankAccountInfo(new PercentsAndFees(0, 0), bankAccount),
        });
            }

            return bankAccount;
        }

        /// <summary>
        /// Removes bank account from bank.
        /// </summary>
        /// <param name="bankAccount">Bank account, you want to remove.</param>
        /// <exception cref="ArgumentNullException">When account reference is null.</exception>
        public void RemoveBankAccount(BankAccount bankAccount)
        {
            ArgumentNullException.ThrowIfNull(bankAccount);

            int index = _accounts[bankAccount.Client].FindIndex(acc => acc.Account.Equals(bankAccount));

            _accounts[bankAccount.Client].RemoveAt(index);
        }

        /// <summary>
        /// Finds all accounts, that belongs to a person with provided ID.
        /// </summary>
        /// <param name="id">Some person's ID.</param>
        /// <returns>Collection of person's bank accounts.</returns>
        public IReadOnlyList<BankAccountInfo> FindBankAccount(IClientID id)
        {
            return _accounts
                .Where(a => a.Key.ID is not null && a.Key.ID.IdNumber == id.IdNumber)
                .SelectMany(a => a.Value)
                .ToList();
        }

        /// <summary>
        /// Performs account operation, e.g Withdraw, MakeDeposit etc.
        /// </summary>
        /// <param name="operation">Operation type, you want to perform.</param>
        /// <exception cref="ArgumentNullException">When operation reference is null.</exception>
        public void MakeTransaction(IAccountOperation operation)
        {
            ArgumentNullException.ThrowIfNull(operation);

            operation.Evaluate();

            foreach (OperationOrder order in operation.Orders)
            {
                UpdateAccountHistory(order.From, operation);

                if (order.From.Equals(order.To) is false)
                    UpdateAccountHistory(order.To, operation);
            }
        }

        /// <summary>
        /// Performs account operation cancellation.
        /// </summary>
        /// <param name="receipt">Bill for the operation, you want to cancell.</param>
        /// <exception cref="ArgumentNullException">When receipt reference is null.</exception>
        /// <exception cref="ReceiptException">When operation has been already cancelled, but wanted to be
        /// cancelled again.</exception>
        public void FoldTransaction(IReceipt receipt)
        {
            ArgumentNullException.ThrowIfNull(receipt);

            if (receipt.IsCancelled)
                throw ReceiptException.UnableToFoldTransaction();

            receipt.Operation.Reset();
            receipt.IsCancelled = true;
        }

        private void UpdateAccountHistory(BankAccount account, IAccountOperation operation)
        {
            account.AddToHistory(new AccountOperationReceipt(
                    operation.Total,
                    $"Operation was made with bank {this} on {DateTime.Now}.",
                    operation,
                    operation.Orders));
        }

        private void CalculateInterest()
        {
            _accounts.Values.SelectMany(i => i)
                .Where(acc => acc.Account is not CreditAccount)
                .ForEach(
                    acc =>
                    {
                        acc.PercentsAndFees.Percent += Math.Round((decimal)(acc.Account.InterestRate / 365 / 100) * acc.Account.Money, 2);
                        acc.PercentsAndFees.Fee += Math.Round((decimal)(acc.Account.InterestRate / 365 / 100) * acc.Account.Money, 2);
                    });
        }

        private void CalculateFee()
        {
            _accounts.Values.SelectMany(i => i)
                .Where(acc => acc.Account is CreditAccount && acc.Account.Money < 0)
                .ForEach(acc => acc.PercentsAndFees.Fee +=
                Math.Round((decimal)(acc.Account.ChargeRate / 365 / 100) * Math.Abs(acc.Account.Money), 2));
        }

        private void AccrueInterest()
        {
            _accounts.Values.SelectMany(i => i)
                .Where(acc => acc.Account is not CreditAccount)
                .ForEach(
                    acc =>
                    {
                        acc.Account.Money += acc.PercentsAndFees.Percent;
                        acc.PercentsAndFees.Percent = 0;
                    });
        }

        private void ChargeFee()
        {
            _accounts.Values.SelectMany(i => i)
                .Where(acc => acc.Account is CreditAccount)
                .ForEach(
                    acc =>
                    {
                        acc.Account.Money -= acc.PercentsAndFees.Fee;
                        acc.PercentsAndFees.Fee = 0;
                    });
        }
    }
}