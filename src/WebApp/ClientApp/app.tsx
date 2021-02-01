import * as React from 'react';
import { Provider } from 'react-redux';
import * as ReactDOM from 'react-dom';

import './vendor/now-ui-kit/css/bootstrap.min.css'
import './vendor/now-ui-kit/css/now-ui-kit.css'

import store from './store';

import Home from './page/home/HomePage';

// Main part
function App() {
  return (
    <React.Fragment>
      <Home/>
    </React.Fragment>
  );
}

ReactDOM.render(
  <React.StrictMode>
    <Provider store={store}>
      <App />
    </Provider>
  </React.StrictMode>,
  document.getElementById("app")
);