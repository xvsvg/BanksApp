using Banks.Domain.Implementations.ClientID.Clients;

namespace Banks.Domain.Contracts.Clients.ClientBuilderInterfaces
{
    public interface IFirstNameBuilder
    {
        public ISecondNameBuilder WithFirstName(string firstName);
    }

    public interface ISecondNameBuilder
    {
        public Client.ClientBuilder WithSecondName(string secondName);
    }
}