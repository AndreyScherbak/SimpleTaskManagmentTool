import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { BoardDto } from '../models/BoardDto';
import { TaskDto } from '../models/TaskDto';
import * as boardsApi from '../api/boardsApi';
import * as tasksApi from '../api/tasksApi';

export function useBoardDetailsVm() {
  const { id } = useParams<{ id: string }>();
  const boardId = id!;
  const [board, setBoard] = useState<BoardDto | null>(null);
  const [tasks, setTasks] = useState<TaskDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setLoading(true);
    boardsApi
      .fetchBoard(boardId)
      .then((res) => {
        setBoard(res.board);
        setTasks(res.tasks);
      })
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, [boardId]);

  const createTask = async (title: string, dueDate: string | null) => {
    const task = await tasksApi.createTask(boardId, title, dueDate);
    setTasks((t) => [...t, task]);
  };

  const updateTask = async (taskId: string, title: string, dueDate: string | null) => {
    const task = await tasksApi.updateTask(boardId, taskId, title, dueDate);
    setTasks((ts) => ts.map((t) => (t.id === taskId ? task : t)));
  };

  const moveTask = async (taskId: string, targetStatus: string) => {
    const task = await tasksApi.moveTask(boardId, taskId, targetStatus);
    setTasks((ts) => ts.map((t) => (t.id === taskId ? task : t)));
  };

  const deleteTask = async (taskId: string) => {
    await tasksApi.deleteTask(boardId, taskId);
    setTasks((ts) => ts.filter((t) => t.id !== taskId));
  };

  return {
    board,
    tasks,
    loading,
    error,
    createTask,
    updateTask,
    moveTask,
    deleteTask,
  };
}
