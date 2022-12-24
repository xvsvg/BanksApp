using Banks.Domain.Contracts.BankAccountOperations;
using Banks.Domain.Exceptions;

namespace Banks.Domain.Implementations.AccountOperations
{
    public class MakeTransaction : IAccountOperation
    {
        public MakeTransaction(IEnumerable<OperationOrder> orders, decimal total)
        {
            ArgumentNullException.ThrowIfNull(orders);

            Orders = orders;
            Total = total;
        }

        public IEnumerable<OperationOrder> Orders { get; }
        public decimal Total { get; }

        public void Evaluate()
        {
            foreach (OperationOrder account in Orders)
            {
                if (account.From.IsSuspicious is true || account.To.IsSuspicious is true)
                    throw AccountOperationException.AccountIsSuspiciousException("Provide all profile information to use service");

                account.From.Money -= Total;
                account.To.Money += Total;
            }
        }

        public void Reset()
        {
            foreach (OperationOrder account in Orders)
            {
                if (account.From.IsSuspicious is true || account.To.IsSuspicious is true)
                    throw AccountOperationException.AccountIsSuspiciousException("Provide all profile information to use service");

                account.From.Money += Total;
                account.To.Money -= Total;
            }
        }
    }
}