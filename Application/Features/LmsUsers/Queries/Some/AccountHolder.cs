using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests
{
    public record CreateAccountHolderDto(string FirstName , string LastName , DateTime BirthDate ,
        string ContactNumber , string Email);
    public record UpdateAccountHolderDto(int id , string FirstName, string LastName, DateTime BirthDate,
       string ContactNumber, string Email);



}
