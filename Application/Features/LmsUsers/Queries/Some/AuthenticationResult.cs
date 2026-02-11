using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ResponsesDto
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; } // اگر دارید
        public string UserId { get; set; }
        public string Email { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string>  Errors { get; set; } = new List<string>();
    }
}
