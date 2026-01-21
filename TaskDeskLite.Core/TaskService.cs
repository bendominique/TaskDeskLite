namespace TaskDeskLite.Core;

public class TaskService : ITaskService
{
    // Persistência em memória
    private readonly List<TaskItem> _tasks = new();

    public IReadOnlyList<TaskItem> GetAll()
        => _tasks.OrderByDescending(t => t.CreatedAt).ToList();

    public TaskItem GetById(Guid id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task is null) throw new NotFoundException("Tarefa não encontrada.");
        return task;
    }

    public TaskItem Create(TaskItem task)
    {
        // TODO: validar
        // TODO: garantir Id novo e Status Pending
        // TODO: adicionar na lista
        // TODO: retornar a tarefa criada

        throw new NotImplementedException();
    }

    public TaskItem Update(TaskItem task)
    {
        // TODO: validar
        // TODO: buscar existente
        // TODO: regra: se Status Done -> não pode editar (BusinessRuleException)
        // TODO: atualizar campos permitidos
        // TODO: retornar atualizado

        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        // TODO: se não existir -> NotFoundException
        // TODO: remover
        throw new NotImplementedException();
    }

    public TaskItem MarkAsDone(Guid id)
    {
        // TODO: buscar existente
        // TODO: marcar Done
        // TODO: retornar
        throw new NotImplementedException();
    }
}
