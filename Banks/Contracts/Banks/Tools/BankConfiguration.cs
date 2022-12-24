using Banks.Domain.Exceptions;

namespace Banks.Domain.Contracts.Banks.Tools;

public class BankConfiguration
{
    private decimal _interestRate;
    private decimal _chargeRate;

    public BankConfiguration(string name, decimal commonInterestRate, decimal commonChargeRate, DepositAccountConfiguration configuration)
    {
        if (string.IsNullOrEmpty(name) is true)
            throw new BanksExceptions("Invalid bank name");

        Name = name;
        CommonInterestRate = commonInterestRate;
        CommonChargeRate = commonChargeRate;
        Configuration = configuration;
    }

    public string Name { get; }
    public decimal CommonInterestRate
    {
        get => _interestRate;
        private set
        {
            if (value < 0)
                throw new BanksExceptions("Bank interest rate should be positive");

            _interestRate = value;
        }
    }

    public decimal CommonChargeRate
    {
        get => _chargeRate;
        private set
        {
            if (value < 0)
                throw new BanksExceptions("Bank charge rate should be positive");

            _chargeRate = value;
        }
    }

    public DepositAccountConfiguration Configuration { get; }
}