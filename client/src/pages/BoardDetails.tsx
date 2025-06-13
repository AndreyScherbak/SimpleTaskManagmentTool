import {
  Typography,
  CircularProgress,
  Stack,
  TextField,
  Button,
} from '@mui/material';
import { useState } from 'react';
import TaskCard from '../components/TaskCard';
import { useBoardDetailsVm } from '../viewmodels/useBoardDetailsVm';

export default function BoardDetails() {
  const {
    board,
    tasks,
    loading,
    error,
    actionError,
    createTask,
    updateTask,
    moveTask,
    deleteTask,
  } = useBoardDetailsVm();
  const [title, setTitle] = useState('');
  const [dueDate, setDueDate] = useState('');

  if (loading) return <CircularProgress />;
  if (error) return <Typography color="error">{error}</Typography>;
  if (!board) return null;

  return (
    <div>
      <Typography variant="h4" gutterBottom>
        {board.title}
      </Typography>
      <Stack direction="row" spacing={2} alignItems="center" sx={{ mb: 2 }}>
        <TextField
          size="small"
          label="Task title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
        <TextField
          size="small"
          type="date"
          label="Due date"
          InputLabelProps={{ shrink: true }}
          value={dueDate}
          onChange={(e) => setDueDate(e.target.value)}
        />
        <Button
          variant="contained"
          disabled={!title}
          onClick={async () => {
            await createTask(title, dueDate || null);
            setTitle('');
            setDueDate('');
          }}
        >
          Add Task
        </Button>
      </Stack>
    </div>
  );
}
