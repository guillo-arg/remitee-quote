using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace remitee.Helpers
{
    public class AppException : Exception
    {
        public AppException(string msg, int statusCode = 500) : base(msg)
        {
            StatusCode = statusCode;
        }
        public int StatusCode { get; }
    }
}
