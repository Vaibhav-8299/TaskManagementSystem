using TaskManagement.Api.DTOs;

namespace TaskManagement.Api.Services;

// Interface — defines the "contract" for what TaskService must do
public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetAllTasksAsync();
    Task<TaskDto?> GetTaskByIdAsync(int id);
    Task<TaskDto> CreateTaskAsync(CreateTaskDto dto);
    Task<bool> UpdateTaskAsync(int id, UpdateTaskDto dto);
    Task<bool> DeleteTaskAsync(int id);
    Task<bool> UpdateTaskStatusAsync(int id, string status);
}
