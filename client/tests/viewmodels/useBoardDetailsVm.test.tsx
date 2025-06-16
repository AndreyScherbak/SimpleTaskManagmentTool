import { renderHook, act, waitFor } from '@testing-library/react';
import { rest, server } from '../testServer';
import { useBoardDetailsVm } from '../../src/viewmodels/useBoardDetailsVm';
import { MemoryRouter, Route, Routes } from 'react-router-dom';

describe('useBoardDetailsVm', () => {
  it('loads board and tasks', async () => {
    server.use(
      rest.get('/api/boards/1', (_, res, ctx) =>
        res(
          ctx.json({
            board: { id: '1', title: 'Board', tasksCount: 1 },
            tasks: [{ id: 't', boardId: '1', title: 'T', createdAt: '', dueDate: null, status: 'Todo' }],
          }),
        ),
      ),
    );
    const wrapper = ({ children }: { children: React.ReactNode }) => (
      <MemoryRouter initialEntries={['/boards/1']}>
        <Routes>
          <Route path="/boards/:id" element={children} />
        </Routes>
      </MemoryRouter>
    );

    const { result } = renderHook(() => useBoardDetailsVm(), { wrapper });
    await waitFor(() => expect(result.current.tasks.length).toBe(1));
    expect(result.current.board?.title).toBe('Board');
  });

  it('createTask adds task', async () => {
    let getCount = 0;
    server.use(
      rest.get('/api/boards/1', (_, res, ctx) => {
        getCount++;
        if (getCount === 1) {
          return res(
            ctx.json({ board: { id: '1', title: 'B', tasksCount: 0 }, tasks: [] }),
          );
        }
        return res(
          ctx.json({
            board: { id: '1', title: 'B', tasksCount: 1 },
            tasks: [
              {
                id: 't1',
                boardId: '1',
                title: 'New',
                createdAt: '',
                dueDate: null,
                status: 'Todo',
              },
            ],
          }),
        );
      }),
      rest.post('/api/boards/1/tasks', async (req, res, ctx) => {
        const body = await req.json();
        return res(
          ctx.json({
            id: 't1',
            boardId: '1',
            title: body.title,
            createdAt: '',
            dueDate: body.dueDate,
            status: 'Todo',
          }),
        );
      }),
    );
    const wrapper = ({ children }: { children: React.ReactNode }) => (
      <MemoryRouter initialEntries={['/boards/1']}>
        <Routes>
          <Route path="/boards/:id" element={children} />
        </Routes>
      </MemoryRouter>
    );
    const { result } = renderHook(() => useBoardDetailsVm(), { wrapper });
    await waitFor(() => expect(result.current.tasks).toHaveLength(0));
    await act(async () => {
      await result.current.createTask('New', null);
    });
    expect(result.current.tasks).toHaveLength(1);
  });

  it('update, move and delete task reload board', async () => {
    let phase = 0;
    server.use(
      rest.get('/api/boards/1', (_, res, ctx) => {
        phase++;
        if (phase === 1) {
          return res(
            ctx.json({
              board: { id: '1', title: 'B', tasksCount: 1 },
              tasks: [
                { id: 't1', boardId: '1', title: 'Old', createdAt: '', dueDate: null, status: 'Todo' },
              ],
            }),
          );
        }
        if (phase === 2) {
          return res(
            ctx.json({
              board: { id: '1', title: 'B', tasksCount: 1 },
              tasks: [
                { id: 't1', boardId: '1', title: 'Updated', createdAt: '', dueDate: null, status: 'Todo' },
              ],
            }),
          );
        }
        if (phase === 3) {
          return res(
            ctx.json({
              board: { id: '1', title: 'B', tasksCount: 1 },
              tasks: [
                { id: 't1', boardId: '1', title: 'Updated', createdAt: '', dueDate: null, status: 'Done' },
              ],
            }),
          );
        }
        return res(
          ctx.json({ board: { id: '1', title: 'B', tasksCount: 0 }, tasks: [] }),
        );
      }),
      rest.put('/api/boards/1/tasks/t1', async (req, res, ctx) => {
        return res(ctx.json({ id: 't1', boardId: '1', title: 'Updated', createdAt: '', dueDate: null, status: 'Todo' }));
      }),
      rest.post('/api/boards/1/tasks/t1/move', (_, res, ctx) =>
        res(ctx.json({ id: 't1', boardId: '1', title: 'Updated', createdAt: '', dueDate: null, status: 'Done' })),
      ),
      rest.delete('/api/boards/1/tasks/t1', (_, res, ctx) =>
        res(ctx.json({ id: 't1', boardId: '1', title: 'Updated', createdAt: '', dueDate: null, status: 'Done' })),
      ),
    );

    const wrapper = ({ children }: { children: React.ReactNode }) => (
      <MemoryRouter initialEntries={['/boards/1']}>
        <Routes>
          <Route path="/boards/:id" element={children} />
        </Routes>
      </MemoryRouter>
    );

    const { result } = renderHook(() => useBoardDetailsVm(), { wrapper });
    await waitFor(() => expect(result.current.tasks[0].title).toBe('Old'));
    await act(async () => {
      await result.current.updateTask('t1', 'Updated', null);
      await result.current.moveTask('t1', 'Done');
      await result.current.deleteTask('t1');
    });
    expect(result.current.tasks).toHaveLength(0);
  });
});
