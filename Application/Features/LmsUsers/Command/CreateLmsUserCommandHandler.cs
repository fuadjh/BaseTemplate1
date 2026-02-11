using Application.Interfaces.Repositories;
using Common.Enums;
using Common.Wrapper;
using Domain.Users;
using Domain.Users.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LmsUsers.Command
{
    public record CreateLmsUserCommand(
      Guid IdentityUserId,
      string FirstName,
      string LastName,
      string NasionalCode,
      string Email,
      string Mobile,
      UserType UserType  ) : IRequest<ResponseWrapper<Guid>>;


    public class CreateLmsUserCommandHandler   : IRequestHandler<CreateLmsUserCommand, ResponseWrapper<Guid>>
    {
        private readonly ILmsUserRepository _lmsUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateLmsUserCommandHandler(
            ILmsUserRepository lmsUserRepository,
            IUnitOfWork unitOfWork)
        {
            _lmsUserRepository = lmsUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseWrapper<Guid>> Handle(
            CreateLmsUserCommand request,
            CancellationToken cancellationToken)
        {
            var profile = new UserProfile(request.FirstName, request.LastName,request.NasionalCode);
            var email = new Email(request.Email);
            var mobile = new Mobile(request.Mobile);

            var lmsUser = LmsUser.Create(
                request.IdentityUserId,
                profile,
                email,
                mobile,
                request.UserType
            );

            await _lmsUserRepository.AddAsync(lmsUser, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return ResponseWrapper<Guid>.Success(lmsUser.Id, "LMS user created successfully");
        }
    }


}
