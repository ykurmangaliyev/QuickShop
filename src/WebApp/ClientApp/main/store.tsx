import { configureStore } from '@reduxjs/toolkit';

import authenticationReducer, { IAuthenticationSliceState } from '../feature/authentication/authenticationSlice';

export default configureStore({
  reducer: {
    authentication: authenticationReducer,
  }
})

export interface IRootStoreType {
  authentication: IAuthenticationSliceState,
}