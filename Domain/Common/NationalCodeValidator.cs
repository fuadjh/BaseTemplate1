using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public static class NationalCodeValidator
    {
        public static bool IsValid(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length != 10)
                return false;

            var digits = code.Select(c => int.Parse(c.ToString())).ToArray();
            var check = digits[9];
            var sum = Enumerable.Range(0, 9)
                .Sum(i => digits[i] * (10 - i));

            var remainder = sum % 11;
            return remainder < 2
                ? check == remainder
                : check == 11 - remainder;
        }
    }

}
