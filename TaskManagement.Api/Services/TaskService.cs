using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Data;
using TaskManagement.Api.DTOs;
using TaskModel = TaskManagement.Api.Models.Task;

namespace TaskManagement.Api.Services;

// TaskService implements ITaskService
// It talks DIRECTLY to DbContext (no Repository layer)
// Manual mapping is used (no AutoMapper)
public class TaskService : ITaskService
{
    private readonly TaskManagementDbContext _context;

    // DbContext is injected via constructor (Dependency Injection)
    public TaskService(TaskManagementDbContext context)
    {
        _context = context;
    }

    // GET ALL TASKS
    public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
    {
        var tasks = await _context.Tasks.ToListAsync();

        // Manual mapping: Task (DB Model) → TaskDto (Response)
        return tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt
        });
    }

    // GET TASK BY ID
    public async Task<TaskDto?> GetTaskByIdAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null) return null;

        // Manual mapping: Task → TaskDto
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }

    // CREATE TASK
    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto)
    {
        // Business Rule: Default status = Pending if not provided
        var task = new TaskModel
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = string.IsNullOrEmpty(dto.Status) ? "Pending" : dto.Status
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Return the newly created task as DTO
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }

    // UPDATE TASK
    public async Task<bool> UpdateTaskAsync(int id, UpdateTaskDto dto)
    {
        // Business Rule: Task must exist before updating
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return false;

        // Business Rule: Status must be a valid ENUM value
        // MySQL ENUM only allows 'Pending' or 'Completed'
        var validStatuses = new[] { "Pending", "Completed" };
        if (!string.IsNullOrEmpty(dto.Status) && !validStatuses.Contains(dto.Status))
            throw new ArgumentException($"Invalid status '{dto.Status}'. Allowed values: Pending, Completed.");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;

        await _context.SaveChangesAsync();
        return true;
    }

    // DELETE TASK
    public async Task<bool> DeleteTaskAsync(int id)
    {
        // Business Rule: Task must exist before deleting
        var task = await _context.Tasks.FindAsync(id);

        if (task == null) return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }

    // UPDATE STATUS ONLY (PATCH)
    public async Task<bool> UpdateTaskStatusAsync(int id, string status)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null) return false;

        // Business Rule: Only valid statuses are allowed
        if (status != "Pending" && status != "Completed") return false;

        task.Status = status;
        await _context.SaveChangesAsync();
        return true;
    }
}
