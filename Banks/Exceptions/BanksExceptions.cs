namespace Banks.Domain.Exceptions
{
    public class BanksExceptions : Exception
    {
        public BanksExceptions()
            : base() { }

        public BanksExceptions(string message)
            : base(message) { }

        public BanksExceptions(string message, Exception innerException)
            : base(message, innerException) { }
    }
}