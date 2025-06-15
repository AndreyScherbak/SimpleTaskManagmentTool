import { CssBaseline, Container } from '@mui/material';
import { Routes, Route } from 'react-router-dom';
import Dashboard from './pages/Dashboard';
import BoardDetails from './pages/BoardDetails';

export default function App() {
  return (
    <>
      <CssBaseline />
      <Container sx={{ mt: 4 }}>
        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/boards/:id" element={<BoardDetails />} />
        </Routes>
      </Container>
    </>
  );
}
