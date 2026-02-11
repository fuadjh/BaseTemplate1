using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LmsUsers.Queries.CheckNationalCode
{
    public class CheckNationalCodeResult
    {
        public bool Exists { get; set; }
        public Guid? IdentityUserId { get; set; }

    }
}
