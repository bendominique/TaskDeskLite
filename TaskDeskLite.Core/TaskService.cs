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
        // Chama o método de validação do arquivo TaskValidator.cs
        TaskValidator.ValidateForCreateOrUpdate(task);

        // TODO: buscar tarefa existente na lista pelo id
        // Busca, retorna e armazena na variável
        var existing = GetById(task.Id);
        // Se a variável estiver nula
        if (existing is null)
        {
            // Lança uma exceção
            throw new NotFoundException("Tarefa não encontrada.");
        }

        // TODO: regra: se Status Done -> não pode editar (BusinessRuleException - erro de regra de negócio no arquivo Program.cs)
        // Se o status da variável que armazena a tarefa existente for igual a Done
        if (existing.Status == TaskStatus.Done)
        {
            // Lança uma mensagem para o usuário
            throw new BusinessRuleException("Não é possível editar uma tarefa que já foi concluída.");
        }

        // TODO: atualizar campos permitidos
        // Pega os campos da tarefa existente e atribuí os valores da tarefa recebida por parâmetro
        existing.Title = task.Title;
        existing.Description = task.Description;
        existing.Priority = task.Priority;
        existing.DueDate = task.DueDate;
        existing.Status = task.Status;

        // TODO: retornar a tarefa atualizada
        return task;
    }

    public void Delete(Guid id)
    {
        // TODO: se não existir -> NotFoundException
        // TODO: remover
        throw new NotImplementedException();
    }

    public TaskItem MarkAsDone(Guid id)
    {
        // TODO: buscar id existente
        // Chama o método GetById para buscar a tarefa pelo id e armazena na variável
        var task = GetById(id);
        // TODO: marcar Done para a tarefa concluída
        // Altera o status da tarefa da variável para Done
        task.Status = TaskStatus.Done;
        // TODO: retornar
        return task;
    }
}
