using Domain.Common;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Users.ValueObjects
{
    public class Mobile : ValueObject
    {
        public string Number { get; }

        protected Mobile() { } // EF Core

        public Mobile(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new DomainException("Mobile number is required.");

            number = number.Trim();

            if (!IsValidMobile(number))
                throw new DomainException("Invalid mobile number format.");

            Number = number;
        }

        private bool IsValidMobile(string number)
        {
            // فرمت ایرانی: 09xxxxxxxxx
            var pattern = @"^09\d{9}$";
            return Regex.IsMatch(number, pattern);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Number;
        }

        public override string ToString() => Number;
    }
}
