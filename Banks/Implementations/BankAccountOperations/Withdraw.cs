using Banks.Domain.Contracts.BankAccountConfigurations;
using Banks.Domain.Contracts.BankAccountOperations;

namespace Banks.Domain.Implementations.AccountOperations
{
    public class Withdraw : IAccountOperation
    {
        public Withdraw(BankAccount account, decimal total)
        {
            ArgumentNullException.ThrowIfNull(account);

            Account = account;
            Total = total;
        }

        public BankAccount Account { get; }
        public decimal Total { get; }
        public IEnumerable<OperationOrder> Orders
            => new List<OperationOrder> { new OperationOrder(Account, Account) };

        public void Evaluate()
        {
            Account.Money -= Total;
        }

        public void Reset()
        {
            Account.Money += Total;
        }
    }
}