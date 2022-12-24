using Banks.Domain.Implementations.AccountOperations;

namespace Banks.Domain.Contracts.BankAccountOperations
{
    public interface IAccountOperation
    {
        public decimal Total { get; }
        public IEnumerable<OperationOrder> Orders { get; }
        public void Evaluate();
        public void Reset();
    }
}