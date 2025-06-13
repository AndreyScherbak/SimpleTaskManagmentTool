import { TaskDto } from '../models/TaskDto';

const headers = { 'Content-Type': 'application/json' };

export async function createTask(boardId: string, title: string, dueDate: string | null): Promise<TaskDto> {
  const res = await fetch(`/api/boards/${boardId}/tasks`, {
    method: 'POST',
    headers,
    body: JSON.stringify({ title, dueDate }),
  });
  if (!res.ok) {
    throw new Error(await res.text());
  }
  const json = await res.json();
  return json as TaskDto;
}

export async function updateTask(boardId: string, taskId: string, title: string, dueDate: string | null): Promise<TaskDto> {
  const res = await fetch(`/api/boards/${boardId}/tasks/${taskId}`, {
    method: 'PUT',
    headers,
    body: JSON.stringify({ title, dueDate }),
  });
  if (!res.ok) {
    throw new Error(await res.text());
  }
  const json = await res.json();
  return json as TaskDto;
}

export async function moveTask(boardId: string, taskId: string, targetStatus: string): Promise<TaskDto> {
  const res = await fetch(`/api/boards/${boardId}/tasks/${taskId}/move?targetStatus=${targetStatus}`, {
    method: 'POST',
  });
  if (!res.ok) {
    throw new Error(await res.text());
  }
  const json = await res.json();
  return json as TaskDto;
}

export async function deleteTask(boardId: string, taskId: string): Promise<TaskDto> {
  const res = await fetch(`/api/boards/${boardId}/tasks/${taskId}`, {
    method: 'DELETE',
  });
  if (!res.ok) {
    throw new Error(await res.text());
  }
  const json = await res.json();
  return json as TaskDto;
}
