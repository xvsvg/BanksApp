namespace Banks.Domain.Contracts.Clocks
{
    public interface IClock
    {
        public delegate void Alarm();
        public event Alarm? OnMonthlyAlarm;
        public event Alarm? OnDailyAlarm;
        public Task StartClockAsync();
    }
}