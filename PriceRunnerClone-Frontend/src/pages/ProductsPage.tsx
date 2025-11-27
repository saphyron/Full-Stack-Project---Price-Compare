import React, { useMemo, useState } from 'react';
import { useProducts } from '../hooks/useProducts';
import { ProductCard } from '../components/ProductCard';

export const ProductsPage: React.FC = () => {
  const { products, isLoading, error } = useProducts();
  const [query, setQuery] = useState('');

  const filtered = useMemo(() => {
    const q = query.trim().toLowerCase();
    if (!q) return products;
    return products.filter((p) => p.name.toLowerCase().includes(q));
  }, [products, query]);

  return (
    <div className="space-y-4">
      <header className="flex flex-col gap-3 sm:flex-row sm:items-end sm:justify-between">
        <div>
          <h1 className="text-xl font-semibold tracking-tight">Products</h1>
          <p className="text-xs text-slate-400">
            Data fra{' '}
            <code className="text-[11px] text-sky-300">
              /api/products/with-brand-category
            </code>
          </p>
        </div>

        <div className="w-full max-w-xs">
          <label className="block text-[11px] font-medium uppercase tracking-wide text-slate-400">
            Search
          </label>
          <input
            type="search"
            placeholder="Search by name…"
            className="mt-1 w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-1.5 text-sm text-slate-100 outline-none ring-sky-500/40 focus:ring"
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
        </div>
      </header>

      {isLoading && <p className="text-sm text-slate-400">Loading products…</p>}
      {error && <p className="text-sm text-red-400">{error}</p>}

      {!isLoading && !error && filtered.length === 0 && (
        <p className="text-sm text-slate-400">No products found.</p>
      )}

      <section className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
        {filtered.map((product) => (
          <ProductCard key={product.id} product={product} />
        ))}
      </section>
    </div>
  );
};
