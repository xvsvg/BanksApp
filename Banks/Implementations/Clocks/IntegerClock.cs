using Banks.Domain.Contracts.Clocks;

namespace Banks.Domain.Implementations.Clocks
{
    public class IntegerClock : IClock
    {
        private int _timeOffset;
        private int _timeToWait;
        private int _notifyFrequency;

        public IntegerClock(int timeBetweenTicksInMilliseconds, int notifyFrequency)
        {
            _timeToWait = timeBetweenTicksInMilliseconds;
            _notifyFrequency = notifyFrequency;
            _timeOffset = default;
        }

        public event IClock.Alarm? OnMonthlyAlarm;
        public event IClock.Alarm? OnDailyAlarm;

        public async Task StartClockAsync()
        {
            await Task.Run(() => Start());
        }

        private void Start()
        {
            while (true)
            {
                ++_timeOffset;
                OnDailyAlarm?.Invoke();
                Thread.Sleep(_timeToWait);

                if (_timeOffset % _notifyFrequency == 0)
                    OnMonthlyAlarm?.Invoke();
            }
        }
    }
}