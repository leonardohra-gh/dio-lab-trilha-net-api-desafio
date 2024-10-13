using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAPI.Exceptions
{
    public class ParametroInvalidoException : Exception
    {
        public ParametroInvalidoException(string parametro, string valor, string condicao) : 
        base($"O parâmetro passado possui um valor inválido. \nParametro: {parametro}\nValor: {valor}\nCondicao: {condicao}"){}
    }
}