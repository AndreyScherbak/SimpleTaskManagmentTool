# SimpleTaskManagementTool

This repository implements a simple Kanban-like task manager using a clean architecture approach.

The `docs/` folder contains PlantUML diagrams describing the system.

## Client application

A React 18 + TypeScript SPA lives under `client/`. It communicates with the ASP.NET Core API via `/api` routes.

### Setup

```bash
cd client
npm install
npm run dev
```

### Tests

```bash
npm test -- --watch
```

### Lint & format

```bash
npm run lint
npm run format
```

The client uses Vite, MUI, React Router and simple MVVM hooks for state management.
