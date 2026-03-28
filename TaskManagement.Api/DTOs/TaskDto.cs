namespace TaskManagement.Api.DTOs;

// Used when SENDING data back to frontend (Response)
public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// Used when RECEIVING data from frontend (Create / Update Request)
public class CreateTaskDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Status { get; set; }
}

public class UpdateTaskDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Status { get; set; }
}
