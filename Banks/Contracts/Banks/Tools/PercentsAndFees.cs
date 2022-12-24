using Banks.Domain.Exceptions;

namespace Banks.Domain.Contracts.Banks.Tools
{
    public class PercentsAndFees
    {
        private decimal _fee;
        private decimal _percent;

        public PercentsAndFees(decimal percent, decimal fee)
        {
            _percent = percent;
            _fee = fee;
        }

        public decimal Percent
        {
            get => _percent;
            set
            {
                if (value < 0)
                    throw BankAccountException.NegativeRateAmountException("percent cannot be negative");
                _percent = value;
            }
        }

        public decimal Fee
        {
            get => _fee;
            set
            {
                if (value < 0)
                    throw BankAccountException.NegativeChargesAmountException("fees cannot be negative");
                _fee = value;
            }
        }
    }
}