using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoList.Api.Services
{
    public class TodoService : ITodoService
    {
        private readonly TodoContext _context;

        public TodoService(TodoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetIncompleteTodoItemsAsync()
        {
            return await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
        }

        public async Task<TodoItem> GetTodoItemAsync(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task UpdateTodoItemAsync(Guid id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                throw new ArgumentException("Id mismatch");
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemIdExists(id))
                {
                    throw new KeyNotFoundException($"TodoItem with Id {id} not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<TodoItem> CreateTodoItemAsync(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public bool ValidateTodoItem(TodoItem todoItem, out string validationError)
        {
            validationError = null;

            if (string.IsNullOrEmpty(todoItem?.Description))
            {
                validationError = "Description is required";
                return false;
            }

            if (TodoItemDescriptionExists(todoItem.Description))
            {
                validationError = "Description already exists";
                return false;
            }

            return true;
        }

        private bool TodoItemIdExists(Guid id)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }

        private bool TodoItemDescriptionExists(string description)
        {
            return _context.TodoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }
    }
}
