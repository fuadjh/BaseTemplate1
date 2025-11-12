using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RequestsDto
{
    public record RegisterUserRequest (

         string email ,
         string fullName ,
         string password 
   );

}
