namespace Banks.Domain.Exceptions
{
    public class BankAccountException : BanksExceptions
    {
        private BankAccountException()
            : base() { }

        private BankAccountException(string message)
            : base(message) { }

        private BankAccountException(string message, Exception innerException)
            : base(message, innerException) { }

        public static BankAccountException NegativeMoneyAmountException(string message)
            => new BankAccountException(message);

        public static BankAccountException NegativeRateAmountException(string message)
            => new BankAccountException(message);

        public static BankAccountException NegativeChargesAmountException(string message)
            => new BankAccountException(message);

        public static BankAccountException NegativeLoanAmountException(string message)
            => new BankAccountException(message);
    }
}