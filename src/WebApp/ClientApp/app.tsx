import * as React from 'react';
import * as ReactDOM from 'react-dom';

import { signIn, signOut, getCurrentToken, getUserClaims } from './helper/authentication';

import Button from '@material-ui/core/Button';

import { makeQuery, gql } from './helper/graphql';

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
    const query = gql`
      query Ping {
        ping {  
          serverTime
          databaseStatus
          databasePing
        }
      }
    `;

    let data = await makeQuery(query, {});

    if (data) {
      console.log(data);
    }
  }

  async handleSignIn(): Promise<void> {
    const result = await signIn("first", "password");

    if (result.resultCode === "Success") {
      this.setState({
        auth: {
          token: JSON.stringify(getUserClaims()),
        },
      });
    }
  }

  async handleSignOut(): Promise<void> {
    await signOut();

    this.setState({
      auth: {
        token: null,
      },
    });
  }

  render(): React.ReactNode {
    if (this.state.auth.token == null) {
      return (
        <div>
          <Button onClick={this.handleSignIn}>SignIn</Button>
          <Button onClick={this.handlePing}>Ping</Button>
        </div>
      );
    };

    return (
      <>
        <div>Hola! Your token is {JSON.stringify(getUserClaims())}</div>
        <Button onClick={this.handleSignOut}>SignOut</Button>
        <Button onClick={this.handlePing}>Ping</Button>
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