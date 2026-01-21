namespace TaskDeskLite.Core;

public static class TaskValidator
{
    private static readonly string[] ForbiddenWords = { "hack", "drop", "delete" };

    public static void ValidateForCreateOrUpdate(TaskItem task)
    {
        if (task is null) throw new DomainValidationException("Tarefa inválida.");

        var title = (task.Title ?? "").Trim();

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainValidationException("Título é obrigatório.");

        if (title.Length < 3 || title.Length > 40)
            throw new DomainValidationException("Título deve ter entre 3 e 40 caracteres.");

        if (ForbiddenWords.Any(w => title.Contains(w, StringComparison.OrdinalIgnoreCase)))
            throw new DomainValidationException("Título contém termo não permitido.");

        if (!Enum.IsDefined(typeof(TaskPriority), task.Priority))
            throw new DomainValidationException("Prioridade inválida.");

        if (task.Description is not null && task.Description.Length > 200)
            throw new DomainValidationException("Descrição deve ter no máximo 200 caracteres.");

        if (task.DueDate.HasValue)
        {
            var due = task.DueDate.Value.Date;
            var today = DateTime.Now.Date;
            if (due < today)
                throw new DomainValidationException("Prazo não pode ser no passado.");
        }
    }
}
