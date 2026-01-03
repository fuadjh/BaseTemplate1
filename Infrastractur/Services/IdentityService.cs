using Application.Features.Identity.Command;
using Application.Interfaces;
using Common.RequestsDto;
using Common.RequestsDto.Users;
using Common.ResponsesDto;
using Common.ResponsesDto.Users;
using Common.Wrapper;
using Domain.Common.Exceptions;
using Domain.Users;
using Infrastructure.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;


        public IdentityService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<ResponseWrapper<string>> RegisterAsync(RegisterUserRequest command)
        {
            var existingUser = await _userManager.FindByEmailAsync(command.email);
            if (existingUser != null)
                return ResponseWrapper<string>.Failed("User with this email already exists.");

            var user = new ApplicationUser
            {
                UserName = command.fullName,
                Email = command.email
            };

            var result = await _userManager.CreateAsync(user, command.password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseWrapper<string>.Failed(errors);
            }

            var token = await _jwtTokenService.GenerateTokenAsync(user);
            return ResponseWrapper<string>.Success(token, "User registered successfully");
        }

        public async Task<ResponseWrapper<string>> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return ResponseWrapper<string>.Failed("Role already exists.");

            var result = await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });

            return result.Succeeded
                ? ResponseWrapper<string>.Success(roleName, "Role created successfully.")
                : ResponseWrapper<string>.Failed("Role creation failed.");
        }

        public async Task<ResponseWrapper<string>> AddRoleToUserAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return ResponseWrapper<string>.Failed("User not found.");

            if (!await _roleManager.RoleExistsAsync(roleName))
                return ResponseWrapper<string>.Failed("Role not found.");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded
                ? ResponseWrapper<string>.Success(roleName, "Role added successfully.")
                : ResponseWrapper<string>.Failed("Adding role failed.");
        }

        public async Task<AuthenticationResult> LoginAsync(LoginRequest command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                throw new DomainException("کاربر یافت نشد");

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
                throw new DomainException("ایمیل یا رمز عبور اشتباه است");

            var token = await _jwtTokenService.GenerateTokenAsync(user);

            var authResult = new AuthenticationResult
            {
                Token = token,
                // اگر بخوای اطلاعات بیشتر هم برگردونی:
                // UserId = user.Id,
                // UserName = user.UserName,
                // ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            return authResult;
        }

        public async Task<CheckNationalCodeResult> CheckNationalCodeAsync(CheckNationalCodeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NationalCode))
                throw new DomainException("کد ملی الزامی است");

            if (!NationalCode.IsValid(request.NationalCode))
                throw new DomainException("کد ملی نامعتبر است");

            var identityUser = await _userManager.FindByNameAsync(request.NationalCode);

            if (identityUser == null)
            {
                return new CheckNationalCodeResult
                {
                    Exists = false
                };
            }

            return new CheckNationalCodeResult
            {
                Exists = true,
                IdentityUserId = identityUser.Id
            };
        }

        public async Task<Guid> GetOrCreateUserByNationalCodeAsync(string nationalCode)
        {
            var user = await _userManager.FindByNameAsync(nationalCode);

            if (user != null)
                return user.Id;

            var newUser = new ApplicationUser
            {
                UserName = nationalCode,
                Email = null
            };

            var result = await _userManager.CreateAsync(newUser, nationalCode);

            if (!result.Succeeded)
                throw new ApplicationException("Identity user creation failed");

            return newUser.Id;
        }
    }
}