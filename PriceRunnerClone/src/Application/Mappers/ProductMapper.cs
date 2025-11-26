using System.Collections.Generic;
using System.Linq;
using PriceRunner.Application.DTOs;

namespace PriceRunner.Application.Mappers
{
    /// <summary>
    /// Mapping helpers for product-related DTOs.
    /// Keeps mapping logic in the Application layer.
    /// </summary>
    public static class ProductMappers
    {
        /// <summary>
        /// Attach price- and history-data to a base ProductDto.
        /// </summary>
        public static ProductDto WithPricesAndHistory(
            this ProductDto product,
            IEnumerable<ProductPriceDto>? prices,
            IEnumerable<ProductHistoryDto>? history)
        {
            product.Prices = prices?.ToList() ?? new List<ProductPriceDto>();
            product.History = history?.ToList() ?? new List<ProductHistoryDto>();
            return product;
        }
    }
}
