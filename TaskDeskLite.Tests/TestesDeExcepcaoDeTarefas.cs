using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using TaskDeskLite.Core;
using Xunit;
using Xunit.Sdk;
using StatusTarefa = TaskDeskLite.Core.TaskStatus;

/*
 ==========================================================================
  DIFERENÇA ENTRE BUSINESSRULEEXCEPTION E DOMAINVALIDATIONEXCEPTION
 ==========================================================================
 
 BusinessRuleException -> tem como foco principal regras específicas do negócio;
 DomainValidationException -> tem como foco principal dados inválidos dentro de um sistema.
 
 */





namespace TaskDeskLite.Tests
{
    public class TestesDeExcepcaoDeTarefas
    {
        private readonly TaskService _servico;

        public TestesDeExcepcaoDeTarefas()
        {
            // Cada teste começa com o serviço vazio
            _servico = new TaskService();
        }
        // TESTES DE TAREFAS PARA OS MÉTODOS CRIAR E EDITAR TAREFAS.
        [Fact]
        public void Criar_TituloMuitoCurto_DeveLancarExcecaoDeDominio()
        {
            var tarefa = new TaskItem
            {
                Title = ("2"),
                Description = "Descrição válida",
                Priority = TaskPriority.Medium
            };

            //Verificando se o código lança uma exceção específica, ou seja, se tentarmos criar essa tarefa 
            //com o título muito curto, retorna uma DomainValidationException. E aí o teste funciona
            var ex = Assert.Throws<DomainValidationException>(() => _servico.Create(tarefa));

            Assert.Equal("O título deve ter entre 3 e 40 caracteres.", ex.Message); //Assert.Equal verifica se a mensagem corresponde com a mesma do TaskValidator e aí nos retorna um ok
        }

        [Fact]
        public void Criar_TituloApenasComEspacos_DeveLancarExcecaoDeRegraDeNegocio()
        {
            var tarefa = new TaskItem { Title = "   " };

            Assert.Throws<BusinessRuleException>(() =>
                _servico.Create(tarefa));
        }

        [Fact]
        public void Criar_TituloComPalavraProibida_DeveLancarExcecaoDeDominio()
        {
            var tarefa = new TaskItem { Title = "Teste hack" };

            Assert.Throws<DomainValidationException>(() =>
                _servico.Create(tarefa));
        }

        [Fact]
        public void Criar_Tarefa_ComPrazoNoPassado_DeveRetornarUmaExcecaoDeRegraDeNegocio()
        {
            var tarefaPrazoPassado = DateTime.Today.AddDays(-1);
            var tarefa = new TaskItem();
            var ex = Assert.Throws<BusinessRuleException>(() =>
           _servico.Create(tarefa));

            Assert.Equal("A data de criação da tarefa deve ser igual ou após o dia atual.", ex.Message);
        }
        [Fact]
        //Testes de Exclusão - Aléxia
        public void DeletarTarefa_IDInexistente_LancaExcecao()
        {
            ////////// Cenário de validação: Tentar deletar uma tarefa inexistente //////////
            // Cria uma instância do serviço de tarefas que contém as operações de CRUD
            var taskService = new TaskDeskLite.Core.TaskService();

            // Cria três itens de tarefa temporários com dados de exemplo como objetos e atribui a variáveis
            var tarefa1 = new TaskItem
            {
                Title = "Tarefa 1",
                Description = "Descrição da Tarefa 1",
                Priority = TaskPriority.Low,
                DueDate = DateTime.Now.AddDays(3),
                Status = TaskDeskLite.Core.TaskStatus.Pending,
                CreatedAt = DateTime.Now
            };

            var tarefa2 = new TaskItem
            {
                Title = "Tarefa 2",
                Description = "Descrição da Tarefa 2",
                Priority = TaskPriority.High,
                DueDate = DateTime.Now.AddDays(5),
                Status = TaskDeskLite.Core.TaskStatus.Pending,
                CreatedAt = DateTime.Now
            };

            var tarefa3 = new TaskItem
            {
                Title = "Tarefa 3",
                Description = "Descrição da Tarefa 3",
                Priority = TaskPriority.Medium,
                DueDate = DateTime.Now.AddDays(2),
                Status = TaskDeskLite.Core.TaskStatus.Pending,
                CreatedAt = DateTime.Now
            };

            // Cria as tarefas no serviço
            taskService.Create(tarefa1);
            taskService.Create(tarefa2);
            taskService.Create(tarefa3);

            // Tenta deletar uma tarefa inexistente com um ID aleatório e verifica se a exceção NotFoundException é lançada
            Assert.Throws<NotFoundException>(() => taskService.Delete(Guid.NewGuid()));
        }
        [Fact]
        public void DeletarTarefa_IDInvalido_LancaExcecao()
        {
            ////////// Cenário de validação: Tentar deletar uma tarefa com ID inválido (negativo) //////////
            // Cria uma instância do serviço de tarefas que contém as operações de CRUD
            var taskService = new TaskDeskLite.Core.TaskService();

            // Tenta deletar a tarefa com ID inválido e verifica se a exceção BusinessRuleException é lançada
            Assert.Throws<BusinessRuleException>(() => taskService.Delete(Guid.Empty));
        }
        [Fact]
        public void DeletarTarefa_ListaVazia_LancaExcecao()
        {
            ////////// Cenário de validação: Tentar deletar uma tarefa em uma lista vazia //////////
            // Cria uma instância do serviço de tarefas que abriga as operações de CRUD
            var taskService = new TaskDeskLite.Core.TaskService();

            // Tenta deletar uma tarefa em uma lista vazia e verifica se a exceção NotFoundException é lançada
            Assert.Throws<NotFoundException>(() => taskService.Delete(Guid.NewGuid()));
        }
        //public void Criar_PrazoNoPassado_DeveLancarExcecaoDeDominio()
        //{
        //    var tarefa = new TaskItem
        //    {
        //        Title = "Prazo inválido",
        //        DueDate = DateTime.Today.AddDays(-1)
        //    };

        //    Assert.Throws<DomainValidationException>(() =>
        //        _servico.Create(tarefa));
        //}

        //// TESTES DE ATUALIZAÇÃO


        //[Fact]
        //public void Atualizar_TarefaConcluida_DeveLancarExcecaoDeRegraDeNegocio()
        //{
        //    var tarefa = _servico.Create(new TaskItem { Title = "Original" });
        //    _servico.MarkAsDone(tarefa.Id);

        //    tarefa.Title = "Novo título";

        //    Assert.Throws<BusinessRuleException>(() =>
        //        _servico.Update(tarefa));
        //}

        //[Fact]
        //public void Atualizar_TarefaInexistente_DeveLancarExcecaoDeNaoEncontrado()
        //{
        //    var tarefa = new TaskItem
        //    {
        //        Id = Guid.NewGuid(),
        //        Title = "Não existe"
        //    };

        //    Assert.Throws<NotFoundException>(() =>
        //        _servico.Update(tarefa));
        //}


        //Teste de Conclusão - Italo
        [Fact]
        public void ConcluirTarefa_SemId_DeveLancarErro()
        {
            var taskService = new TaskDeskLite.Core.TaskService();
            var taskItem = new TaskDeskLite.Core.TaskItem();
            if (taskItem.Id == Guid.Empty)
            {
                Assert.Throws<ArgumentException>(() => taskService.MarkAsDone(taskItem.Id));
            }

        }
    }
}
