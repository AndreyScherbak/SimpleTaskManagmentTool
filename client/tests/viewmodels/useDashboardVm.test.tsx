import { renderHook } from '@testing-library/react';
import { rest, server } from '../testServer';
import { useDashboardVm } from '../../src/viewmodels/useDashboardVm';

describe('useDashboardVm', () => {
  it('loads boards on mount', async () => {
    server.use(
      rest.get('/api/boards', (_, res, ctx) =>
        res(ctx.json({ boards: [{ id: '1', title: 'Test', tasksCount: 0 }] })),
      ),
    );
    const { result, waitForNextUpdate } = renderHook(() => useDashboardVm());
    await waitForNextUpdate();
    expect(result.current.boards.length).toBe(1);
  });
});
