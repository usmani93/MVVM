using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Model
{
    public class Request
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}
