import React, { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthContext } from '../context/AuthContext';

export const LoginPage: React.FC = () => {
  const { login } = useAuthContext();
  const navigate = useNavigate();
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (evt: FormEvent) => {
    evt.preventDefault();
    setError(null);
    setIsSubmitting(true);

    try {
      await login(userName, password);
      navigate('/', { replace: true });
    } catch (err) {
      console.error(err);
      setError('Login failed. Check username/password.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="mx-auto max-w-sm rounded-xl border border-slate-800 bg-slate-900/80 p-6 shadow-sm">
      <h1 className="text-lg font-semibold tracking-tight">Login</h1>
      <p className="mt-1 text-xs text-slate-400">
        Uses <code className="text-[11px] text-sky-300">/api/auth/login</code>
      </p>

      <form onSubmit={handleSubmit} className="mt-4 space-y-4 text-sm">
        <div className="space-y-1">
          <label
            htmlFor="userName"
            className="block text-xs font-medium text-slate-300"
          >
            Username
          </label>
          <input
            id="userName"
            type="text"
            autoComplete="username"
            className="w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-slate-100 outline-none ring-sky-500/40 focus:ring"
            value={userName}
            onChange={(e) => setUserName(e.target.value)}
          />
        </div>

        <div className="space-y-1">
          <label
            htmlFor="password"
            className="block text-xs font-medium text-slate-300"
          >
            Password
          </label>
          <input
            id="password"
            type="password"
            autoComplete="current-password"
            className="w-full rounded-md border border-slate-700 bg-slate-950 px-3 py-2 text-sm text-slate-100 outline-none ring-sky-500/40 focus:ring"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>

        {error && (
          <p className="text-xs text-red-400" role="alert">
            {error}
          </p>
        )}

        <button
          type="submit"
          disabled={isSubmitting}
          className="inline-flex w-full items-center justify-center rounded-md bg-sky-500 px-3 py-2 text-xs font-semibold text-slate-950 hover:bg-sky-400 disabled:opacity-60"
        >
          {isSubmitting ? 'Logging in…' : 'Login'}
        </button>
      </form>
    </div>
  );
};
