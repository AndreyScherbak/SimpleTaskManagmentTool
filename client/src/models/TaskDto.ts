export interface TaskDto {
  id: string;
  boardId: string;
  title: string;
  createdAt: string;
  dueDate: string | null;
  status: string;
}
