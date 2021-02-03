import { ApolloClient, InMemoryCache } from '@apollo/client';

const client = new ApolloClient({
  uri: '/graphql',
  cache: new InMemoryCache(),
  defaultOptions: {
    query: {
      fetchPolicy: 'no-cache',
    },
  },
});

export async function makeQuery<TData>(query: any, variables: any): Promise<TData> {
  const result = await client.query({ query, variables });
  return result.data;
}

export { gql } from '@apollo/client';