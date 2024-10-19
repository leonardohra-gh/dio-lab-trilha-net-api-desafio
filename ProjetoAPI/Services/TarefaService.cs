using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;
using TrilhaApiDesafio.Models.Database;
using TrilhaApiDesafio.Models.DTO.Request;
using TrilhaApiDesafio.Models.DTO.Response;

namespace TrilhaApiDesafio.Services
{
    public class TarefaService
    {
        private readonly OrganizadorContext _context;
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

        public TarefaService(OrganizadorContext context)
        {
            _context = context;
        }

        public async Task<Tarefa> FindTarefaAsync(int id)
        {
            return await _context.Tarefas.FindAsync(id);
        } 

        public async Task<Tarefa> CriarTarefaAsync(Tarefa tarefa)
        {
            await _context.Tarefas.AddAsync(tarefa);
            await _context.SaveChangesAsync();
            return tarefa;
        } 

        public async Task<Tarefa> AtualizarTarefaAsync(Tarefa tarefaPassada, Tarefa tarefaBanco)
        {
            tarefaBanco.Titulo = tarefaPassada.Titulo;
            tarefaBanco.Descricao = tarefaPassada.Descricao;
            tarefaBanco.Data = tarefaPassada.Data;
            tarefaBanco.Status = tarefaPassada.Status;
            
            _context.Tarefas.Update(tarefaBanco);
            await _context.SaveChangesAsync();

            return tarefaBanco;
        }

        public async Task<List<Tarefa>> CriarTarefasAleatorias(int quantidade)
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
            
            return tarefasCriadas;
        }

        public IQueryable<Tarefa> ApplySorting(IQueryable<Tarefa> initialQuery, QueryParameters sortingParameters)
        {
            return sortingParameters.IsAcending? initialQuery.OrderBy(t => EF.Property<object>(t, sortingParameters.SortBy)) :
                                                 initialQuery.OrderByDescending(t => EF.Property<object>(t, sortingParameters.SortBy));
        }

        public async Task<PagedResponse<Tarefa>> ApplyPaginationAsync(IQueryable<Tarefa> query, QueryParameters paginationParameters)
        {
            int totalRecords = await query.CountAsync();
            List<Tarefa> data = totalRecords == 0? new() :
                                                    await query
                                                            .Skip((paginationParameters.PageNumber - 1)*paginationParameters.PageSize)
                                                            .Take(paginationParameters.PageSize)
                                                            .ToListAsync();
            
            PagedResponse<Tarefa> pagedResponse = new(data, paginationParameters.PageNumber, paginationParameters.PageSize, totalRecords);

            return pagedResponse;
        }

        public IQueryable<Tarefa> GetQueryable()
        {
            return _context.Tarefas.AsQueryable();
        }
    }
}