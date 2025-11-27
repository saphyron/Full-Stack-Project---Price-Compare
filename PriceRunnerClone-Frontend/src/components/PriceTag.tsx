import React from 'react';

interface PriceTagProps {
  amount?: number | null;
  currency?: string;
  amountLabel?: string;
}

export const PriceTag: React.FC<PriceTagProps> = ({
  amount,
  currency = 'DKK',
  amountLabel = 'Price',
}) => {
  return (
    <div className="inline-flex flex-col rounded-lg bg-slate-800 px-3 py-2 text-xs">
      <span className="text-[10px] uppercase tracking-wide text-slate-400">
        {amountLabel}
      </span>
      {typeof amount === 'number' ? (
        <span className="text-sm font-semibold text-sky-300">
          {amount.toFixed(2)} {currency}
        </span>
      ) : (
        <span className="text-xs text-slate-500">See details</span>
      )}
    </div>
  );
};
