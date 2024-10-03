using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
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
        public IActionResult ObterPorId(int id)
        {
            Tarefa tarefa = _context.Tarefas.Find(id);
            return tarefa == null? NotFound() : Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            List<Tarefa> todasTarefas = _context.Tarefas.ToList();
            return todasTarefas.Count == 0? NoContent() : Ok(todasTarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefasComTitulo = _context.Tarefas.Where(t => t.Titulo.Contains(titulo));
            return tarefasComTitulo.Any() ? Ok(tarefasComTitulo) : NoContent() ;
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefasNaData = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            return tarefasNaData.Any() ? Ok(tarefasNaData) : NoContent();
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefaComStatus = _context.Tarefas.Where(x => x.Status == status);
            return tarefaComStatus.Any() ? Ok(tarefaComStatus) : NoContent();
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // TODO: Adicionar a tarefa recebida no EF e salvar as mudanças (save changes)
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
            // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
            return NoContent();
        }
    }
}
