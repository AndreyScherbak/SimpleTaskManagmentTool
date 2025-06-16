import Dashboard from './pages/Dashboard';
import BoardDetails from './pages/BoardDetails';
import { RouteObject } from 'react-router-dom';

export const routes: RouteObject[] = [
  {
    path: '/',
    element: <Dashboard />,
  },
  {
    path: '/boards/:id',
    element: <BoardDetails />,
  },
];
