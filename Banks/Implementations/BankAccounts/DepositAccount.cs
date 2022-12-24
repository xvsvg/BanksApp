using Banks.Domain.Contracts.BankAccountConfigurations;
using Banks.Domain.Contracts.Banks;
using Banks.Domain.Contracts.Clients;

namespace Banks.Domain.Implementations.BankAccounts
{
    public class DepositAccount : BankAccount
    {
        public DepositAccount(
            Bank bank,
            IClient client,
            decimal firstDeposit,
            bool isOverdraftEnabled,
            DateTime expirationDate)
            : base(bank, client, firstDeposit, isOverdraftEnabled, expirationDate) { }

        public override decimal InterestRate => EvaluateInterestRate();

        private decimal EvaluateInterestRate()
        {
            var depositAmounts = Bank.BankConfiguration.Configuration.Deposits.ToList();
            var interestRates = Bank.BankConfiguration.Configuration.Interests.ToList();

            for (int i = 1; i < depositAmounts.Count; ++i)
            {
                if (depositAmounts[i - 1] < Money && Money < depositAmounts[i])
                    return interestRates[i];
            }

            if (Money < depositAmounts.First())
                return interestRates.First();
            else if (depositAmounts.Last() < Money)
                return interestRates.Last();

            throw new NotSupportedException("Invalid deposit account configuration.");
        }
    }
}