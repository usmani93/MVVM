using System;
using System.Collections.Generic;
using System.Text;

namespace ChatAPI.Models
{
    public class User
    {
        public string connectionId { get; set; }
        public int userID { get; set; }
        public string userName { get; set; }
        public string userToken { get; set; }
    }
}
