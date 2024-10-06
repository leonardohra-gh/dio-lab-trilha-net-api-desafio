using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrilhaApiDesafio.Models.DTO.Response
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public string DetailedMessage { get; set; }
        public bool Success { get; set; }

        public ApiResponse(T data, string message = "", string detailedMessage = "", bool success = true)
        {
            Data = data;
            Message = message;
            DetailedMessage = detailedMessage; 
            Success = success;
        }
    }
}