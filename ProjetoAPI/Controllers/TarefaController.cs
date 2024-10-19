using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Exceptions;
using TrilhaApiDesafio.Models;
using TrilhaApiDesafio.Models.Database;
using TrilhaApiDesafio.Models.DTO.Request;
using TrilhaApiDesafio.Models.DTO.Response;
using TrilhaApiDesafio.Services;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly TarefaService _tarefaService;
        private readonly OrganizadorContext _context;
        private readonly static string MensagemTarefaNaoEncontrada = "Não encontrado";
        private readonly static string MensagemDetalhadaTarefaNaoEncontrada = "Não foram encontradas tarefas com esse critério";
            
        public TarefaController(OrganizadorContext context, TarefaService tarefaService)
        {
            _context = context;
            _tarefaService = tarefaService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            Tarefa tarefa = await _tarefaService.FindTarefaAsync(id);
            return tarefa == null? NotFound(new ApiResponse<Tarefa>(null, "Tarefa não encontrada")) : Ok(new ApiResponse<Tarefa>(tarefa));
        }

        [HttpPost("ObterTodos")]
        public async Task<IActionResult> ObterTodos(QueryParameters parameters)
        {
            parameters.CheckValidity();
            IQueryable<Tarefa> queryTarefas = _tarefaService.GetQueryable();
            queryTarefas = _tarefaService.ApplySorting(queryTarefas, parameters);
            
            PagedResponse<Tarefa> todasTarefas = await _tarefaService.ApplyPaginationAsync(queryTarefas, parameters);

            return Ok(new ApiResponse<PagedResponse<Tarefa>>(todasTarefas));
        }

        [HttpPost("ObterPorTitulo")]
        public async Task<IActionResult> ObterPorTitulo(QueryParameters parameters, string titulo)
        {
            parameters.CheckValidity();
            IQueryable<Tarefa> queryTarefas = _tarefaService.GetQueryable();
            queryTarefas = queryTarefas.Where(t => t.Titulo.Contains(titulo));
            queryTarefas = _tarefaService.ApplySorting(queryTarefas, parameters);

            PagedResponse<Tarefa> tarefasComTitulo = await _tarefaService.ApplyPaginationAsync(queryTarefas, parameters);

            return Ok(new ApiResponse<PagedResponse<Tarefa>>(tarefasComTitulo));
        }

        [HttpPost("ObterPorData")]
        public async Task<IActionResult> ObterPorData(QueryParameters parameters, DateTime data)
        {
            parameters.CheckValidity();
            IQueryable<Tarefa> queryTarefas = _tarefaService.GetQueryable();
            queryTarefas = queryTarefas.Where(x => x.Data.Date == data.Date);
            queryTarefas = _tarefaService.ApplySorting(queryTarefas, parameters);

            PagedResponse<Tarefa> tarefasNaData = await _tarefaService.ApplyPaginationAsync(queryTarefas, parameters);

            return Ok(new ApiResponse<PagedResponse<Tarefa>>(tarefasNaData));
        }

        [HttpPost("ObterPorStatus")]
        public async Task<IActionResult> ObterPorStatus(QueryParameters parameters, StatusTarefa status)
        {
            parameters.CheckValidity();
            IQueryable<Tarefa> queryTarefas = _tarefaService.GetQueryable();
            queryTarefas = queryTarefas.Where(x => x.Status == status);
            queryTarefas = _tarefaService.ApplySorting(queryTarefas, parameters);

            PagedResponse<Tarefa> tarefaComStatus = await _tarefaService.ApplyPaginationAsync(queryTarefas, parameters);
            return Ok(new ApiResponse<PagedResponse<Tarefa>>(tarefaComStatus));
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                throw new DataVaziaException();

            Tarefa tarefaCriada = await _tarefaService.CriarTarefaAsync(tarefa);

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, new ApiResponse<Tarefa>(tarefaCriada));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Tarefa tarefa)
        {
            Tarefa tarefaBanco = await _tarefaService.FindTarefaAsync(id);

            if (tarefaBanco == null)
                return NotFound(new ApiResponse<Tarefa>(null, MensagemTarefaNaoEncontrada, MensagemDetalhadaTarefaNaoEncontrada, false));
            
            if (tarefa.Data == DateTime.MinValue)
                throw new DataVaziaException();

            Tarefa tarefaAtualizada = await _tarefaService.AtualizarTarefaAsync(tarefa, tarefaBanco);

            return Ok(new ApiResponse<Tarefa>(tarefaAtualizada));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound(new ApiResponse<Tarefa>(null, MensagemTarefaNaoEncontrada, MensagemDetalhadaTarefaNaoEncontrada, false));

            _context.Remove(tarefaBanco);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Estou criando essa função para popular o banco de dados com exemplos aleatórios, apenas para testar outras funções
        [HttpGet("PopularBancoDeDados")]
        public async Task<IActionResult> PopularBanco(int quantidade)
        {
            List<Tarefa> tarefasCriadas = await _tarefaService.CriarTarefasAleatorias(quantidade);

            return CreatedAtAction(nameof(ObterTodos), new { parameters = new QueryParameters() }, new ApiResponse<List<Tarefa>>(tarefasCriadas));
        }
    }
}
