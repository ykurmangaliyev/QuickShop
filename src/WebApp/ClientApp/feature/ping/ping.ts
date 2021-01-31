import { makeQuery, gql } from '../../helper/graphql';

export const pingAsync = () => {
  return async function (dispatch: any): Promise<void> {
    const query = gql`
    query Ping {
      ping {  
        serverTime
        databaseStatus
        databasePing
      }
    }
  `;

    let data = await makeQuery(query, {});

    if (data) {
      console.log(data);
    }
  };
};