using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ResponsesDto.Users
{
    public class CheckNationalCodeResult
    {
        public bool Exists { get; set; }
        public int? IdentityUserId { get; set; }

      
    }
}
