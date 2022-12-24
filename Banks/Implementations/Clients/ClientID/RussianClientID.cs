using Banks.Domain.Contracts.ClientID;
using Banks.Domain.Exceptions;

namespace Banks.Domain.Implementations.ClientID
{
    public class RussianClientID : IClientID
    {
        public RussianClientID(int idNumber)
        {
            IdNumber = Validate(idNumber);
        }

        public int IdNumber { get; }

        public bool Equals(RussianClientID other)
            => other.IdNumber == IdNumber;

        private int Validate(int number)
        {
            if (Convert.ToString(number).Length is < 0 or > 6)
                throw ClientIDException.InvalidIDNumber();

            return number;
        }
    }
}