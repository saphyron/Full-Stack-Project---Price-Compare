import { httpClient } from './httpClient';

export interface LoginRequest {
  userName: string;
  password: string;
}

export interface LoginResponse {
  userId: number;
  userName: string;
  userRoleId?: number | null;
  userRoleName?: string | null;
  token?: string | null;
}

export async function login(
  request: LoginRequest,
): Promise<LoginResponse> {
  return httpClient.post<LoginResponse>('/api/auth/login', request);
}
