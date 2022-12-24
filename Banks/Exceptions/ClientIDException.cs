namespace Banks.Domain.Exceptions
{
    public class ClientIDException : BanksExceptions
    {
        private ClientIDException()
            : base() { }

        private ClientIDException(string message)
            : base(message) { }

        public static ClientIDException InvalidIDNumber()
            => new ClientIDException();
    }
}