using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Interfaces
{
    public interface IAuthenticateUser
    {
        public string AuthenticateUserWithJWT(string userName);
    }
}
