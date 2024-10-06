using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Exceptions;
using TrilhaApiDesafio.Models;
using TrilhaApiDesafio.Models.Database;
using TrilhaApiDesafio.Models.DTO.Response;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;
        private readonly static string MensagemTarefaNaoEncontrada = "Não encontrado";
        private readonly static string MensagemDetalhadaTarefaNaoEncontrada = "Não foram encontradas tarefas com esse critério";

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var a = 1/id;
            Tarefa tarefa = await _context.Tarefas.FindAsync(id);
            return tarefa == null? NotFound(new ApiResponse<Tarefa>(null, "Tarefa não encontrada")) : Ok(new ApiResponse<Tarefa>(tarefa));
        }

        [HttpGet("ObterTodos")]
        public async Task<IActionResult> ObterTodos()
        {
            List<Tarefa> todasTarefas = await _context.Tarefas.ToListAsync();
            return todasTarefas.Any()? Ok(new ApiResponse<List<Tarefa>>(todasTarefas)) : 
                                       NotFound(new ApiResponse<List<Tarefa>>(new List<Tarefa>()));
        }

        [HttpGet("ObterPorTitulo")]
        public async Task<IActionResult> ObterPorTitulo(string titulo)
        {
            var tarefasComTitulo = await _context.Tarefas.Where(t => t.Titulo.Contains(titulo)).ToListAsync();
            return tarefasComTitulo.Any() ? Ok(new ApiResponse<List<Tarefa>>(tarefasComTitulo)) : 
                                            NotFound(new ApiResponse<List<Tarefa>>(new List<Tarefa>()));
        }

        [HttpGet("ObterPorData")]
        public async Task<IActionResult> ObterPorData(DateTime data)
        {
            var tarefasNaData = await _context.Tarefas.Where(x => x.Data.Date == data.Date).ToListAsync();
            return tarefasNaData.Any() ? Ok(new ApiResponse<List<Tarefa>>(tarefasNaData)) : 
                                         NotFound(new ApiResponse<List<Tarefa>>(new List<Tarefa>()));
        }

        [HttpGet("ObterPorStatus")]
        public async Task<IActionResult> ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefaComStatus = await _context.Tarefas.Where(x => x.Status == status).ToListAsync();
            return tarefaComStatus.Any() ? Ok(new ApiResponse<List<Tarefa>>(tarefaComStatus)) : 
                                           NotFound(new ApiResponse<List<Tarefa>>(new List<Tarefa>()));
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                throw new DataVaziaException();

            await _context.Tarefas.AddAsync(tarefa);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, new ApiResponse<Tarefa>(tarefa));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound(new ApiResponse<Tarefa>(null, "Tarefa não encontrada"));
            
            if (tarefa.Data == DateTime.MinValue)
                throw new DataVaziaException();

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;
            
            _context.Tarefas.Update(tarefaBanco);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<Tarefa>(tarefaBanco));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound(new ApiResponse<Tarefa>(null, "Tarefa não encontrada"));

            _context.Remove(tarefaBanco);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
