using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IdentityModels
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public ApplicationRole Role { get; set; } = default!;
        public int PermissionId { get; set; }
        public ApplicationPermission Permission { get; set; } = default!;
    }

}
