import '@testing-library/jest-dom';
import 'whatwg-fetch';
import { server } from './testServer';

beforeAll(() => server.listen());
afterEach(() => server.resetHandlers());
afterAll(() => server.close());
