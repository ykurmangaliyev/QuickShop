import * as React from 'react';
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from "react-router-dom";

import Navbar from '../component/Navbar/Navbar';

import HomePage from '../page/home/HomePage';
import StorePage from '../page/store/StorePage';

export default function App() {
  return (
    <React.Fragment>
      <Router>
        <Navbar />
        <div className="container">
          <Switch>
            <Route exact path="/" component={HomePage}/>

            <Route exact path="/manage">
              TODO: manage
            </Route>

            <Route exact path="/store/:store" component={StorePage}/>

            <Redirect to="/" />
          </Switch>
        </div>
      </Router>
    </React.Fragment>
  );
}