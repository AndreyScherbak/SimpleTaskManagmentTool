import { rest, server } from '../testServer';
import { fetchBoards, fetchBoard, createBoard } from '../../src/api/boardsApi';

describe('boardsApi', () => {
  it('fetchBoards calls GET /api/boards', async () => {
    server.use(
      rest.get('/api/boards', (_, res, ctx) =>
        res(ctx.json({ boards: [{ id: '1', title: 'Test', tasksCount: 0 }] })),
      ),
    );
    const boards = await fetchBoards();
    expect(boards[0].title).toBe('Test');
  });

  it('createBoard posts to /api/boards', async () => {
    server.use(
      rest.post('/api/boards', async (req, res, ctx) => {
        const body = await req.json();
        return res(ctx.json({ board: { id: '2', title: body.title, tasksCount: 0 } }));
      }),
    );
    const board = await createBoard('New');
    expect(board.title).toBe('New');
  });

  it('fetchBoard calls GET /api/boards/:id', async () => {
    server.use(
      rest.get('/api/boards/1', (_, res, ctx) =>
        res(ctx.json({ board: { id: '1', title: 'A', tasksCount: 0 }, tasks: [] })),
      ),
    );
    const resp = await fetchBoard('1');
    expect(resp.board.id).toBe('1');
  });
});
