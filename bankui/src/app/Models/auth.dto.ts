export interface LoginRequest {
  usernameOrEmail: string;
  password: string;
}

export interface PasswordResetRequest {
  password: string;
  verifyPassword: string;
  token:string;
}

export interface ForgetUsernameRequest {
  email: string;
}

export interface ForgetPasswordRequest {
  email: string;
}

export interface DecodedToken {
  user: string;
}
