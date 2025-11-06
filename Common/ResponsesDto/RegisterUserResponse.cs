using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ResponsesDto
{
    public record RegisterUserResponse(bool IsSuccess, string? UserId, string? ErrorMessage);
}
