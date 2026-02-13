using Common.Enums;

namespace WebApi.Contracts.Requests
{
    public sealed record RegisterLmsUserRequest(
    string NationalCode,
    string FirstName,
    string LastName,
    string Email,
    string Mobile,
    UserType UserType
);
}
