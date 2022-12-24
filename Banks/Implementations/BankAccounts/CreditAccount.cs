using Banks.Domain.Contracts.BankAccountConfigurations;
using Banks.Domain.Contracts.Banks;
using Banks.Domain.Contracts.Clients;

namespace Banks.Domain.Implementations.BankAccounts
{
    public class CreditAccount : BankAccount
    {
        public CreditAccount(Bank bank, IClient client, decimal loan, DateTime? expirationDate)
            : base(bank, client, loan, isOverdraftEnabled: true, expirationDate) { }

        public decimal Loan => Money;
    }
}