using Banks.Domain.Contracts.BankAccountConfigurations;
using Banks.Domain.Contracts.Banks;
using Banks.Domain.Contracts.Clients;

namespace Banks.Domain.Implementations.BankAccounts
{
    public class DebitAccount : BankAccount
    {
        public DebitAccount(
            Bank bank,
            IClient client,
            decimal firstDeposit,
            bool isOverdraftEnabled,
            DateTime? expirationDate)
            : base(bank, client, firstDeposit, isOverdraftEnabled, expirationDate) { }
    }
}