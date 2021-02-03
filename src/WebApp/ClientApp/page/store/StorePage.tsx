import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';

import GraphQLLoader from '../../graphql/GraphQLLoader';
import { gql } from '../../graphql/graphql';

interface IMatchProps {
  store: string,
}

// Main part
export default function StorePage(props: RouteComponentProps<IMatchProps>): JSX.Element {
  const query = gql`
    query Ping {
      ping {  
        serverTime
        databaseStatus
        databasePing
      }
    }
  `;

  return (
    <React.Fragment>
      <h3>{props.match.params.store}</h3>

      <GraphQLLoader
        query={query}
        variables={{}}
        renderLoading={() => <>Loading!</>}
        renderError={(e) => <>Error: {e}!</>}
        renderData={(data) => <>Success: {JSON.stringify(data)}!</>}
      />
    </React.Fragment>
  );
}