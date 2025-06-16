import { Card, CardActionArea, CardContent, Typography } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import { BoardDto } from '../models/BoardDto';

interface Props {
  board: BoardDto;
}

export default function BoardCard({ board }: Props) {
  return (
    <Card sx={{ mb: 2 }}>
      <CardActionArea component={RouterLink} to={`/boards/${board.id}`}>
        <CardContent>
          <Typography variant="h6">{board.title}</Typography>
          <Typography variant="body2" color="text.secondary">
            {board.tasksCount} tasks
          </Typography>
        </CardContent>
      </CardActionArea>
    </Card>
  );
}
