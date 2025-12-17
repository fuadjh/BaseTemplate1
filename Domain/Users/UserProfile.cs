using Common.Enums;
using Domain.Common;
using Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class UserProfile : ValueObject
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string NationalCode { get; }
        public string? FatherName { get; }
        public Gender Gender { get; }
        public DateOnly? BirthDate { get; }
        public string? AvatarUrl { get; }

        // EF Core
        protected UserProfile() { }

        public UserProfile(
            string firstName,
            string lastName,
            string nationalCode,
            Gender gender,
            DateOnly? birthDate = null,
            string? avatarUrl = null)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainException("FirstName is required");

            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainException("LastName is required");

            if (!NationalCodeValidator.IsValid(nationalCode))
                throw new DomainException("Invalid national code");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            NationalCode = nationalCode.Trim();
            Gender = gender;
            BirthDate = birthDate;
            AvatarUrl = avatarUrl;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NationalCode;
        }
    }

}
