using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RequestsDto
{
    public class UserProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string FatherName { get; set; }
        public string AvatarUrl { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public UserType UserType { get; set; }
    }

}
