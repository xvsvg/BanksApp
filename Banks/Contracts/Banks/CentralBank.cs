using Banks.Domain.Contracts.Clocks;

namespace Banks.Domain.Contracts.Banks
{
    public class CentralBank
    {
        private readonly List<Bank> _banks;
        private readonly IClock _localTime;

        public CentralBank(IClock localTime, decimal centralBankMoney)
        {
            ArgumentNullException.ThrowIfNull(localTime);

            _localTime = localTime;
            Money = centralBankMoney;

            _banks = new List<Bank>();

            _localTime.OnMonthlyAlarm += MonthlyNotify;
            _localTime.OnDailyAlarm += DailtyNotify;
        }

        public delegate void MonthlyOperationHandler();
        public delegate void DailyOperationHandler();

        public event MonthlyOperationHandler? NotifyAboutMonthChange;
        public event DailyOperationHandler? NotifyAboutDateChange;

        public decimal Money { get; }
        public IEnumerable<Bank> Banks => _banks;

        public Bank AddBank(Bank bank)
        {
            ArgumentNullException.ThrowIfNull(bank);

            _banks.Add(bank);

            return bank;
        }

        public void RemoveBank(Bank bank)
        {
            ArgumentNullException.ThrowIfNull(bank);

            _banks.Remove(bank);
        }

        private void DailtyNotify()
        {
            NotifyAboutDateChange?.Invoke();
        }

        private void MonthlyNotify()
        {
            NotifyAboutMonthChange?.Invoke();
        }
    }
}