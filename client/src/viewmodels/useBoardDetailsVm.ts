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
  const [actionError, setActionError] = useState<string | null>(null);

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
    try {
      const task = await tasksApi.createTask(boardId, title, dueDate);
      setTasks((t) => [...t, task]);
      setActionError(null);
    } catch (e) {
      setActionError((e as Error).message);
    }
  };

  const updateTask = async (taskId: string, title: string, dueDate: string | null) => {
    try {
      const task = await tasksApi.updateTask(boardId, taskId, title, dueDate);
      setTasks((ts) => ts.map((t) => (t.id === taskId ? task : t)));
      setActionError(null);
    } catch (e) {
      setActionError((e as Error).message);
    }
  };

  const moveTask = async (taskId: string, targetStatus: string) => {
    try {
      const task = await tasksApi.moveTask(boardId, taskId, targetStatus);
      setTasks((ts) => ts.map((t) => (t.id === taskId ? task : t)));
      setActionError(null);
    } catch (e) {
      setActionError((e as Error).message);
    }
  };

  const deleteTask = async (taskId: string) => {
    try {
      await tasksApi.deleteTask(boardId, taskId);
      setTasks((ts) => ts.filter((t) => t.id !== taskId));
      setActionError(null);
    } catch (e) {
      setActionError((e as Error).message);
    }
  };

  return {
    board,
    tasks,
    loading,
    error,
    actionError,
    createTask,
    updateTask,
    moveTask,
    deleteTask,
  };
}
