import { Card, CardContent, Typography } from '@mui/material';
import { TaskDto } from '../models/TaskDto';

interface Props {
  task: TaskDto;
}

export default function TaskCard({ task }: Props) {
  return (
    <Card sx={{ mb: 1 }}>
      <CardContent>
        <Typography variant="body1">{task.title}</Typography>
        {task.dueDate && (
          <Typography variant="caption" color="text.secondary">
            Due {new Date(task.dueDate).toLocaleDateString()}
          </Typography>
        )}
      </CardContent>
    </Card>
  );
}
