namespace Banks.Domain.Exceptions
{
    public class AccountOperationException : BanksExceptions
    {
        private AccountOperationException()
            : base() { }

        private AccountOperationException(string message)
            : base(message) { }

        public static AccountOperationException AccountIsSuspiciousException(string details)
            => new AccountOperationException(details);
    }
}