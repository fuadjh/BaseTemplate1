using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Wrapper
{
    public class ResponseWrapper<T>
    {
        public bool IsSuccess { get; set; }
        public List<string> Messages { get; set; } = new();
        public T? Data { get; set; }

        public static ResponseWrapper<T> Success(T data, string? message = null)
        {
            return new ResponseWrapper<T>
            {
                IsSuccess = true,
                Data = data,
                Messages = message != null ? new List<string> { message } : new()
            };
        }

        public static ResponseWrapper<T> Failed(string message)
        {
            return new ResponseWrapper<T>
            {
                IsSuccess = false,
                Data = default,
                Messages = new List<string> { message }
            };
        }
    }
}