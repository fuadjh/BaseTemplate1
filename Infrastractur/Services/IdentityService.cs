using Application.Features.Identity.Command;
using Application.Interfaces;
using Common.RequestsDto;
using Common.Wrapper;
using Infrastructure.Identity;
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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<ResponseWrapper<string>> RegisterAsync(RegisterUserRequest command)
        {
            var existingUser = await _userManager.FindByEmailAsync(command.email);
            if (existingUser != null)
                return new ResponseWrapper<string>().Failed("User with this email already exists.");

            var user = new ApplicationUser
            {
                UserName = command.fullName,
                Email = command.email
            };

            var result = await _userManager.CreateAsync(user, command.password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResponseWrapper<string>().Failed(errors);
            }

            var token = await _jwtTokenService.GenerateTokenAsync(user);
            return new ResponseWrapper<string>().Success(token, "User registered successfully");
        }

        public async Task<ResponseWrapper<string>> LoginAsync(LoginRequest command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            if (user == null)
                return new ResponseWrapper<string>().Failed("User not found");

            var result = await _signInManager.CheckPasswordSignInAsync(user, command.Password, false);
            if (!result.Succeeded)
                return new ResponseWrapper<string>().Failed("Invalid credentials");

            var token = await _jwtTokenService.GenerateTokenAsync(user);
            return new ResponseWrapper<string>().Success(token, "Login successful");
        }
    }
}