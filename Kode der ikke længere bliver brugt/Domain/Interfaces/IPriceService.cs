// src/Domain/Interfaces/IPriceService.cs
using System.Collections.Generic;
using PriceRunner.Domain;
using PriceRunner.Domain.Value;

namespace PriceRunner.Domain.Interfaces
{
    public interface IPriceService
    {
        Money? GetLowestPrice(IEnumerable<Price> prices);
        Price? GetLowestPriceEntry(IEnumerable<Price> prices);
    }
}
