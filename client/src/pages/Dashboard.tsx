import {
  Typography,
  CircularProgress,
  Button,
  TextField,
  Stack,
} from '@mui/material';
import { useState } from 'react';
import BoardCard from '../components/BoardCard';
import { useDashboardVm } from '../viewmodels/useDashboardVm';

export default function Dashboard() {
  const { boards, loading, error, createBoard } = useDashboardVm();
  const [title, setTitle] = useState('');

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
      <Stack direction="row" spacing={2} sx={{ mt: 2 }}>
        <TextField
          size="small"
          label="Board title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
        <Button
          variant="contained"
          disabled={!title}
          onClick={async () => {
            await createBoard(title);
            setTitle('');
          }}
        >
          Add Board
        </Button>
      </Stack>
    </div>
  );
}
