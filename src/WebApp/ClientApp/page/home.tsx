import * as React from 'react';
import { useSelector, useDispatch } from 'react-redux';

import Button from '@material-ui/core/Button';

import { IRootStoreType } from '../store';

import { signInAsync, signOutAsync } from '../feature/authentication/authenticationSlice';
import { pingAsync } from '../feature/ping/ping';

// Main part
export default function Home() {
  const dispatch = useDispatch();
  const authenticationState = useSelector((state: IRootStoreType) => state.authentication);

  if (authenticationState.token == null) {
    return (
      <div>
        <Button onClick={() => dispatch(signInAsync("first", "password"))}>SignIn</Button>
        <Button onClick={() => dispatch(pingAsync())}>Ping</Button>
      </div>
    );
  };

  return (
    <React.Fragment>
      <div>Hola! Your token is {JSON.stringify(authenticationState.user)}</div>
      <Button onClick={() => dispatch(signOutAsync())}>SignOut</Button>
      <Button onClick={() => dispatch(pingAsync())}>Ping</Button>
    </React.Fragment>
  );
}