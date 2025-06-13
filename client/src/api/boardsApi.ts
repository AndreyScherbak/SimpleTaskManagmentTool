import { BoardDto } from '../models/BoardDto';
import { TaskDto } from '../models/TaskDto';

const BASE = '/api/boards';
const headers = { 'Content-Type': 'application/json' };

export async function fetchBoards(): Promise<BoardDto[]> {
  const res = await fetch(BASE);
  if (!res.ok) {
    throw new Error(await res.text());
  }
  const json = await res.json();
  return json.boards as BoardDto[];
}

export async function fetchBoard(id: string): Promise<{ board: BoardDto; tasks: TaskDto[] }> {
  const res = await fetch(`${BASE}/${id}`);
  if (!res.ok) {
    throw new Error(await res.text());
  }
  return res.json();
}

export async function createBoard(title: string): Promise<BoardDto> {
  const res = await fetch(BASE, {
    method: 'POST',
    headers,
    body: JSON.stringify({ title }),
  });
  if (!res.ok) {
    throw new Error(await res.text());
  }
  const json = await res.json();
  return json.board as BoardDto;
}
