using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RequestsDto
{
    public class RegisterUserRequest {

        public string email { get; set; }
        public string fullName { get; set; }
        public string password { get; set; }
    }

}
