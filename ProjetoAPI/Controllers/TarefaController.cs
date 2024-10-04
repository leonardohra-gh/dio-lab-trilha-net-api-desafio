using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Exceptions;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            Tarefa tarefa = await _context.Tarefas.FindAsync(id);
            return tarefa == null? NotFound() : Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public async Task<IActionResult> ObterTodos()
        {
            List<Tarefa> todasTarefas = await _context.Tarefas.ToListAsync();
            return todasTarefas.Count == 0? NoContent() : Ok(todasTarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public async Task<IActionResult> ObterPorTitulo(string titulo)
        {
            var tarefasComTitulo = await _context.Tarefas.Where(t => t.Titulo.Contains(titulo)).ToListAsync();
            return tarefasComTitulo.Any() ? Ok(tarefasComTitulo) : NoContent() ;
        }

        [HttpGet("ObterPorData")]
        public async Task<IActionResult> ObterPorData(DateTime data)
        {
            var tarefasNaData = await _context.Tarefas.Where(x => x.Data.Date == data.Date).ToListAsync();
            return tarefasNaData.Any() ? Ok(tarefasNaData) : NoContent();
        }

        [HttpGet("ObterPorStatus")]
        public async Task<IActionResult> ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefaComStatus = await _context.Tarefas.Where(x => x.Status == status).ToListAsync();
            return tarefaComStatus.Any() ? Ok(tarefaComStatus) : NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                throw new DataVaziaException();

            await _context.Tarefas.AddAsync(tarefa);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                throw new DataVaziaException();

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;
            tarefaBanco.Status = tarefa.Status;
            
            _context.Tarefas.Update(tarefaBanco);
            await _context.SaveChangesAsync();

            return Ok(tarefaBanco);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Remove(tarefaBanco);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
