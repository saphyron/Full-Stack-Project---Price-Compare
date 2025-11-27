import React from 'react';
import { Link } from 'react-router-dom';
import type { ProductDto } from '../api/productsApi';
import { PriceTag } from './PriceTag';

interface ProductCardProps {
  product: ProductDto;
}

export const ProductCard: React.FC<ProductCardProps> = ({ product }) => {
  return (
    <article className="flex flex-col gap-2 rounded-xl border border-slate-800 bg-slate-900/80 p-4 shadow-sm transition-colors hover:border-sky-500/60">
      <header className="flex justify-between gap-2">
        <Link
          to={`/products/${product.id}`}
          className="text-sm font-semibold text-slate-50 hover:text-sky-300"
        >
          {product.name}
        </Link>
        {product.brandName && (
          <span className="rounded-full bg-slate-800 px-2 py-0.5 text-[11px] text-slate-300">
            {product.brandName}
          </span>
        )}
      </header>

      {product.categoryName && (
        <p className="text-[11px] uppercase tracking-wide text-slate-500">
          {product.categoryName}
        </p>
      )}

      {product.shopName && (
        <p className="text-xs text-slate-400">
          Main shop:{' '}
          <span className="font-medium text-slate-200">
            {product.shopName}
          </span>
        </p>
      )}

      <div className="mt-2 flex items-center justify-between gap-2">
        <PriceTag amountLabel="Cheapest price" />
        {product.productUrl && (
          <a
            href={product.productUrl}
            target="_blank"
            rel="noreferrer"
            className="text-xs text-sky-400 hover:text-sky-300"
          >
            View original
          </a>
        )}
      </div>
    </article>
  );
};
