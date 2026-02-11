using Common.Enums;

using Domain.Contracts;
using Domain.Users.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Exceptions;

namespace Domain.Users
{
    public class LmsUser : AggregateRoot<Guid>
    {
        public Guid IdentityUserId { get; private set; }

        public UserProfile Profile { get; private set; }
        public Email Email { get; private set; }
        public bool EmailConfirmed { get; private set; }

        public Mobile Mobile { get; private set; }
        public bool MobileConfirmed { get; private set; }

        public UserType UserType { get; private set; }
        public bool IsActive { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? LastLoginAt { get; private set; }

        protected LmsUser() { } // EF Core

        private LmsUser(
            Guid identityUserId,
            UserProfile profile,
            Email email,
            Mobile mobile,
            UserType userType)
        {
            if (identityUserId == Guid.Empty)
                throw new DomainException("IdentityUserId is required");

            IdentityUserId = identityUserId;
            Profile = profile ?? throw new DomainException("Profile is required");
            Email = email ?? throw new DomainException("Email is required");
            Mobile = mobile ?? throw new DomainException("Mobile is required");
            UserType = userType;

            EmailConfirmed = false;
            MobileConfirmed = false;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;

            Id = Guid.NewGuid();
        }

        public static LmsUser Create(
            Guid identityUserId,
            UserProfile profile,
            Email email,
            Mobile mobile,
            UserType userType)
        {
            return new LmsUser(identityUserId, profile, email, mobile, userType);
        }

        // -------- Behaviors --------
        public void ConfirmEmail() => EmailConfirmed = true;
        public void ConfirmMobile() => MobileConfirmed = true;
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
        public void UpdateLastLogin() => LastLoginAt = DateTime.UtcNow;
        public void ChangeUserType(UserType userType) => UserType = userType;
        public void UpdateProfile(UserProfile profile)
        {
            if (profile == null) throw new DomainException("Profile cannot be null");
            Profile = profile;
        }

        public void ChangeEmail(Email email)
        {
            if (email == null) throw new DomainException("Email cannot be null");
            Email = email;
            EmailConfirmed = false;
        }

        public void ChangeMobile(Mobile mobile)
        {
            if (mobile == null) throw new DomainException("Mobile cannot be null");
            Mobile = mobile;
            MobileConfirmed = false;
        }
    }

}
