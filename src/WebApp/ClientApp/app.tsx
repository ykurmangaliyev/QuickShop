import * as React from 'react';
import * as ReactDOM from 'react-dom';

type AppState = {
  auth: {
    token?: String
  };
}

type AppProps = {
};

class App extends React.Component<AppProps, AppState>
{
  constructor(props: AppProps) {
    super(props);

    this.state = {
      auth: {
        token: "",
      }
    };
  }

  render() {
    return (
      <div>Hola !!!</div>
    );
  }
}

ReactDOM.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
  document.getElementById("app")
);