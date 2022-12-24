using Banks.Domain.Contracts.BankAccountConfigurations;

namespace Banks.Domain.Implementations.AccountOperations
{
    public class OperationOrder
    {
        public OperationOrder(BankAccount from, BankAccount to)
        {
            ArgumentNullException.ThrowIfNull(from);
            ArgumentNullException.ThrowIfNull(to);

            From = from;
            To = to;
        }

        public BankAccount From { get; }
        public BankAccount To { get; }
    }
}