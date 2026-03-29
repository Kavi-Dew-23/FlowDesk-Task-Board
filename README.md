# FlowDesk Task Board API

A backend REST API service for the FlowDesk Task Board, built with ASP.NET Core (.NET 9) using Clean Architecture principles. This service allows teams to manage tasks within projects — creating, organising, updating, and tracking them through different stages of completion.

## Tech Stack
- **Framework-** ASP.NET Core 9 
- **Database-** SQLite
- **Authentication-** JWT Bearer Token
- **Testing-** xUnit + FluentAsserations

## Architecture

```
FlowDesk/
├── FlowDesk.API/               # Controllers, Middleware, Program.cs
├── FlowDesk.Application/       # DTOs, Interfaces, Business contracts
├── FlowDesk.Domain/            # Entities, Enums, Domain logic
├── FlowDesk.Infrastructure/    # EF Core, Services, Database
└── FlowDesk.Tests/             # Unit tests
```

## Steps

### 1. Clone the repo

```bash
git clone https://github.com/Kavi-Dew-23/FlowDesk-Task-Board.git
cd FlowDesk.API
```

### 2. Install EF Core tools (If not installed already)

```bash
dotnet tool install --global dotnet-ef
```

### 3. Apply database migrations
```bash
dotnet ef database update --project FlowDesk.Infrastructure --startup-project FlowDesk.API
```

### 4. Run the API
```bash
cd FlowDesk.API
dotnet run
```

### 5. Open Swagger
``` bash
http://localhost:5017/swagger/index.html
```

## API endpoints

### Auth

| Method |         Endpoint        |         Description     |
|--------|-------------------------|-------------------------|
| POST   | `/api/auth/register`    | Register a new user     |
| POST   | `/api/auth/login`       | Login and get JWT token |

### Projects

| Method |         Endpoint     |     Description      |
|--------|----------------------|----------------------|
| POST   | `/api/projects`      | Create a new project |
| GET    | `/api/projects`      | Get all projects     |
| GET    | `/api/projects/{id}` | Get project by ID    |

### Tasks

| Method |                Endpoint                           |        Description           |
|--------|---------------------------------------------------|------------------------------|
| POST   | `/api/projects/{projectId}/tasks`                 | Create a task                |
| GET    | `/api/projects/{projectId}/tasks`                 | Get all tasks (with filters) |
| PUT    | `/api/projects/{projectId}/tasks/{taskId}`        | Update a task                |
| PATCH  | `/api/projects/{projectId}/tasks/{taskId}/status` | Transition task status       |
| PATCH  | `/api/projects/{projectId}/tasks/{taskId}/archive`| Archive a task               |
| GET    | `/api/projects/{projectId}/tasks/archived`        | Get archived tasks           |

### Query Parameters for GET /tasks

| Parameter  |   Example |     Description    |
|------------|-----------|--------------------|
| status     | `ToDo`    | Filter by status   |
| priority   | `High`    | Filter by priority |
| assigneeId | `guid`    | Filter by assignee |
| sortBy     | `duedate` | Sort by field      |
| page       | `1`       | Page number        |
| pageSize   | `10`      | Items per page     |

## Testing the API

### step1 - Register

```json
POST api/auth/register
```

### step2 - Copy the token from response and authorixe in swagger
```
Click the authorize and paste the token and authorize it.
```

  ### step3 - Create a project
  ```json
POST api/projects
```

### step4 - Create a task
```json
POST /api/projects/{projectId}/tasks
```

### step5 - Move the task through workflow
```json
PATCH /api/projects/{projectID}/tasks/{taskID}/status
```

Change the status of the new status

### Running Test
```bash
dotnet test
