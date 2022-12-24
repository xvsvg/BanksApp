namespace Banks.Domain.Contracts.Banks.Tools
{
    public class DepositAccountConfiguration
    {
        private readonly List<decimal> _interests;
        private readonly List<decimal> _deposits;

        public DepositAccountConfiguration(IEnumerable<decimal> deposits, IEnumerable<decimal> interests)
        {
            _deposits = deposits.ToList();
            _interests = interests.ToList();
        }

        public IEnumerable<decimal> Interests => _interests;
        public IEnumerable<decimal> Deposits => _deposits;
    }
}