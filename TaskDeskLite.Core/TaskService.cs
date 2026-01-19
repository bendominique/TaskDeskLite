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
        //para a tarefa ser valida, conferir com:
        ValidateTask(task);

        //id da tarefa.
        task.Id = Guid.NewGuid();
        //tarefas novas começando como Pendente
        task.Status = TaskStatus.Pending;
        //data e hora guardadas
        task.CreatedAt = DateTime.Now;

        //salvar a tarefa na lista (memoria)
        _tasks.Add(task);

        return task;
    }
       ///////// validaçao da tarefa:
    private void ValidateTask(TaskItem task)
    {
        // verifica se o titulo esta vazio ou só com espaços
        if (string.IsNullOrWhiteSpace(task.Title))
            throw new BusinessRuleException("O título é obrigatório.");

        //verifica se o titulo é muito curto ou longo
        if (task.Title.Length < 3 || task.Title.Length > 40)
            throw new BusinessRuleException("O título deve ter entre 3 e 40 caracteres.");
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
        // Busca a tarefa (se não existir, dá erro)
        var task = GetById(id);

        // Remove da lista
        _tasks.Remove(task);
    }

    public TaskItem MarkAsDone(Guid id)
    {
        // TODO: buscar existente
        // TODO: marcar Done
        // TODO: retornar
        throw new NotImplementedException();
    }
}
