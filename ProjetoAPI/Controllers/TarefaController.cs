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
        private readonly static string[] verbos = new string[]{"Amar", "Comer", "Partir", "Falar", "Estudar", "Cantar", "Correr", "Vender", "Beber", 
        "Escrever", "Andar", "Ficar", "Chegar", "Agir", "Gostar", "Brincar", "Abrir", "Viver", "Comprar", "Jogar", "Corrigir", "Passar", 
        "Receber", "Viajar", "Pensar", "Conhecer", "Assistir", "Aprender", "Dançar", "Começar", "Trabalhar", "Encontrar", "Precisar", 
        "Deixar", "Dividir", "Levar", "Dever", "Colorir", "Voltar", "Chamar", "Voar", "Pular", "Entender", "Passear", "Nascer", "Olhar", 
        "Parar", "Achar", "Usar", "Lembrar", "Dirigir", "Esquecer", "Morar", "Acordar", "Colocar", "Proteger", "Ganhar", "Feder", "Tomar", 
        "Esperar", "Conjugar", "Pegar", "Descer", "Morrer", "Entrar", "Fingir", "Mexer", "Contar", "Possuir", "Ajudar", "Bater", "Conversar", 
        "Cumprir", "Mandar", "Chover", "Chorar", "Continuar", "Suar", "Lavar", "Doer", "Doar", "Acabar", "Desejar", "Imprimir", "Existir", 
        "Decidir", "Sonhar", "Caminhar", "Pagar", "Falir", "Procurar", "Nadar", "Atender", "Acontecer", "Mudar", "Escolher", "Entreter", "Responder", 
        "Crescer", "Perdoar", "Fechar", "Almoçar", "Enviar", "Desistir", "Encher", "Tornar", "Participar", "Parecer", "Realizar", "Vencer", "Ligar", 
        "Permitir", "Avisar", "Terminar", "Sentar", "Demolir", "Tirar", "Buscar", "Apresentar", "Sofrer", "Entregar", "Criar", "Casar", "Levantar", 
        "Perceber", "Adquirir", "Perguntar", "Agradecer", "Convidar", "Aceitar", "Precaver", "Tentar", "Mostrar", "Ensinar", "Acreditar", "Informar", 
        "Visitar", "Pentear", "Matar", "Aparecer", "Aproveitar", "Beijar", "Respeitar", "Resolver", "Tocar", "Gritar", "Incluir", "Roer", "Zoar", "Surgir"}; 
        private readonly static string[] substantivos = new string[]{"amor", "café", "equipe", "explosão", "violão", "plástico", "creme", "martelo", "livros", 
        "lápis", "temor", "alumínio", "embarcação", "letra", "mel", "arquipélago", "roda", "discoteca", "universidade", "livraria", "cachorros", "chaves", 
        "manada", "pelo", "papai", "sofá", "felicidade", "berço", "teclado", "guardanapo", "escola", "monitor", "gente", "caneta", "garfo", "estatística", 
        "mapa", "fauna", "mensagem", "lima", "foguete", "rei", "edifício", "grama", "presidência", "folhas", "família", "colégio", "granizo", "pestana", 
        "lâmpada", "mão", "saúde", "flor", "música", "homem", "parafuso", "quarto", "veleiro", "vovó", "guerra", "pau", "satélite", "templo", "lentes", 
        "lapiseira", "prato", "nuvem", "governo", "garrafa", "castelo", "pinheiro", "riqueza", "verão", "pessoa", "planeta", "televisor", "luvas", "metal", 
        "poder", "elegância", "macaco", "camiseta", "mola", "petróleo", "cabide", "leilão", "debate", "tempo", "caderno", "ruído", "parede", "sorte", "ferramenta", 
        "cartas", "chocolate", "óculos", "impressora", "balas", "sala", "luzes", "angústia", "sapato", "bomba", "cacho", "olho", "gravata", "cerimônia", "alma", 
        "planta", "ódio", "mergulhador", "escritório", "persiana", "porta", "tio", "cadeira", "salada", "pedreira", "zoológico", "candidato", "deporte", 
        "recipiente", "jornais", "fotografia", "ave", "ferro", "refúgio", "calça", "religião", "carne", "neve", "tecla", "umidade", "tropa", "apartamento", 
        "celular", "tristeza", "hipopótamo", "vocabulário", "cama", "gás", "coral", "casaco", "discurso", "carros", "cinto", "entusiasmo", "famoso", "madeira", 
        "massa", "chão", "mala", "relógio", "deputado", "faca", "escuridão", "cadeado", "luz", "montanhas", "computador", "rádio", "laço", "quadro", "calor", 
        "jogo", "teatro", "clientela", "festa", "sono", "guarda-chuva"}; 
        private readonly static StatusTarefa[] status = new StatusTarefa[]{StatusTarefa.Pendente, StatusTarefa.Finalizado}; 
            

        public TarefaController(OrganizadorContext context, TarefaService tarefaService)
        {
            _context = context;
            _tarefaService = tarefaService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            Tarefa tarefa = await _context.Tarefas.FindAsync(id);
            return tarefa == null? NotFound(new ApiResponse<Tarefa>(null, "Tarefa não encontrada")) : Ok(new ApiResponse<Tarefa>(tarefa));
        }

        [HttpPost("ObterTodos")]
        public async Task<IActionResult> ObterTodos(QueryParameters parameters)
        {
            parameters.CheckValidity();
            IQueryable<Tarefa> queryTarefas = _tarefaService.GetQueryable();
            queryTarefas = _tarefaService.ApplySorting(queryTarefas, parameters);
            
            PagedResponse<Tarefa> pagedResponse = await _tarefaService.ApplyPaginationAsync(queryTarefas, parameters);

            return pagedResponse.Data.Any()? Ok(new ApiResponse<PagedResponse<Tarefa>>(pagedResponse)) : 
                                             NotFound(new ApiResponse<PagedResponse<Tarefa>>(pagedResponse, MensagemTarefaNaoEncontrada, MensagemDetalhadaTarefaNaoEncontrada, false));
        }

        [HttpPost("ObterPorTitulo")]
        public async Task<IActionResult> ObterPorTitulo(QueryParameters parameters, string titulo)
        {
            var tarefasComTitulo = await _context.Tarefas.Where(t => t.Titulo.Contains(titulo)).ToListAsync();
            return tarefasComTitulo.Any() ? Ok(new ApiResponse<List<Tarefa>>(tarefasComTitulo)) : 
                                            NotFound(new ApiResponse<List<Tarefa>>(null, MensagemTarefaNaoEncontrada, MensagemDetalhadaTarefaNaoEncontrada, false));
        }

        [HttpGet("ObterPorData")]
        public async Task<IActionResult> ObterPorData(DateTime data)
        {
            var tarefasNaData = await _context.Tarefas.Where(x => x.Data.Date == data.Date).ToListAsync();
            return tarefasNaData.Any() ? Ok(new ApiResponse<List<Tarefa>>(tarefasNaData)) : 
                                         NotFound(new ApiResponse<List<Tarefa>>(null, MensagemTarefaNaoEncontrada, MensagemDetalhadaTarefaNaoEncontrada, false));
        }

        [HttpGet("ObterPorStatus")]
        public async Task<IActionResult> ObterPorStatus(StatusTarefa status)
        {
            var tarefaComStatus = await _context.Tarefas.Where(x => x.Status == status).ToListAsync();
            return tarefaComStatus.Any() ? Ok(new ApiResponse<List<Tarefa>>(tarefaComStatus)) : 
                                           NotFound(new ApiResponse<List<Tarefa>>(null, MensagemTarefaNaoEncontrada, MensagemDetalhadaTarefaNaoEncontrada, false));
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
                return NotFound(new ApiResponse<Tarefa>(null, MensagemTarefaNaoEncontrada, MensagemDetalhadaTarefaNaoEncontrada, false));
            
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
                return NotFound(new ApiResponse<Tarefa>(null, MensagemTarefaNaoEncontrada, MensagemDetalhadaTarefaNaoEncontrada, false));

            _context.Remove(tarefaBanco);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Estou criando essa função para popular o banco de dados com exemplos aleatórios, apenas para testar outras funções
        [HttpGet("PopularBancoDeDados")]
        public async Task<IActionResult> PopularBanco(int quantidade)
        {
            Random random = new();
            List<Tarefa> tarefasCriadas = new();
            for(int i = 0; i < quantidade; i++)
            {
                string randomVerbo = verbos[random.Next(verbos.Length)];
                string randomSubstantivo = substantivos[random.Next(substantivos.Length)];
                string titulo = $"{randomVerbo} {randomSubstantivo}";
                StatusTarefa statusTarefa = status[random.Next(status.Length)];
                DateTime randomDate = DateTime.Now.AddDays(random.Next(-15, 31));
                Tarefa tarefa = new()
                {
                    Titulo = titulo,
                    Data = randomDate,
                    Descricao = "",
                    Status = statusTarefa
                };
                await _context.Tarefas.AddAsync(tarefa);
                tarefasCriadas.Add(tarefa);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterTodos), new { parameters = new QueryParameters() }, new ApiResponse<List<Tarefa>>(tarefasCriadas));
        }
    }
}
