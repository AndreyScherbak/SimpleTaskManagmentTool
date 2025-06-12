import { useEffect, useState } from 'react';
import { BoardDto } from '../models/BoardDto';
import * as api from '../api/boardsApi';

export function useDashboardVm() {
  const [boards, setBoards] = useState<BoardDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setLoading(true);
    api
      .fetchBoards()
      .then(setBoards)
      .catch((e) => setError(e.message))
      .finally(() => setLoading(false));
  }, []);

  const createBoard = async (title: string) => {
    const board = await api.createBoard(title);
    setBoards((b) => [...b, board]);
  };

  return { boards, loading, error, createBoard };
}
