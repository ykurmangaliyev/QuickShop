import './home.less';

import * as React from 'react';
import { useSelector, useDispatch } from 'react-redux';

import { IRootStoreType } from '../../main/store';

import { signInAsync, signOutAsync } from '../../feature/authentication/authenticationSlice';
import { pingAsync } from '../../feature/ping/ping';

// Main part
export default function HomePage() {
  const dispatch = useDispatch();
  const authenticationState = useSelector((state: IRootStoreType) => state.authentication);

  if (authenticationState.token == null) {
    return (
      <React.Fragment>
        <button type="button" className="btn bg-primary" onClick={() => dispatch(signInAsync("first", "password"))}>Sign in</button>
        <button type="button" className="btn bg-primary" onClick={() => dispatch(pingAsync())}>Ping</button>


      </React.Fragment>
    );
  };

  return (
    <React.Fragment>
      <div>Hola! Your token is {JSON.stringify(authenticationState.user)}</div>
      <button type="button" className="btn bg-primary" onClick={() => dispatch(signOutAsync())}>Sign out</button>
      <button type="button" className="btn bg-primary" onClick={() => dispatch(pingAsync())}>Ping</button>
    </React.Fragment>
  );
}