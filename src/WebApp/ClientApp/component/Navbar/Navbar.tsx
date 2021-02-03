import * as React from 'react';
import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';

import { IRootStoreType } from '../../main/store';

export default function QuickShopNavbar() {
  const authenticationState = useSelector((state: IRootStoreType) => state.authentication);

  return (
    <>
      <nav className="navbar navbar-expand-lg navbar-fixed-top bg-info">
        <div className="container">
          <Link
            className="navbar-brand"
            to="/"
          >
            <p>Quick Shop</p>
          </Link>

          <div className="justify-content-end">
            <ul className="navbar-nav">
              <li className="nav-item">
                <Link
                  className="nav-link"
                  to="/manage"
                >
                  <i className="now-ui-icons shopping_basket"></i>
                  <p>{authenticationState.user ? 'Go to my store' : 'Create your own store!'}</p>
                </Link>
              </li>
            </ul>
          </div>
        </div>
      </nav>
    </>
  );
}