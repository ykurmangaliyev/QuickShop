import * as React from 'react';
import { Provider } from 'react-redux';
import * as ReactDOM from 'react-dom';

import './vendor/now-ui-kit/css/bootstrap.min.css'
import './vendor/now-ui-kit/css/now-ui-kit.css'

import App from './main/App';

import store from './main/store';

setTimeout(() => {
    ReactDOM.render(
      <Provider store={store}>
        <App/>
      </Provider>,
      document.getElementById("app")
    );
  },
  500
);
