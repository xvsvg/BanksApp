using Banks.Domain.Contracts.BankAccountOperations;
using Banks.Domain.Contracts.Receipts;
using Banks.Domain.Exceptions;
using Banks.Domain.Implementations.AccountOperations;

namespace Banks.Domain.Implementations.Receipts
{
    public class AccountOperationReceipt : IReceipt
    {
        private decimal _total;
        private bool _isCancelled;

        public AccountOperationReceipt(
            decimal total,
            string details,
            IAccountOperation operation,
            IEnumerable<OperationOrder> orders)
        {
            ArgumentNullException.ThrowIfNull(details);
            ArgumentNullException.ThrowIfNull(orders);
            ArgumentNullException.ThrowIfNull(operation);

            if (total <= 0) throw ReceiptException.NegativeMoneyAmountException();

            _total = total;
            Details = details;
            Orders = orders;
            Operation = operation;
            _isCancelled = false;
        }

        public decimal Total
        {
            get => _total;
            set
            {
                if (value <= 0)
                    throw ReceiptException.NegativeMoneyAmountException();
                _total = value;
            }
        }

        public string Details { get; }
        public bool IsCancelled
        {
            get => _isCancelled;
            set
            {
                if (_isCancelled is true && value is false)
                    throw ReceiptException.UnableToChangeStatusException();
            }
        }

        public IAccountOperation Operation { get; }

        public IEnumerable<OperationOrder> Orders { get; }
    }
}