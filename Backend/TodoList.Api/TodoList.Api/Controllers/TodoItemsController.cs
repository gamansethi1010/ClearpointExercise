using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(ITodoService todoService, ILogger<TodoItemsController> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            try
            {
                var results = await _todoService.GetIncompleteTodoItemsAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving todo items");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            try
            {
                var result = await _todoService.GetTodoItemAsync(id);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving todo item");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            try
            {
                await _todoService.UpdateTodoItemAsync(id, todoItem);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating todo item");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostTodoItem(TodoItem todoItem)
        {
            try
            {
                if (!_todoService.ValidateTodoItem(todoItem, out var validationError))
                {
                    return BadRequest(validationError);
                }

                var createdTodoItem = await _todoService.CreateTodoItemAsync(todoItem);

                return CreatedAtAction(nameof(GetTodoItem), new { id = createdTodoItem.Id }, createdTodoItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating todo item");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
