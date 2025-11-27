import React from 'react';
import { Link } from 'react-router-dom';

export const NotFoundPage: React.FC = () => {
  return (
    <div className="space-y-4 text-center">
      <h1 className="text-3xl font-bold tracking-tight">404 – Not found</h1>
      <p className="text-sm text-slate-400">
        The page you were looking for does not exist.
      </p>
      <Link
        to="/"
        className="inline-flex items-center rounded-full bg-sky-500 px-4 py-2 text-xs font-semibold text-slate-950 hover:bg-sky-400"
      >
        Back to home
      </Link>
    </div>
  );
};
