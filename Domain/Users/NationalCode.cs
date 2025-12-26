using Domain.Common;
using Domain.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public sealed class NationalCode : ValueObject
{
    public string Value { get; }

    private NationalCode(string value)
    {
        Value = value;
    }

    public static NationalCode Create(string value)
    {
        if (!IsValid(value))
            throw new DomainException("کد ملی نامعتبر است");

        return new NationalCode(value);
    }

    public static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (!Regex.IsMatch(value, @"^\d{10}$"))
            return false;

        var check = int.Parse(value[9].ToString());
        var sum = Enumerable.Range(0, 9)
            .Sum(i => int.Parse(value[i].ToString()) * (10 - i));

        var remainder = sum % 11;
        return (remainder < 2 && check == remainder)
            || (remainder >= 2 && check == 11 - remainder);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

