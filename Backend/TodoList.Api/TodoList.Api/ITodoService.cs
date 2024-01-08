using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoList.Api.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItem>> GetIncompleteTodoItemsAsync();
        Task<TodoItem> GetTodoItemAsync(Guid id);
        Task UpdateTodoItemAsync(Guid id, TodoItem todoItem);
        Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem);
        bool ValidateTodoItem(TodoItem todoItem, out string validationError);
    }
}
