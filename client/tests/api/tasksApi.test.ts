import { rest, server } from '../testServer';
import { createTask, updateTask, moveTask, deleteTask } from '../../src/api/tasksApi';

describe('tasksApi', () => {
  it('createTask posts to /api/boards/:boardId/tasks', async () => {
    server.use(
      rest.post('/api/boards/1/tasks', async (req, res, ctx) => {
        const body = await req.json();
        return res(ctx.json({ id: 't1', boardId: '1', title: body.title, createdAt: new Date().toISOString(), dueDate: body.dueDate, status: 'Todo' }));
      }),
    );
    const task = await createTask('1', 'New Task', null);
    expect(task.title).toBe('New Task');
  });

  it('updateTask puts to /api/boards/:boardId/tasks/:taskId', async () => {
    server.use(
      rest.put('/api/boards/1/tasks/t1', async (req, res, ctx) => {
        const body = await req.json();
        return res(ctx.json({ id: 't1', boardId: '1', title: body.title, createdAt: new Date().toISOString(), dueDate: body.dueDate, status: 'Todo' }));
      }),
    );
    const task = await updateTask('1', 't1', 'Edit', null);
    expect(task.title).toBe('Edit');
  });

  it('moveTask posts to /move', async () => {
    server.use(
      rest.post('/api/boards/1/tasks/t1/move', (_, res, ctx) =>
        res(ctx.json({ id: 't1', boardId: '1', title: 'A', createdAt: '', dueDate: null, status: 'Done' })),
      ),
    );
    const task = await moveTask('1', 't1', 'Done');
    expect(task.status).toBe('Done');
  });

  it('deleteTask deletes /tasks/:id', async () => {
    server.use(
      rest.delete('/api/boards/1/tasks/t1', (_, res, ctx) =>
        res(ctx.json({ id: 't1', boardId: '1', title: 'A', createdAt: '', dueDate: null, status: 'Todo' })),
      ),
    );
    const task = await deleteTask('1', 't1');
    expect(task.id).toBe('t1');
  });
});
