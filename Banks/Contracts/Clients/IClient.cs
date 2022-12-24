using Banks.Domain.Contracts.ClientID;

namespace Banks.Domain.Contracts.Clients
{
    public interface IClient
    {
        public delegate void StatusHandler();
        public event StatusHandler? OnStatusChange;

        public string FirstName { get; }
        public string SecondName { get; }
        public string? Address { get; set; }
        public IClientID? ID { get; set; }
        public bool IsSuspicious { get; set; }
    }
}