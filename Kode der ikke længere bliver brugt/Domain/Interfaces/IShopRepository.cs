// src/Domain/Interfaces/IShopRepository.cs
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PriceRunner.Domain;

namespace PriceRunner.Domain.Interfaces
{
    public interface IShopRepository
    {
        Task<Shop?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Shop>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Shop shop,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
            Shop shop,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            Shop shop,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
