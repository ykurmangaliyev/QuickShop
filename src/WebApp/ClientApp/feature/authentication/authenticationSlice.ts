import { createSlice, PayloadAction } from '@reduxjs/toolkit'

import jwt_decode from 'jwt-decode';
import Cookies from "universal-cookie";

const cookieName = "JwtBearerAuthentication";

interface IUserClaims {
  issuedAt: number,
  expireAt: number,
  username: string,
  id: string,
}

export interface IAuthenticationSliceState {
  token: string,
  error: {
    message: string,
  },
  user: IUserClaims,
};

function getCurrentToken(): string {
  return new Cookies().get(cookieName);
}

function getCurrentUser(): IUserClaims {
  const token = getCurrentToken();

  if (!token)
    return null;

  const decoded = jwt_decode(token) as any;

  return {
    issuedAt: decoded.iat,
    expireAt: decoded.ist,
    id: decoded.id,
    username: decoded.nameid,
  };
}

const initialState: IAuthenticationSliceState = {
  token: getCurrentToken(),
  error: {
    message: null,
  },
  user: getCurrentUser(),
};

const postsSlice = createSlice({
  name: 'authentication',
  initialState,
  reducers: {
    signInSucceeded(state: IAuthenticationSliceState, action: PayloadAction<{ token: string, user: IUserClaims }>): void {
      const { token, user } = action.payload;

      state.token = token;
      state.user = user;

      state.error.message = null;
    },
    signInFailed(state: IAuthenticationSliceState, action: PayloadAction<{ errorMessage: string }>): void {
      const { errorMessage } = action.payload;
      state.error.message = errorMessage;
    },
    signOutSucceeded(state: IAuthenticationSliceState, action: PayloadAction<void>): void {
      state.token = null;
      state.user = null;
    },
  },
});

export const signInAsync = (username: string, password: string) => {
  return async function (dispatch: any): Promise<void> {
    const result = await fetch('/api/auth',
      {
        method: 'POST',
        body: JSON.stringify({ username, password }), 
        headers: {
          "Content-Type": "application/json"
        }
      }
    );

    if (result.status !== 200 && result.status !== 401) {
      dispatch(postsSlice.actions.signInFailed({ errorMessage: 'Unexpected response for the sign-in request' }));
      return null;
    }

    const responseBody = await result.json();

    if (responseBody.resultCode !== "Success") {
      dispatch(postsSlice.actions.signInFailed(responseBody.resultCode || 'Unknown error'));
      return null;
    }

    dispatch(postsSlice.actions.signInSucceeded({ token: getCurrentToken(), user: getCurrentUser() }));
    return null;
  };
};

export const signOutAsync = () => {
  return async function (dispatch: any): Promise<void> {
    const result = await fetch('/api/auth',
      {
        method: 'DELETE',
        body: '{}',
        headers: {
          "Content-Type": "application/json"
        }
      }
    );

    if (result.status !== 200) {
      console.log(`Sign-out failed: ${result.status}, will just remove the cookie!`);

      new Cookies().remove(cookieName);
    }

    dispatch(postsSlice.actions.signOutSucceeded());
  };
}

export default postsSlice.reducer;