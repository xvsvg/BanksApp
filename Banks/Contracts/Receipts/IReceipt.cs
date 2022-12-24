using Banks.Domain.Contracts.BankAccountOperations;
using Banks.Domain.Implementations.AccountOperations;

namespace Banks.Domain.Contracts.Receipts
{
    public interface IReceipt
    {
        public decimal Total { get; }
        public string Details { get; }
        public bool IsCancelled { get; set; }
        public IAccountOperation Operation { get; }
        public IEnumerable<OperationOrder> Orders { get; }
    }
}