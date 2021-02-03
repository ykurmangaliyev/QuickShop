import * as React from 'react';
import { useState } from 'react';

import { makeQuery } from './graphql';

interface IProps<TData> {
  query: any,
  variables: any,
  renderLoading: () => JSX.Element,
  renderData: (data: TData) => JSX.Element,
  renderError: (errorMessage: string) => JSX.Element,
}

enum Status {
  Loading,
  Error,
  Success,
}

interface IState<TData> {
  status: Status,
  data: TData,
  errorMessage: string,
}

export default function GraphQLLoader<TData>(props: IProps<TData>): JSX.Element {
  const [state, setState] = useState<IState<TData>>({
    status: Status.Loading,
    data: null,
    errorMessage: null,
  });

  React.useEffect(() => {
    makeQuery(props.query, props.variables)
      .then((data: TData) => {
        setState({
          status: Status.Success,
          data: data,
          errorMessage: null,
        })
      })
      .catch(error => {
        setState({
          status: Status.Error,
          data: null,
          errorMessage: error.message,
        })
      });
  }, []);  

  let content;
  switch (state.status)
  {
    case Status.Loading:
      content = props.renderLoading();
      break;

    case Status.Error:
      content = props.renderError(state.errorMessage);
      break;

    case Status.Success:
      content = props.renderData(state.data);
      break;
  }

  return <React.Fragment>
    {content}
  </React.Fragment>;
}