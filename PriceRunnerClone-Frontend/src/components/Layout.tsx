import React from 'react';
import { Link, NavLink } from 'react-router-dom';
import { useAuthContext } from '../context/AuthContext';

interface LayoutProps {
  children: React.ReactNode;
}

export const Layout: React.FC<LayoutProps> = ({ children }) => {
  const { user, isAuthenticated, logout } = useAuthContext();

  const isAdmin =
    !!user &&
    (user.userRoleName?.toLowerCase() === 'admin' || user.userRoleId === 1);

  return (
    <div className="flex min-h-screen flex-col bg-slate-950 text-slate-100">
      <header className="border-b border-slate-800 bg-slate-900/80 backdrop-blur">
        <div className="mx-auto flex max-w-6xl items-center justify-between gap-4 px-4 py-3">
          <Link to="/" className="flex items-center gap-2">
            <span className="text-lg font-semibold tracking-tight">
              PriceRunner<span className="text-sky-400">Clone</span>
            </span>
          </Link>
          <nav className="flex items-center gap-4 text-sm">
            <NavLink
              to="/products"
              className={({ isActive }) =>
                `hover:text-sky-300 ${
                  isActive ? 'text-sky-400 font-medium' : 'text-slate-300'
                }`
              }
            >
              Products
            </NavLink>

            {isAdmin && (
              <NavLink
                to="/admin/products"
                className={({ isActive }) =>
                  `hidden rounded-full border border-amber-500/60 px-3 py-1 text-xs font-medium text-amber-300 hover:bg-amber-500/10 sm:inline ${
                    isActive ? 'bg-amber-500/10' : ''
                  }`
                }
              >
                Admin products
              </NavLink>
            )}

            {isAuthenticated ? (
              <>
                <span className="hidden text-xs text-slate-400 sm:inline">
                  Logged in as{' '}
                  <span className="font-medium text-slate-100">
                    {user?.userName}
                    {user?.userRoleName ? ` (${user.userRoleName})` : ''}
                  </span>
                </span>
                <button
                  type="button"
                  onClick={logout}
                  className="rounded-full border border-slate-700 px-3 py-1 text-xs font-medium hover:bg-slate-800"
                >
                  Logout
                </button>
              </>
            ) : (
              <NavLink
                to="/login"
                className={({ isActive }) =>
                  `rounded-full border border-sky-500/60 px-3 py-1 text-xs font-medium hover:bg-sky-500/10 ${
                    isActive ? 'text-sky-300' : 'text-sky-400'
                  }`
                }
              >
                Login
              </NavLink>
            )}
          </nav>
        </div>
      </header>

      <main className="flex-1">
        <div className="mx-auto max-w-6xl px-4 py-6">{children}</div>
      </main>

      <footer className="border-t border-slate-800 bg-slate-900/80 text-xs text-slate-500">
        <div className="mx-auto flex max-w-6xl justify-between px-4 py-3">
          <span>PriceRunnerClone &copy; {new Date().getFullYear()}</span>
          <span className="hidden sm:inline">
            Demo – .NET 9, Dapper, React, Vite, Tailwind
          </span>
        </div>
      </footer>
    </div>
  );
};
