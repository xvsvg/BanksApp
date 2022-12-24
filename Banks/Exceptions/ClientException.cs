namespace Banks.Domain.Exceptions
{
    public class ClientException : BanksExceptions
    {
        private ClientException()
            : base() { }

        private ClientException(string message)
            : base(message) { }

        public static ClientException InvalidClientInitials()
            => new ClientException();

        public static ClientException InvalidClientAddress()
            => new ClientException();

        internal static ClientException CannotChangeStatusWithoutFullProfileData()
            => new ClientException();
    }
}