import jwt_decode from 'jwt-decode';
import Cookies from "universal-cookie";

const cookieName = "JwtBearerAuthentication";

export interface IUserClaims {
  issuedAt: number,
  expireAt: number,
  username: string,
  id: string,
}

export interface IAuthResult {
  resultCode: string,
}

// Main part
export async function signIn(username: string, password: string): Promise<IAuthResult> {
  const result = await fetch('/auth',
    {
      method: 'POST',
      body: JSON.stringify({ username, password }),
      headers: {
        "Content-Type": "application/json"
      }
    }
  );

  if (result.status !== 200 && result.status !== 401)
    throw 'Unexpected response for the sign-in request';

  const responseBody = await result.json();

  return { resultCode: responseBody.resultCode };
}

export async function signOut(): Promise<void> {
  const result = await fetch('/auth',
    {
      method: 'DELETE',
      body: '{}',
      headers: {
        "Content-Type": "application/json"
      }
    }
  );

  if (result.status !== 200)
    throw 'Unexpected response for the sign-out request';
}

export function getCurrentToken() : string
{
  return new Cookies().get(cookieName);
}

export function getUserClaims() : IUserClaims {
  const cookie = getCurrentToken();
  const decoded = jwt_decode(cookie) as any;

  return {
    issuedAt: decoded.iat,
    expireAt: decoded.ist,
    id: decoded.id,
    username: decoded.nameid,
  };
} 