using Application.Features.AccountHolders.Queries;
using Application.Repositories;
using Common.Responses;
using Common.Wrapper;
using Domain;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Application.Features.AccountHolders.Queries
//{
//    public class GetAccuontHoldersQueries :IRequest<ResponseWrapper<List<AccountHolderResponse>>>
//    {
        
//    }

//}

//public class GetAccuontHoldersQueriesHandler : IRequestHandler<GetAccuontHoldersQueries, ResponseWrapper<List<AccountHolderResponse>>>
//{
//    private readonly IUnitOfWork<int> _unitOfWork;
//    public GetAccuontHoldersQueriesHandler(IUnitOfWork<int> unitOfWork)
//    {
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<ResponseWrapper<List<AccountHolderResponse>>> Handle(GetAccuontHoldersQueries request, CancellationToken cancellationToken)
//    {
//        var accountHolderInDb = await _unitOfWork.ReadRepositoryFor<AccountHolder>().GetAllAsync();

//        if (accountHolderInDb.Count>0)
//        {
//            return new ResponseWrapper<List<AccountHolderResponse>>()
//                    .Success(accountHolderInDb.Adapt<List<AccountHolderResponse>>());
//        }
//        return new ResponseWrapper<List<AccountHolderResponse>>().Failed("No account Holder were found");



//    }

    
//}
