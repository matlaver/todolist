# ToDoList Application

A simple, modern To-Do List web application built with ASP.NET Core minimal APIs.

## Features

- ✅ Add new tasks
- ✅ Mark tasks as complete/incomplete
- ✅ Delete tasks
- ✅ Responsive design
- ✅ Modern UI with gradient styling
- ✅ Real-time updates

## Prerequisites

- .NET 6.0 or later
- Any modern web browser

## Getting Started

1. **Clone or download** this repository
2. **Navigate** to the project directory
3. **Run** the application:
   ```bash
   dotnet run
   ```
4. **Open** your browser and go to `http://localhost:5000` (or the URL shown in the console)

## Usage

- **Add Task**: Type in the input field and click "Add Task" or press Enter
- **Complete Task**: Click the checkbox next to any task
- **Delete Task**: Click the "Delete" button next to any task

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Serves the main web interface |
| GET | `/api/todos` | Get all to-do items |
| POST | `/api/todos` | Create a new to-do item |
| PUT | `/api/todos/{id}/toggle` | Toggle completion status |
| DELETE | `/api/todos/{id}` | Delete a to-do item |

### API Examples

**Get all todos:**
```http
GET /api/todos
```

**Create a new todo:**
```http
POST /api/todos
Content-Type: application/json

{
  "title": "Buy groceries",
  "isCompleted": false
}
```

**Toggle todo completion:**
```http
PUT /api/todos/1/toggle
```

**Delete a todo:**
```http
DELETE /api/todos/1
```

## Technical Details

- **Framework**: ASP.NET Core with minimal APIs
- **Storage**: In-memory (data resets on restart)
- **Frontend**: Vanilla HTML, CSS, and JavaScript
- **Architecture**: Single-file application with embedded UI

## Project Structure

```
ToDoList/
├── Program.cs          # Main application file
└── README.md          # This file
```

## Data Model

```csharp
public class ToDoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}
```

## Notes

- Data is stored in memory and will be lost when the application stops
- The application uses auto-incrementing IDs for new tasks
- The UI is fully responsive and works on mobile devices