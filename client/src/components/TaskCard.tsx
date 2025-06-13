import {
  Card,
  CardContent,
  Typography,
  IconButton,
  TextField,
  Stack,
  Select,
  MenuItem,
  Button,
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { TaskDto } from '../models/TaskDto';
import { useState } from 'react';

interface Props {
  task: TaskDto;
  onEdit: (title: string, dueDate: string | null) => void;
  onMove: (status: string) => void;
  onDelete: () => void;
}

export default function TaskCard({ task, onEdit, onMove, onDelete }: Props) {
  const [editing, setEditing] = useState(false);
  const [title, setTitle] = useState(task.title);
  const [dueDate, setDueDate] = useState(task.dueDate ?? '');

  return (
    <Card sx={{ mb: 1 }}>
      <CardContent>
        {editing ? (
          <Stack spacing={1}>
            <TextField
              size="small"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              label="Title"
            />
            <TextField
              size="small"
              type="date"
              InputLabelProps={{ shrink: true }}
              value={dueDate}
              onChange={(e) => setDueDate(e.target.value)}
            />
            <Stack direction="row" spacing={1}>
              <Button
                variant="contained"
                size="small"
                onClick={() => {
                  onEdit(title, dueDate || null);
                  setEditing(false);
                }}
              >
                Save
              </Button>
              <Button size="small" onClick={() => setEditing(false)}>
                Cancel
              </Button>
            </Stack>
          </Stack>
        ) : (
          <>
            <Typography variant="body1">{task.title}</Typography>
            {task.dueDate && (
              <Typography variant="caption" color="text.secondary">
                Due {new Date(task.dueDate).toLocaleDateString()}
              </Typography>
            )}
            <Stack direction="row" spacing={1} alignItems="center" sx={{ mt: 1 }}>
              <Select
                size="small"
                value={task.status}
                onChange={(e) => onMove(e.target.value as string)}
              >
                {['Todo', 'InProgress', 'Done'].map((s) => (
                  <MenuItem key={s} value={s}>
                    {s}
                  </MenuItem>
                ))}
              </Select>
              <IconButton size="small" onClick={() => setEditing(true)}>
                <EditIcon fontSize="small" />
              </IconButton>
              <IconButton size="small" onClick={onDelete}>
                <DeleteIcon fontSize="small" />
              </IconButton>
            </Stack>
          </>
        )}
      </CardContent>
    </Card>
  );
}
