using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace remitee.DTOs
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
