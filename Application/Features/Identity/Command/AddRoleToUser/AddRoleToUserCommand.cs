using Application.Interfaces;
using Common.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Identity.Command.AddRoleToUser
{
    public record AddRoleToUserCommand(string Email, string RoleName) : IRequest<ResponseWrapper<string>>;

   
}
