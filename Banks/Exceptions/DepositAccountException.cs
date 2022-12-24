namespace Banks.Domain.Exceptions
{
    public class DepositAccountException : BanksExceptions
    {
        private DepositAccountException()
            : base() { }

        private DepositAccountException(string message)
                : base(message) { }

        public static DepositAccountException InvalidOperationException(string details)
            => new DepositAccountException(details);
    }
}