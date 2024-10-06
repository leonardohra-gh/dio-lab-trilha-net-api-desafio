using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrilhaApiDesafio.Exceptions
{
    public class DataVaziaException : Exception
    {
        public DataVaziaException() : base("A data da tarefa não pode ser vazia"){}
    }
}