using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models.Database;
using TrilhaApiDesafio.Models.DTO.Request;
using TrilhaApiDesafio.Models.DTO.Response;

namespace TrilhaApiDesafio.Services
{
    public class TarefaService
    {
        private readonly OrganizadorContext _context;

        public TarefaService(OrganizadorContext context)
        {
            _context = context;
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