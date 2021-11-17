using System;

namespace QrPayment.Model
{
    public class MoneyAmount : IEquatable<MoneyAmount>
    {
        public decimal Amount { get; }
        
        public Currency Currency { get; }

        public MoneyAmount(decimal amount, Currency currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public bool Equals(MoneyAmount? other)
        {
            if (other is null)
                return false;

            return other.Amount == Amount && other.Currency == Currency;
        }

        public override bool Equals(object? obj)
        {
            if (obj is MoneyAmount other)
                return Equals(other);
            
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, (int)Currency);
        }

        public static bool operator ==(MoneyAmount amount1, MoneyAmount amount2)
        {
            return amount1.Equals(amount2);
        }

        public static bool operator !=(MoneyAmount amount1, MoneyAmount amount2)
        {
            return ! amount1.Equals(amount2);
        }
    }
}