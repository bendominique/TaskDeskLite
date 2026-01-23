using System;
using Xunit;
using System.Net.NetworkInformation;
using TaskDeskLite.Core;
using StatusTarefa = TaskDeskLite.Core.TaskStatus;

namespace TaskDeskLite.Tests
{
    public class TestesDeValidacaoDeTarefas
    {
        private readonly TaskService _servico;

        public TestesDeValidacaoDeTarefas()
        {
            _servico = new TaskService();
        }
        [Fact]
        //Testes de Cadastro e Edição - Stephany

        public void Criar_TarefaValida_DeveCriarComSucesso()
        {
            // busque dados válidos
            var tarefa = new TaskItem
            {
                Title = "Tarefa válida",
                Priority = TaskPriority.Medium
            };

            //  ação executada
            var resultado = _servico.Create(tarefa);

            // Assert = verificações
            Assert.NotEqual(Guid.Empty, resultado.Id);              // ID gerado automaticamente
            Assert.Equal(StatusTarefa.Pending, resultado.Status);   // Status inicial Pendente
            Assert.Single(_servico.GetAll());                        // Salva em memória
        }

        [Fact]
        //Testes de Exclusão - Aléxia
        public void DeletarTarefa_IDExiste_RemoverDaLista()
        {
            ////////// Cenário de validação: Deletar uma tarefa existente pelo seu ID //////////

            // Cria uma instância do serviço de tarefas que contém as operações de CRUD
            var taskService = new TaskDeskLite.Core.TaskService();

            // Cria um item de tarefa temporário com dados de exemplo
            var tarefa = new TaskItem
            {
                Title = "Tarefa de Teste para Deleção",
                Description = "Tarefa criada para testar a funcionalidade de deleção.",
                Priority = TaskPriority.Medium,
                DueDate = DateTime.Now.AddDays(7),
                Status = TaskDeskLite.Core.TaskStatus.Pending,
                CreatedAt = DateTime.Now
            };

            // Cria a tarefa no serviço e armazena na variável
            var tarefaCriada = taskService.Create(tarefa);

            // Deleta a tarefa criada por meio do ID da tarefa
            taskService.Delete(tarefaCriada.Id);

            // Verifica se a lista de tarefas está vazia após a deleção
            // (Valor esperado, valor obtido e a contagem pós a deleção)
            Assert.Equal(0, taskService.GetAll().Count());
        }
    }
    }

