import React from 'react';
import { Link } from 'react-router-dom';

export const HomePage: React.FC = () => {
  return (
    <div className="space-y-6">
      <section className="space-y-2">
        <h1 className="text-2xl font-bold tracking-tight">
          PriceRunnerClone – frontend
        </h1>
        <p className="text-sm text-slate-300">
          React + Vite + Tailwind, der taler sammen med .NET 9 Dapper API.
        </p>
      </section>

      <section className="grid gap-4 sm:grid-cols-2">
        <Link
          to="/products"
          className="rounded-xl border border-sky-500/60 bg-sky-500/10 px-4 py-3 text-sm hover:bg-sky-500/15"
        >
          <h2 className="font-semibold text-sky-200">Browse products</h2>
          <p className="mt-1 text-xs text-sky-100/80">
            Henter data fra <code>/api/products/with-brand-category</code>.
          </p>
        </Link>

        <Link
          to="/login"
          className="rounded-xl border border-slate-700 bg-slate-900 px-4 py-3 text-sm hover:border-slate-500"
        >
          <h2 className="font-semibold text-slate-100">Login</h2>
          <p className="mt-1 text-xs text-slate-400">
            Bruger <code>/api/auth/login</code> – klar til admin-views senere.
          </p>
        </Link>
      </section>
    </div>
  );
};
