// src/Domain/Value/ProductId.cs
using System;

namespace PriceRunner.Domain.Value
{
    public readonly record struct ProductId
    {
        public Id Value { get;}
        public ProductId(Id value)
        {
            if (value == Id.Empty) 
                throw new ArgumentException("Value can not be Empty.", nameof(value));
            Value = value;
        }
        public static ProductId New() => new(Id.NewId());
        public override string Tostring() => Value.ToString();
        public static implicit operator Id(ProductId id) => id.Value;
        public static implicit operator ProductId(Id value) => new(value);
    }
}