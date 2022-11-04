using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Data.Infrastructure.Entities
{
    public class DataResult
    {
        public DataResult(bool isSucceed, string message)
        {
            IsSucceed = isSucceed;
            Message = message;
        }
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
    }
}
