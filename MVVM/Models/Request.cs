using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVVM.Models
{
    public class Request
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string ConnectionId { get; set; }
    }
}
