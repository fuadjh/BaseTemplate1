using Application.Interfaces;
using Common.RequestsDto.Users;
using Common.ResponsesDto.Users;
using Common.Wrapper;
using Domain.Common.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LmsUsers.Command.CheckNationalCode
{
    public class CheckNationalCodeCommand: IRequest<ResponseWrapper<CheckNationalCodeResult>>
    {
        public CheckNationalCodeRequest Request { get; set; } = default!;
    }
   



}
