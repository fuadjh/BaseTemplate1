using Application.Repositories;
using Common.Requests;
using Common.Wrapper;
using Domain;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AccountHolders.Command
{
    public class CreateAccounHolderRequestCommand : IRequest<ResponseWrapper<int>>/// یک الگوی درخواست با کتابخانه مدیت آر ایجاد می شود که یک رکورد داده است  
    {
        public CreateAccountHolderDto CreateAccountHolderDto { get; set; }
       // یک پراپرتی از جنس رکوردی که قبلا در کامون تعریف کرده ایم می سازیم 
    }
    public class CreateAccountHolderCommandHandler : IRequestHandler<CreateAccounHolderRequestCommand, ResponseWrapper<int>>//پردازشگر دستور 
    {

        private readonly IUnitOfWork<int> _unitOfWork;
        public CreateAccountHolderCommandHandler(IUnitOfWork<int> unitOfWork)
        {
           _unitOfWork = unitOfWork;     
        }

        public async Task<ResponseWrapper<int>> Handle(CreateAccounHolderRequestCommand request, CancellationToken cancellationToken)
        {
           
           var accountHolder  =request.CreateAccountHolderDto.Adapt<AccountHolder>();

           await _unitOfWork.WriteRepositoryFor<AccountHolder>().AddAsync(accountHolder);
           await _unitOfWork.CommitAsync(cancellationToken);

           return new ResponseWrapper<int>().Success(accountHolder.Id, " Success Create account holder ");
        }
    }

}
