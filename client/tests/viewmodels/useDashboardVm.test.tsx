import { renderHook, act, waitFor } from '@testing-library/react';
import { rest, server } from '../testServer';
import { useDashboardVm } from '../../src/viewmodels/useDashboardVm';

describe('useDashboardVm', () => {
  it('loads boards on mount', async () => {
    server.use(
      rest.get('/api/boards', (_, res, ctx) =>
        res(ctx.json({ boards: [{ id: '1', title: 'Test', tasksCount: 0 }] })),
      ),
    );
    const { result } = renderHook(() => useDashboardVm());
    await waitFor(() => expect(result.current.boards.length).toBe(1));
  });

  it('createBoard adds a board', async () => {
    server.use(
      rest.get('/api/boards', (_, res, ctx) => res(ctx.json({ boards: [] }))),
      rest.post('/api/boards', async (req, res, ctx) => {
        const body = await req.json();
        return res(ctx.json({ board: { id: '2', title: body.title, tasksCount: 0 } }));
      }),
    );

    const { result } = renderHook(() => useDashboardVm());
    await waitFor(() => expect(result.current.boards).toHaveLength(0));
    await act(async () => {
      await result.current.createBoard('New Board');
    });
    expect(result.current.boards).toHaveLength(1);
    expect(result.current.boards[0].title).toBe('New Board');
  });
});
