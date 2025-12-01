// src/Domain/Value/Money.cs
using System;
using System.Globalization;

namespace Pricerunner.Domain.Value
{
    public readonly record struct Money
    {
        public double Amount { get;}
        public string Currency { get;}

        public Money (double amount, string currency)
        {
            if(string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Current is required.", nameof(currency));
            Amount = amount;
            Currency = currency;
        }

        public static Money Zero(string currency) => new(0.00, currency);
        public bool IsZero => Amount == 0.00;

        public override string ToString()
        {
            var amountString = Amount.ToString("0.00", CultureInfo.InvariantCulture);
            return $"{amountString} {currency}";
        }

        public static Money operator +(Money left, Money right)
        {
            EnsureSameCurrency(left, right);
            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            EnsureSameCurrency(left, right);
            return new Money(left.Amount - right.Amount, left.Currency);
        }

        public static bool operator >(Money left, Money right) {
            EnsureSameCurrency(left, right);
            return left.Amount > right.Amount;
        }

        public static bool operator <(Money left, Money right)
        {
            EnsureSameCurrency(left, right);
            return left.Amount < right.Amount;
        }

        public static bool operator >=(Money left, Money right)
        {
            EnsureSameCurrency(left, right);
            return left.Amount >= right.Amount;
        }

        public static bool operator <=(Money left, Money right)
        {
            EnsureSameCurrency(left, right);
            return left.Amount <= right.Amount;
        }

        private static void EnsureSameCurrency(Money left, Money Right)
        {
            if (!string.equals(left.Currency, Right.Currency, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Money values must have the same currency.");
        }
    }
}