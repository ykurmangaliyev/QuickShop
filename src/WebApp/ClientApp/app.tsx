import * as React from 'react';
import * as ReactDOM from 'react-dom';

import { getCurrentToken, getUserClaims } from './helper/authentication';
import { auth, ping, UnauthorizedError } from './api/client';

interface IAppState {
  auth: {
    token: string,
  },
}
 
interface IAppProps { };

class App extends React.Component<IAppProps, IAppState>
{
  constructor(props: IAppProps) {
    super(props);

    this.state = {
      auth: {
        token: getCurrentToken(),
      }
    };

    this.handleSignIn = this.handleSignIn.bind(this);
    this.handleSignOut = this.handleSignOut.bind(this);
    this.handlePing = this.handlePing.bind(this);
  }

  async handlePing(): Promise<void> {
    let result = await ping.ping();

    if (result.ok) {
      console.log(result);
    }
  }

  async handleSignIn(): Promise<void> {
    let result = await auth.signIn("first", "password");

    if (result.ok && result.resultCode === "Success") {
      this.setState({
        auth: {
          token: JSON.stringify(getUserClaims()),
        },
      });
    }
  }

  async handleSignOut(): Promise<void> {
    let result = await auth.signOut();

    if (result.ok) {
      this.setState({
        auth: {
          token: null,
        },
      });
    }
  }

  render(): React.ReactNode {
    if (this.state.auth.token == null) {
      return (
        <div>
          <button onClick={this.handleSignIn}>SignIn</button>
          <button onClick={this.handlePing}>Ping</button>
        </div>
      );
    };

    return (
      <>
        <div>Hola! Your token is {this.state.auth.token}</div>
        <button onClick={this.handleSignOut}>SignOut</button>
        <button onClick={this.handlePing}>Ping</button>
      </>
    );
  }
}

ReactDOM.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
  document.getElementById("app")
);