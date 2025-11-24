// src/Domain/Interfaces/IProductRepository.cs
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PriceRunner.Domain;
using PriceRunner.Domain.Value;

namespace PriceRunner.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id, 
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Product>> GetAllAsync( 
            CancellationToken cancellationToken = default);
        Task AddAsync(
            Product product,
            CancellationToken cancellationToken = default
        );
        Task UpdateAsync(
            Product product,
            CancellationToken cancellationToken = default
        );
        Task DeleteAsync(
            Product product,
            CancellationToken cancellationToken = default
        );
        Task<bool> ExistsAsync(
            int productId,
            CancellationToken cancellationToken = default
        );
    }
}