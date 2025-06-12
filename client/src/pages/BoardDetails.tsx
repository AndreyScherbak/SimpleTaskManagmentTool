import { Typography, CircularProgress, Stack } from '@mui/material';
import TaskCard from '../components/TaskCard';
import { useBoardDetailsVm } from '../viewmodels/useBoardDetailsVm';

export default function BoardDetails() {
  const { board, tasks, loading, error } = useBoardDetailsVm();

  if (loading) return <CircularProgress />;
  if (error) return <Typography color="error">{error}</Typography>;
  if (!board) return null;

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        {board.title}
      </Typography>
      <Stack direction="row" spacing={2} alignItems="flex-start">
        {['Todo', 'InProgress', 'Done'].map((status) => (
          <div key={status} style={{ width: '33%' }}>
            <Typography variant="h6">{status}</Typography>
            {tasks.filter((t) => t.status === status).map((t) => (
              <TaskCard key={t.id} task={t} />
            ))}
          </div>
        ))}
      </Stack>
    </div>
  );
}
