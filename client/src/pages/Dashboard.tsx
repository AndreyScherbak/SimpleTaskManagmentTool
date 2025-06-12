import { Typography, CircularProgress, Button } from '@mui/material';
import BoardCard from '../components/BoardCard';
import { useDashboardVm } from '../viewmodels/useDashboardVm';

export default function Dashboard() {
  const { boards, loading, error } = useDashboardVm();

  if (loading) return <CircularProgress />;
  if (error) return <Typography color="error">{error}</Typography>;

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        Boards
      </Typography>
      {boards.map((b) => (
        <BoardCard key={b.id} board={b} />
      ))}
      {boards.length === 0 && <Typography>No boards yet</Typography>}
      <Button variant="contained" disabled sx={{ mt: 2 }}>
        Add Board
      </Button>
    </div>
  );
}
