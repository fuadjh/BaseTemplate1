using Domain.Common;
using Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class Email : ValueObject
    {
        public string Address { get; }

        protected Email() { } // EF Core

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new DomainException("Email is required.");

            address = address.Trim();

            if (!IsValidEmail(address))
                throw new DomainException("Invalid email format.");

            Address = address;
        }

        private bool IsValidEmail(string email)
        {
            // ساده ولی قابل اعتماد
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address.ToLower(); // Case-insensitive equality
        }

        public override string ToString() => Address;
    }
}
