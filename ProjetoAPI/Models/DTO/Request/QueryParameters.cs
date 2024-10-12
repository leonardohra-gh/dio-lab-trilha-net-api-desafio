using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrilhaApiDesafio.Models.DTO.Request
{
    public class QueryParameters
    {
        public string SortBy { get; set; } = "Id";
        public bool IsAcending { get; set; } = true;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public void CheckValidity()
        {
            
        }
    }
}