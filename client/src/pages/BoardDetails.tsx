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
      {actionError && (
        <Typography color="error" sx={{ mb: 2 }}>
          {actionError}
        </Typography>
      )}
      <Stack direction="row" spacing={2} alignItems="flex-start">
        {['Todo', 'InProgress', 'Done'].map((status) => (
          <div key={status} style={{ width: '33%' }}>
            <Typography variant="h6">{status}</Typography>
            {tasks.filter((t) => t.status === status).map((t) => (
              <TaskCard
                key={t.id}
                task={t}
                onEdit={(title, dueDate) => updateTask(t.id, title, dueDate)}
                onMove={(s) => moveTask(t.id, s)}
                onDelete={() => deleteTask(t.id)}
              />
            ))}
          </div>
        ))}
      </Stack>
    </div>
  );
}
