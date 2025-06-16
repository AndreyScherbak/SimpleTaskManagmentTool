import { setupServer } from 'msw/node';
import { rest } from 'msw';

const handlers: Parameters<typeof setupServer>[0][] = [];

export const server = setupServer(...handlers);
export { rest };
