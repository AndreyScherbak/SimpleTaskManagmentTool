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

  const loadBoard = async () => {
    setLoading(true);
    try {
      const res = await boardsApi.fetchBoard(boardId);
      setBoard(res.board);
      setTasks(res.tasks);
      setError(null);
    } catch (e) {
      setError((e as Error).message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadBoard();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [boardId]);

  const createTask = async (title: string, dueDate: string | null) => {
    try {
      await tasksApi.createTask(boardId, title, dueDate);
      await loadBoard();
      setActionError(null);
    } catch (e) {
      setActionError((e as Error).message);
    }
  };

  const updateTask = async (taskId: string, title: string, dueDate: string | null) => {
    try {
      await tasksApi.updateTask(boardId, taskId, title, dueDate);
      await loadBoard();
      setActionError(null);
    } catch (e) {
      setActionError((e as Error).message);
    }
  };

  const moveTask = async (taskId: string, targetStatus: string) => {
    try {
      await tasksApi.moveTask(boardId, taskId, targetStatus);
      await loadBoard();
      setActionError(null);
    } catch (e) {
      setActionError((e as Error).message);
    }
  };

  const deleteTask = async (taskId: string) => {
    try {
      await tasksApi.deleteTask(boardId, taskId);
      await loadBoard();
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
