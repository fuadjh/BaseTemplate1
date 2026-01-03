using Application.Interfaces;
using Application.Interfaces.Repositories;
using Common.Enums;
using Common.Wrapper;
using Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.LmsUsers.Command
{
    public record RegisterLmsUserCommand(
    string NationalCode,
    UserProfile Profile,
    Email Email,
    Mobile Mobile,
    UserType UserType
) : IRequest<ResponseWrapper<Guid>>;



    public class RegisterLmsUserCommandHandler : IRequestHandler<RegisterLmsUserCommand, ResponseWrapper<Guid>>
    {
        private readonly IIdentityService _identityService;
        private readonly ILmsUserRepository _lmsUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterLmsUserCommandHandler(
            IIdentityService identityService,
            ILmsUserRepository lmsUserRepository,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _lmsUserRepository = lmsUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseWrapper<Guid>> Handle(
            RegisterLmsUserCommand request,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Identity
            var identityUserId =
                await _identityService.GetOrCreateUserByNationalCodeAsync(request.NationalCode);

            // 2️⃣ LMS
            var lmsUser = await _lmsUserRepository
                .GetByIdentityUserIdAsync(identityUserId, cancellationToken);

            if (lmsUser == null)
            {
                lmsUser = LmsUser.Create(
                    identityUserId,
                    request.Profile,
                    request.Email,
                    request.Mobile,
                    request.UserType
                );

                await _lmsUserRepository.AddAsync(lmsUser, cancellationToken);
            }
            else
            {
                lmsUser.UpdateProfile(request.Profile);
                lmsUser.ChangeEmail(request.Email);
                lmsUser.ChangeMobile(request.Mobile);
                lmsUser.ChangeUserType(request.UserType);

                await _lmsUserRepository.UpdateAsync(lmsUser, cancellationToken);
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return ResponseWrapper<Guid>.Success(lmsUser.Id, "ثبت کاربر با موفقیت انجام شد");
        }
    }

}
