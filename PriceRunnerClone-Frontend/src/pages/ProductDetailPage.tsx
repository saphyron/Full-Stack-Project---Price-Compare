import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import * as productsApi from '../api/productsApi';
import { PriceTag } from '../components/PriceTag';

export const ProductDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [product, setProduct] = useState<productsApi.ProductDto | null>(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!id) return;
    let isMounted = true;

    const load = async () => {
      setIsLoading(true);
      setError(null);
      try {
        const data = await productsApi.getProduct(Number(id));
        if (isMounted) setProduct(data);
      } catch (err) {
        console.error(err);
        if (isMounted) setError('Failed to load product.');
      } finally {
        if (isMounted) setIsLoading(false);
      }
    };

    load();
    return () => {
      isMounted = false;
    };
  }, [id]);

  if (!id) {
    return <p className="text-sm text-red-400">Invalid product id.</p>;
  }

  if (isLoading) {
    return <p className="text-sm text-slate-400">Loading product…</p>;
  }

  if (error) {
    return <p className="text-sm text-red-400">{error}</p>;
  }

  if (!product) {
    return (
      <div className="space-y-3">
        <p className="text-sm text-slate-400">Product not found.</p>
        <Link
          to="/products"
          className="text-xs text-sky-400 hover:text-sky-300"
        >
          Back to products
        </Link>
      </div>
    );
  }

  const cheapest =
    product.prices && product.prices.length > 0
      ? [...product.prices].sort((a, b) => a.amount - b.amount)[0]
      : undefined;

  return (
    <div className="space-y-6">
      <div className="space-y-2">
        <Link
          to="/products"
          className="text-xs text-slate-400 hover:text-sky-300"
        >
          ← Back to products
        </Link>

        <h1 className="text-xl font-semibold tracking-tight">{product.name}</h1>

        <div className="flex flex-wrap gap-2 text-xs text-slate-400">
          {product.brandName && (
            <span className="rounded-full bg-slate-800 px-2 py-0.5">
              Brand:{' '}
              <span className="text-slate-100">{product.brandName}</span>
            </span>
          )}
          {product.categoryName && (
            <span className="rounded-full bg-slate-800 px-2 py-0.5">
              Category:{' '}
              <span className="text-slate-100">{product.categoryName}</span>
            </span>
          )}
          {product.shopName && (
            <span className="rounded-full bg-slate-800 px-2 py-0.5">
              Example shop:{' '}
              <span className="text-slate-100">{product.shopName}</span>
            </span>
          )}
        </div>

        {product.productUrl && (
          <a
            href={product.productUrl}
            target="_blank"
            rel="noreferrer"
            className="inline-flex items-center text-xs text-sky-400 hover:text-sky-300"
          >
            Open original product page
          </a>
        )}
      </div>

      <section className="space-y-3">
        <h2 className="text-sm font-semibold text-slate-100">
          Current prices
        </h2>
        {product.prices && product.prices.length > 0 ? (
          <div className="space-y-2">
            {cheapest && (
              <div className="mb-2">
                <PriceTag
                  amount={cheapest.amount}
                  currency={cheapest.currency}
                  amountLabel={`Cheapest at ${cheapest.shopName}`}
                />
              </div>
            )}
            <div className="overflow-x-auto rounded-lg border border-slate-800">
              <table className="min-w-full text-left text-xs">
                <thead className="bg-slate-900">
                  <tr>
                    <th className="px-3 py-2 font-semibold text-slate-300">
                      Shop
                    </th>
                    <th className="px-3 py-2 font-semibold text-slate-300">
                      Price
                    </th>
                    <th className="px-3 py-2 font-semibold text-slate-300">
                      Last updated
                    </th>
                  </tr>
                </thead>
                <tbody>
                  {product.prices.map((row) => (
                    <tr
                      key={row.id}
                      className="border-t border-slate-800"
                    >
                      <td className="px-3 py-2">{row.shopName}</td>
                      <td className="px-3 py-2">
                        {row.amount.toFixed(2)} {row.currency}
                      </td>
                      <td className="px-3 py-2 text-slate-400">
                        {row.lastUpdatedUtc
                          ? new Date(row.lastUpdatedUtc).toLocaleString()
                          : '—'}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        ) : (
          <p className="text-xs text-slate-400">
            No current prices found for this product.
          </p>
        )}
      </section>

      <section className="space-y-3">
        <h2 className="text-sm font-semibold text-slate-100">
          Price history
        </h2>
        {product.history && product.history.length > 0 ? (
          <div className="overflow-x-auto rounded-lg border border-slate-800">
            <table className="min-w-full text-left text-xs">
              <thead className="bg-slate-900">
                <tr>
                  <th className="px-3 py-2 font-semibold text-slate-300">
                    Recorded at
                  </th>
                  <th className="px-3 py-2 font-semibold text-slate-300">
                    Shop
                  </th>
                  <th className="px-3 py-2 font-semibold text-slate-300">
                    Price
                  </th>
                </tr>
              </thead>
              <tbody>
                {product.history.map((row) => (
                  <tr
                    key={row.id}
                    className="border-t border-slate-800"
                  >
                    <td className="px-3 py-2 text-slate-400">
                      {row.recordedAtUtc
                        ? new Date(row.recordedAtUtc).toLocaleString()
                        : '—'}
                    </td>
                    <td className="px-3 py-2">{row.shopName}</td>
                    <td className="px-3 py-2">
                      {row.amount.toFixed(2)} {row.currency}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ) : (
          <p className="text-xs text-slate-400">
            No history rows found for this product.
          </p>
        )}
      </section>
    </div>
  );
};
