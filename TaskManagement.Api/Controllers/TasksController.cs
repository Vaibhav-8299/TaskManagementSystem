using Microsoft.AspNetCore.Mvc;
using TaskManagement.Api.DTOs;
using TaskManagement.Api.Services;

namespace TaskManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    // ITaskService injected via constructor (NOT TaskService directly)
    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    // GET /api/tasks
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    // GET /api/tasks/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);

        if (task == null)
            return NotFound(new { message = $"Task with ID {id} not found." });

        return Ok(task);
    }

    // POST /api/tasks
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        // Business Rule: Title is required
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new { message = "Task title is required." });

        var createdTask = await _taskService.CreateTaskAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdTask.Id }, createdTask);
    }

    // PUT /api/tasks/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        // Business Rule: Title is required
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new { message = "Task title is required." });

        try
        {
            var result = await _taskService.UpdateTaskAsync(id, dto);
            if (!result)
                return NotFound(new { message = $"Task with ID {id} not found." });

            return Ok(new { message = "Task updated successfully." });
        }
        catch (ArgumentException ex)
        {
            // Catches invalid status values like 'Approved', 'Done', etc.
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE /api/tasks/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _taskService.DeleteTaskAsync(id);

        if (!result)
            return NotFound(new { message = $"Task with ID {id} not found." });

        return Ok(new { message = "Task deleted successfully." });
    }

    // PATCH /api/tasks/{id}/status
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusDto dto)
    {
        var result = await _taskService.UpdateTaskStatusAsync(id, dto.Status);

        if (!result)
            return BadRequest(new { message = "Task not found or invalid status. Use 'Pending' or 'Completed'." });

        return Ok(new { message = "Task status updated successfully." });
    }
}

// Small DTO used only for PATCH status endpoint
public class UpdateStatusDto
{
    public string Status { get; set; } = null!;
}
