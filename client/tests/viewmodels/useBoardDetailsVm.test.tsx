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
    server.use(
      rest.get('/api/boards/1', (_, res, ctx) =>
        res(
          ctx.json({ board: { id: '1', title: 'B', tasksCount: 0 }, tasks: [] }),
        ),
      ),
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
});
