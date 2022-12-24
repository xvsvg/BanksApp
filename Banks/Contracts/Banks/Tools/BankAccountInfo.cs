using Banks.Domain.Contracts.BankAccountConfigurations;

namespace Banks.Domain.Contracts.Banks.Tools
{
    public class BankAccountInfo
    {
        public BankAccountInfo(PercentsAndFees percentsAndFees, BankAccount account)
        {
            ArgumentNullException.ThrowIfNull(account);
            ArgumentNullException.ThrowIfNull(percentsAndFees);

            PercentsAndFees = percentsAndFees;
            Account = account;
        }

        public PercentsAndFees PercentsAndFees { get; }
        public BankAccount Account { get; }
    }
}