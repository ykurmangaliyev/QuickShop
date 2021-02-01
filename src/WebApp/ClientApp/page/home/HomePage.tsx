import './home.less';

import * as React from 'react';
import { useSelector, useDispatch } from 'react-redux';

import {Button} from 'reactstrap';
import HomePageHeader from './HomePageHeader';
import HomePageNavbar from './HomePageNavbar';

import { IRootStoreType } from '../../store';

import { signInAsync, signOutAsync } from '../../feature/authentication/authenticationSlice';
import { pingAsync } from '../../feature/ping/ping';

// Main part
function renderButtons() {
  const dispatch = useDispatch();
  const authenticationState = useSelector((state: IRootStoreType) => state.authentication);

  if (authenticationState.token == null) {
    return (
      <React.Fragment>
        <Button onClick={() => dispatch(signInAsync("first", "password"))}>Sign in</Button>
        <Button onClick={() => dispatch(pingAsync())}>Ping</Button>
      </React.Fragment>
    );
  };

  return (
    <React.Fragment>
      <div>Hola! Your token is {JSON.stringify(authenticationState.user)}</div>
      <Button onClick={() => dispatch(signOutAsync())}>Sign out</Button>
      <Button onClick={() => dispatch(pingAsync())}>Ping</Button>
    </React.Fragment>
  );
}

export default function Home() {
  return (
    <React.Fragment>
      <HomePageNavbar/>
      <div className="wrapper">
        <HomePageHeader/>
      </div>
      
      {renderButtons()}
    </React.Fragment>
  );
}