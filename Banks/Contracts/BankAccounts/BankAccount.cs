using Banks.Domain.Contracts.Banks;
using Banks.Domain.Contracts.Clients;
using Banks.Domain.Contracts.Receipts;

namespace Banks.Domain.Contracts.BankAccountConfigurations
{
    public class BankAccount
    {
        private readonly List<IReceipt> _receipts;

        public BankAccount(
            Bank bank,
            IClient client,
            decimal firstDeposit,
            bool isOverdraftEnabled = false,
            DateTime? expirationDate = null)
        {
            ArgumentNullException.ThrowIfNull(bank);
            ArgumentNullException.ThrowIfNull(client);

            Bank = bank;
            Client = client;
            Money = firstDeposit;
            ExpirationDate = expirationDate;
            IsOverdraftEnabled = isOverdraftEnabled;
            ExpirationDate = expirationDate;

            _receipts = new List<IReceipt>();
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
        public Bank Bank { get; }
        public IClient Client { get; }
        public decimal Money { get; set; }
        public bool IsSuspicious => Client.IsSuspicious;

        public bool IsOverdraftEnabled { get; }
        public DateTime? ExpirationDate { get; }
        public virtual decimal InterestRate
            => Bank.BankConfiguration.CommonInterestRate;

        public virtual decimal ChargeRate
            => Bank.BankConfiguration.CommonChargeRate;

        public IEnumerable<IReceipt> AccountHistory => _receipts;

        public bool Equals(BankAccount? account)
            => account is not null && Id.Equals(account.Id);

        public IReceipt AddToHistory(IReceipt receipt)
        {
            ArgumentNullException.ThrowIfNull(receipt);

            _receipts.Add(receipt);

            return receipt;
        }
    }
}