import { useEffect, useState } from 'react';
import * as productsApi from '../api/productsApi';

interface UseProductsResult {
  products: productsApi.ProductDto[];
  isLoading: boolean;
  error: string | null;
}

export const useProducts = (): UseProductsResult => {
  const [products, setProducts] = useState<productsApi.ProductDto[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let isMounted = true;

    const load = async () => {
      setIsLoading(true);
      setError(null);
      try {
        const data = await productsApi.getProducts();
        if (isMounted) setProducts(data);
      } catch (err) {
        console.error(err);
        if (isMounted) setError('Failed to load products.');
      } finally {
        if (isMounted) setIsLoading(false);
      }
    };

    load();
    return () => {
      isMounted = false;
    };
  }, []);

  return { products, isLoading, error };
};
