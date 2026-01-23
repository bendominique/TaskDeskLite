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
            throw new DomainValidationException("O título deve ter entre 3 e 40 caracteres.");
        //verifica se a data da tarefa
        if (task.CreatedAt < DateTime.Today)
            throw new BusinessRuleException("A data de criação da tarefa deve ser igual ou após o dia atual.");
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
        // Busca a tarefa (se não existir, dá erro)
        var task = GetById(id);

        // Remove da lista
        _tasks.Remove(task);
    }
    public TaskItem MarkAsDone(Guid id)
    {
        // TODO: buscar id existente
        // Chama o método GetById para buscar a tarefa pelo id e armazena na variável
        var task = GetById(id);

   
        task.Status  = TaskStatus.Done;
        return task;
    }
}
