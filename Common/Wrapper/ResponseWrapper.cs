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
        public List<string> Massages { get; set; }
        public T Data { get; set; }
        public ResponseWrapper<T> Success (T data , string massage=null)
        {
            IsSuccess = true;
            Massages = [massage];
                Data = data;
            return this;
        }

        public ResponseWrapper<T> Failed( string massage )
        {
            IsSuccess = false;
            Massages = [massage];
           
            return this;
        }
    }
}
