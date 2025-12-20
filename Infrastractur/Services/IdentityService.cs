using Application.Features.Identity.Command;
using Application.Interfaces;
using Common.RequestsDto;
using Common.RequestsDto.Users;
using Common.ResponsesDto;
using Common.ResponsesDto.Users;
using Common.Wrapper;
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
                return  ResponseWrapper<string>.Failed(errors);
            }

            var token = await _jwtTokenService.GenerateTokenAsync(user);
            return ResponseWrapper<string>.Success(token, "User registered successfully");
        }

        public async Task<ResponseWrapper<string>> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return  ResponseWrapper<string>.Failed("Role already exists.");

            var result = await _roleManager.CreateAsync(new ApplicationRole { Name = roleName });

            return result.Succeeded
                ?  ResponseWrapper<string>.Success(roleName, "Role created successfully.")
                :ResponseWrapper<string>.Failed("Role creation failed.");
        }

        public async Task<ResponseWrapper<string>> AddRoleToUserAsync(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return  ResponseWrapper<string>.Failed("User not found.");

            if (!await _roleManager.RoleExistsAsync(roleName))
                return  ResponseWrapper<string>.Failed("Role not found.");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded
                ?  ResponseWrapper<string>.Success(roleName, "Role added successfully.")
                :  ResponseWrapper<string>.Failed("Adding role failed.");
        }

        public async Task<ResponseWrapper<AuthenticationResult>> LoginAsync(LoginRequest command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                return ResponseWrapper<AuthenticationResult>.Failed("کاربر یافت نشد");

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: false);
            if (!signInResult.Succeeded)
                return ResponseWrapper<AuthenticationResult>.Failed("ایمیل یا رمز عبور اشتباه است");

            var token = await _jwtTokenService.GenerateTokenAsync(user);

            var authResult = new AuthenticationResult
            {
                Token = token,
                // اگر بخوای اطلاعات بیشتر هم برگردونی:
                // UserId = user.Id,
                // UserName = user.UserName,
                // ExpiresAt = DateTime.UtcNow.AddHours(1)
            };

            return ResponseWrapper<AuthenticationResult>.Success(authResult, "ورود با موفقیت انجام شد");
        }

        public async Task<ResponseWrapper<CheckNationalCodeResult>> CheckNationalCodeAsync(CheckNationalCodeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NationalCode))
                return  ResponseWrapper<CheckNationalCodeResult>.Failed("کد ملی الزامی است");

            // (در صورت نیاز: اعتبارسنجی الگوریتمی کد ملی)

            var identityUser = await _userManager
                .FindByNameAsync(request.NationalCode);

            if (identityUser == null)
            {
                return ResponseWrapper<CheckNationalCodeResult>.Success(
                    new CheckNationalCodeResult { Exists = false }
                );
            }

         

            return ResponseWrapper<CheckNationalCodeResult>.Success(
                new CheckNationalCodeResult
                {
                    Exists = true,
                    IdentityUserId = identityUser.Id
                  
                });
        }
    }
}