import { httpClient } from './httpClient';

export interface ProductPriceDto {
  id: number;
  shopId: number;
  shopName: string;
  amount: number;
  currency: string;
  lastUpdatedUtc?: string | null;
}

export interface ProductHistoryDto {
  id: number;
  shopId: number;
  shopName: string;
  amount: number;
  currency: string;
  recordedAtUtc?: string | null;
}

export interface ProductDto {
  id: number;
  name: string;
  productUrl?: string | null;
  shopId?: number | null;
  shopName?: string | null;
  brandName?: string | null;
  categoryName?: string | null;
  prices?: ProductPriceDto[];
  history?: ProductHistoryDto[];
}

export async function getProducts(): Promise<ProductDto[]> {
  // Bruger endpoint der inkluderer brand/category
  return httpClient.get<ProductDto[]>('/api/products/with-brand-category');
}

export async function getProduct(id: number): Promise<ProductDto> {
  return httpClient.get<ProductDto>(`/api/products/${id}`);
}
