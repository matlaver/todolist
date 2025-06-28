using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var todos = new List<ToDoItem>();
var nextId = 1;

app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html>
<head>
    <title>ToDoList</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <style>
        * { box-sizing: border-box; margin: 0; padding: 0; }
        body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; background: #f5f7fa; padding: 20px; }
        .container { max-width: 600px; margin: 0 auto; background: white; border-radius: 12px; box-shadow: 0 4px 6px rgba(0,0,0,0.1); overflow: hidden; }
        .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; }
        .header h1 { font-size: 2.5em; font-weight: 300; }
        .add-todo { padding: 20px; border-bottom: 1px solid #eee; }
        .input-group { display: flex; gap: 10px; }
        #newTodo { flex: 1; padding: 12px 16px; border: 2px solid #e1e5e9; border-radius: 8px; font-size: 16px; }
        #newTodo:focus { outline: none; border-color: #667eea; }
        .btn { padding: 12px 24px; background: #667eea; color: white; border: none; border-radius: 8px; cursor: pointer; font-size: 16px; font-weight: 500; }
        .btn:hover { background: #5a67d8; }
        .todo-list { list-style: none; }
        .todo-item { display: flex; align-items: center; padding: 16px 20px; border-bottom: 1px solid #f0f0f0; }
        .todo-item:hover { background: #f8f9fa; }
        .todo-checkbox { margin-right: 12px; width: 18px; height: 18px; }
        .todo-text { flex: 1; font-size: 16px; }
        .todo-text.completed { text-decoration: line-through; color: #9ca3af; }
        .delete-btn { background: #ef4444; color: white; border: none; padding: 6px 12px; border-radius: 6px; cursor: pointer; font-size: 14px; }
        .delete-btn:hover { background: #dc2626; }
        .empty-state { text-align: center; padding: 40px; color: #9ca3af; }
    </style>
</head>
<body>
    <div class="container">
        <div class="header">
            <h1>ToDoList</h1>
        </div>
        <div class="add-todo">
            <div class="input-group">
                <input id="newTodo" placeholder="What needs to be done?" />
                <button class="btn" onclick="addTodo()">Add Task</button>
            </div>
        </div>
        <ul id="todoList" class="todo-list"></ul>
        <div id="emptyState" class="empty-state" style="display: none;">
            <p>No tasks yet. Add one above!</p>
        </div>
    </div>
    
    <script>
        async function loadTodos() {
            const response = await fetch('/api/todos');
            const todos = await response.json();
            const list = document.getElementById('todoList');
            const emptyState = document.getElementById('emptyState');
            
            if (todos.length === 0) {
                list.style.display = 'none';
                emptyState.style.display = 'block';
            } else {
                list.style.display = 'block';
                emptyState.style.display = 'none';
                list.innerHTML = todos.map(todo => 
                    `<li class="todo-item">
                        <input type="checkbox" class="todo-checkbox" ${todo.isCompleted ? 'checked' : ''} 
                               onchange="toggleTodo(${todo.id})">
                        <span class="todo-text ${todo.isCompleted ? 'completed' : ''}">${todo.title}</span>
                        <button class="delete-btn" onclick="deleteTodo(${todo.id})">Delete</button>
                    </li>`
                ).join('');
            }
        }
        
        async function addTodo() {
            const input = document.getElementById('newTodo');
            if (!input.value.trim()) return;
            
            await fetch('/api/todos', {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify({title: input.value.trim(), isCompleted: false})
            });
            
            input.value = '';
            loadTodos();
        }
        
        document.getElementById('newTodo').addEventListener('keypress', function(e) {
            if (e.key === 'Enter') addTodo();
        });
        
        async function toggleTodo(id) {
            await fetch(`/api/todos/${id}/toggle`, {method: 'PUT'});
            loadTodos();
        }
        
        async function deleteTodo(id) {
            await fetch(`/api/todos/${id}`, {method: 'DELETE'});
            loadTodos();
        }
        
        loadTodos();
    </script>
</body>
</html>
""", "text/html"));

app.MapGet("/api/todos", () => todos);

app.MapPost("/api/todos", (ToDoItem item) => {
    item.Id = nextId++;
    todos.Add(item);
    return Results.Created($"/api/todos/{item.Id}", item);
});

app.MapPut("/api/todos/{id}/toggle", (int id) => {
    var todo = todos.FirstOrDefault(t => t.Id == id);
    if (todo == null) return Results.NotFound();
    todo.IsCompleted = !todo.IsCompleted;
    return Results.Ok(todo);
});

app.MapDelete("/api/todos/{id}", (int id) => {
    var todo = todos.FirstOrDefault(t => t.Id == id);
    if (todo == null) return Results.NotFound(); 
    todos.Remove(todo);
    return Results.NoContent();
});

app.Run();