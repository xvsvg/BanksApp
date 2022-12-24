using Banks.Domain.Contracts.ClientID;
using Banks.Domain.Contracts.Clients;
using Banks.Domain.Contracts.Clients.ClientBuilderInterfaces;
using Banks.Domain.Exceptions;

namespace Banks.Domain.Implementations.ClientID.Clients
{
    public class Client : IClient
    {
        private string? _address;
        private IClientID? _id;
        private bool _isSuspicious;

        private Client(string firstName, string secondName, string? address, IClientID? id)
        {
            FirstName = firstName;
            SecondName = secondName;
            _address = address;
            _id = id;

            if (address is null || id is null)
                _isSuspicious = true;
        }

        public event IClient.StatusHandler? OnStatusChange;

        public static IFirstNameBuilder Builder => new ClientBuilder();

        public string FirstName { get; }
        public string SecondName { get; }
        public string? Address
        {
            get => _address;
            set
            {
                if (string.IsNullOrEmpty(value) is true)
                    throw ClientException.InvalidClientAddress();

                _address = value;
            }
        }

        public IClientID? ID
        {
            get => _id;
            set
            {
                ArgumentNullException.ThrowIfNull(value);

                _id = value;
            }
        }

        public bool IsSuspicious
        {
            get => _isSuspicious;
            set
            {
                if ((_id is null || _address is null) && _isSuspicious is false)
                    throw ClientException.CannotChangeStatusWithoutFullProfileData();

                _isSuspicious = value;
                OnStatusChange?.Invoke();
            }
        }

        public sealed class ClientBuilder : IFirstNameBuilder, ISecondNameBuilder
        {
            private string? _firstName = null;
            private string? _secondName = null;
            private string? _address = null;
            private IClientID? _id = null;

            public ISecondNameBuilder WithFirstName(string firstName)
            {
                if (firstName == string.Empty)
                    throw ClientException.InvalidClientInitials();

                _firstName = firstName;
                return this;
            }

            public ClientBuilder WithSecondName(string secondName)
            {
                if (secondName == string.Empty)
                    throw ClientException.InvalidClientInitials();

                _secondName = secondName;
                return this;
            }

            public ClientBuilder WithAddress(string address)
            {
                if (address == string.Empty)
                    throw ClientException.InvalidClientAddress();

                _address = address;
                return this;
            }

            public ClientBuilder WithId(IClientID id)
            {
                _id = id;
                return this;
            }

            public IClient Build()
            {
                return new Client(
                    _firstName ?? throw new ArgumentNullException(),
                    _secondName ?? throw new ArgumentNullException(),
                    _address,
                    _id);
            }
        }
    }
}