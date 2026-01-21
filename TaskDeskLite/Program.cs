
using System.ComponentModel.DataAnnotations;
using TaskDeskLite.Core;

namespace TaskDeskLite;

public class Program
{
    static void Main()
    {
        ITaskService service = new TaskService();

        while (true)
        {
            Console.Clear();
            ConsoleUi.Title("TaskDesk Lite (Console)");

            Console.WriteLine("1) Listar tarefas");
            Console.WriteLine("2) Cadastrar tarefa");
            Console.WriteLine("3) Editar tarefa");
            Console.WriteLine("4) Concluir tarefa");
            Console.WriteLine("5) Excluir tarefa");
            Console.WriteLine("0) Sair");

            var op = ConsoleUi.ReadInt("\nEscolha uma opção: ", min: 0, max: 5);

            try
            {
                switch (op)
                {
                    case 1:
                        ListTasks(service);
                        break;
                    case 2:
                        CreateTask(service);
                        break;
                    case 3:
                        EditTask(service);
                        break;
                    case 4:
                        MarkDone(service);
                        break;
                    case 5:
                        DeleteTask(service);
                        break;
                    case 0:
                        return;
                }
            }
            catch (DomainValidationException ex)
            {
                ConsoleUi.Error($"Validação: {ex.Message}");
                ConsoleUi.Pause();
            }
            catch (NotFoundException ex)
            {
                ConsoleUi.Error($"Não encontrado: {ex.Message}");
                ConsoleUi.Pause();
            }
            catch (BusinessRuleException ex)
            {
                ConsoleUi.Error($"Regra de negócio: {ex.Message}");
                ConsoleUi.Pause();
            }
            catch (Exception ex)
            {
                ConsoleUi.Error($"Erro inesperado: {ex.Message}");
                ConsoleUi.Pause();
            }
        }
    }

    static void ListTasks(ITaskService service)
    {
        Console.Clear();
        ConsoleUi.Title("Lista de tarefas");

        var tasks = service.GetAll();

        if (tasks.Count == 0)
        {
            Console.WriteLine("Nenhuma tarefa cadastrada.");
            ConsoleUi.Pause();
            return;
        }

        foreach (var t in tasks)
        {
            Console.WriteLine($"{t.Id}");
            Console.WriteLine($"  Título:     {t.Title}");
            Console.WriteLine($"  Prioridade: {t.Priority}");
            Console.WriteLine($"  Status:     {t.Status}");
            Console.WriteLine($"  Prazo:      {(t.DueDate?.ToString("dd/MM/yyyy") ?? "-")}");
            Console.WriteLine(new string('-', 40));
        }

        ConsoleUi.Pause();
    }

    static void CreateTask(ITaskService service)
    {
        Console.Clear();
        ConsoleUi.Title("Cadastrar tarefa");

        var title = ConsoleUi.ReadString("Título: ");
        var desc = ConsoleUi.ReadOptionalString("Descrição (opcional): ");

        var priority = ConsoleUi.ReadEnum<TaskPriority>(
            "Prioridade (Low/Medium/High): ",
            defaultValue: TaskPriority.Medium);

        var dueDate = ConsoleUi.ReadOptionalDate("Prazo (dd/MM/yyyy) ou vazio: ");

        var created = service.Create(new TaskItem
        {
            Title = title,
            Description = desc,
            Priority = priority,
            DueDate = dueDate
        });

        ConsoleUi.Success($"Tarefa criada com sucesso! ID: {created.Id}");
        ConsoleUi.Pause();
    }

    static void EditTask(ITaskService service)
    {
        Console.Clear();
        ConsoleUi.Title("Editar tarefa");

        var id = ConsoleUi.ReadGuid("Informe o ID da tarefa: ");
        var existing = service.GetById(id);

        Console.WriteLine("\nDeixe vazio para manter o valor atual.\n");

        var newTitle = ConsoleUi.ReadOptionalString($"Título atual: {existing.Title}\nNovo título: ");
        var newDesc = ConsoleUi.ReadOptionalString($"Descrição atual: {(existing.Description ?? "-")}\nNova descrição: ");

        var newPriority = ConsoleUi.ReadOptionalEnum<TaskPriority>(
            $"Prioridade atual: {existing.Priority}\nNova prioridade (Low/Medium/High): ");

        var newDue = ConsoleUi.ReadOptionalDate($"Prazo atual: {(existing.DueDate?.ToString("dd/MM/yyyy") ?? "-")}\nNovo prazo (dd/MM/yyyy): ");

        var toUpdate = new TaskItem
        {
            Id = existing.Id,
            Title = string.IsNullOrWhiteSpace(newTitle) ? existing.Title : newTitle!,
            Description = newDesc is null ? existing.Description : newDesc,
            Priority = newPriority ?? existing.Priority,
            DueDate = newDue ?? existing.DueDate,
            Status = existing.Status,      
            CreatedAt = existing.CreatedAt 
        };

        service.Update(toUpdate);

        ConsoleUi.Success("Tarefa atualizada!");
        ConsoleUi.Pause();
    }

    static void MarkDone(ITaskService service)
    {
        Console.Clear();
        ConsoleUi.Title("Concluir tarefa");

        var id = ConsoleUi.ReadGuid("Informe o ID da tarefa: ");
        service.MarkAsDone(id);

        ConsoleUi.Success("Tarefa concluída!");
        ConsoleUi.Pause();
    }

    static void DeleteTask(ITaskService service)
    {
        Console.Clear();
        ConsoleUi.Title("Excluir tarefa");

        var id = ConsoleUi.ReadGuid("Informe o ID da tarefa: ");

        var confirm = ConsoleUi.ReadString("Tem certeza? (s/n): ");
        if (!confirm.Equals("s", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Operação cancelada.");
            ConsoleUi.Pause();
            return;
        }

        service.Delete(id);

        ConsoleUi.Success("Tarefa excluída!");
        ConsoleUi.Pause();
    }
}
