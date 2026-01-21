namespace TaskDeskLite.Core;

public interface ITaskService
{
    IReadOnlyList<TaskItem> GetAll();
    TaskItem GetById(Guid id);

    TaskItem Create(TaskItem task);

    TaskItem Update(TaskItem task);

    void Delete(Guid id);

    TaskItem MarkAsDone(Guid id);
}
