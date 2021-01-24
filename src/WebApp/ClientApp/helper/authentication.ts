import jwt_decode from 'jwt-decode';
import Cookies from "universal-cookie";

const cookieName = "JwtBearerAuthentication";

export interface IUserClaims {
  issuedAt: number,
  expireAt: number,
  username: string,
  id: string,
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