namespace TaskDeskLite.Core;

public enum TaskPriority
{
    Low = 0,
    Medium = 1,
    High = 2
}

public enum TaskStatus
{
    Pending = 0,
    Done = 1
}

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
