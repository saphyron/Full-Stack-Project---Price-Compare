import React from 'react';
import { useProducts } from '../hooks/useProducts';

export const AdminProductPage: React.FC = () => {
  const { products, isLoading, error } = useProducts();

  return (
    <div className="space-y-4">
      <header>
        <h1 className="text-xl font-semibold tracking-tight">
          Admin – Products
        </h1>
        <p className="text-xs text-slate-400">
          Kun synlig for brugere med rollen <code>Admin</code>. Viser
          alle produkter med shop, brand og category.
        </p>
      </header>

      {isLoading && (
        <p className="text-sm text-slate-400">Loading products…</p>
      )}

      {error && <p className="text-sm text-red-400">{error}</p>}

      {!isLoading && !error && products.length === 0 && (
        <p className="text-sm text-slate-400">No products found.</p>
      )}

      {!isLoading && !error && products.length > 0 && (
        <div className="overflow-x-auto rounded-lg border border-slate-800">
          <table className="min-w-full text-left text-xs">
            <thead className="bg-slate-900">
              <tr>
                <th className="px-3 py-2 font-semibold text-slate-300">
                  ID
                </th>
                <th className="px-3 py-2 font-semibold text-slate-300">
                  Name
                </th>
                <th className="px-3 py-2 font-semibold text-slate-300">
                  Shop
                </th>
                <th className="px-3 py-2 font-semibold text-slate-300">
                  Brand
                </th>
                <th className="px-3 py-2 font-semibold text-slate-300">
                  Category
                </th>
                <th className="px-3 py-2 font-semibold text-slate-300">
                  Product URL
                </th>
              </tr>
            </thead>
            <tbody>
              {products.map((p) => (
                <tr
                  key={p.id}
                  className="border-t border-slate-800"
                >
                  <td className="px-3 py-2 text-slate-300">{p.id}</td>
                  <td className="px-3 py-2 text-slate-100">{p.name}</td>
                  <td className="px-3 py-2 text-slate-200">
                    {p.shopName ?? '—'}
                  </td>
                  <td className="px-3 py-2 text-slate-200">
                    {p.brandName ?? '—'}
                  </td>
                  <td className="px-3 py-2 text-slate-200">
                    {p.categoryName ?? '—'}
                  </td>
                  <td className="px-3 py-2 text-sky-400">
                    {p.productUrl ? (
                      <a
                        href={p.productUrl}
                        target="_blank"
                        rel="noreferrer"
                        className="hover:text-sky-300"
                      >
                        Open
                      </a>
                    ) : (
                      <span className="text-slate-500">—</span>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};
