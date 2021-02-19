import { makeQuery, gql } from '../../graphql/graphql';

export const pingAsync = () => {
  return async function (dispatch: any): Promise<void> {
    const result = await fetch('/api/ping',
      {
        method: 'POST',
        body: '{}',
        headers: {
          "Content-Type": "application/json"
        }
      }
    );

    let data = await result.json();

    if (data) {
      console.log(data);
    }
  };
};