using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjetoAPI.Exceptions;
using TrilhaApiDesafio.Models.Database;

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
            if(PageNumber < 1)
                throw new ParametroInvalidoException("PageNumber", PageNumber.ToString(), "O número da página deve ser um número positivo");
            if(!IsValidProperty(SortBy))
                throw new ParametroInvalidoException("SortBy", SortBy, $"O parâmetro de ordenação deve ser uma coluna da tabela Tarefa: {PropertiesInTarefa()}");
        }

        private static bool IsValidProperty(string propertyName)
        {
            return typeof(Tarefa).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) != null;
        }

        private static string PropertiesInTarefa()
        {
            List<string> strProperties = new();
            PropertyInfo[] properties = typeof(Tarefa).GetProperties();
            foreach(PropertyInfo property in properties)
                strProperties.Add(property.Name);

            return string.Join(", ", strProperties);
        }
    }
}