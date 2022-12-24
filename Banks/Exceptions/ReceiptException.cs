namespace Banks.Domain.Exceptions
{
    public class ReceiptException : BanksExceptions
    {
        private ReceiptException()
            : base() { }

        private ReceiptException(string message)
            : base(message) { }

        public static ReceiptException UnableToChangeStatusException()
            => new ReceiptException();

        public static ReceiptException NegativeMoneyAmountException()
            => new ReceiptException();

        public static ReceiptException UnableToFoldTransaction()
            => new ReceiptException();
    }
}