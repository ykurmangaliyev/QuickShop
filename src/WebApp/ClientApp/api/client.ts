interface IResponse {
  status: number,
  ok: boolean,
}

type HttpMethods = 'GET' | 'POST' | 'DELETE';

async function makeRequest(url: string, method: HttpMethods, body: any = null): Promise<any> {
  const result = await fetch(url,
    {
      method: method,
      body: body ? JSON.stringify(body) : null,
      headers: {
        "Content-Type": "application/json"
      }
    }
  );

  console.log(result);

  let responseBody = {};

  try {
    responseBody = await result.json();
  } catch (e) {
    console.error("Cannot deserialize server response, probably something is wrong...", e);
  }

  if (result.status === 401)
    throw new UnauthorizedError("401 Unauthorized");

  return {
    status: result.status,
    ok: result.ok,
    ...responseBody,
  };
}

// Errors
export class UnauthorizedError extends Error {
  constructor(message: string) {
    super(message);
  }
}

// Auth
interface IAuthenticateResponse extends IResponse {
  resultCode: string,
}

export const auth = {
  signIn(username: string, password: string) : Promise<IAuthenticateResponse> {
    return makeRequest("/api/auth", "POST", { username, password });
  },
  signOut(): Promise<IResponse> {
    return makeRequest("/api/auth", "DELETE");
  }
}

// Ping
interface IPingResponse extends IResponse {
  serverTime: string,
  userId: string,
};

export const ping = {
  ping(): Promise<IPingResponse> {
    return makeRequest("/api/ping", "POST");
  }
}