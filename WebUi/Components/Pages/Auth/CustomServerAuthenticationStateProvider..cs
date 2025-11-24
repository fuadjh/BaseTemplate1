using static System.Net.Mime.MediaTypeNames;

namespace WebUi.Components.Pages.Auth
{
    using Application.Authentication.Commands.LoginUser; // برای AuthenticationResult
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
    using Microsoft.AspNetCore.Http; // برای دسترسی به HttpContext
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt; // برای خواندن JWT
    using System.Net.Http.Headers;
    using System.Security.Claims;

    namespace WebUI.Client.Auth
    {
        public class CustomServerAuthenticationStateProvider : AuthenticationStateProvider
        {
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IHttpClientFactory _httpClientFactory;
            private readonly ProtectedSessionStorage _sessionStorage; // یا ProtectedLocalStorage

            public CustomServerAuthenticationStateProvider(
                IHttpContextAccessor httpContextAccessor,
                IHttpClientFactory httpClientFactory,
                ProtectedSessionStorage sessionStorage) // یا ProtectedLocalStorage
            {
                _httpContextAccessor = httpContextAccessor;
                _httpClientFactory = httpClientFactory;
                _sessionStorage = sessionStorage;
            }

            public override async Task GetAuthenticationStateAsync()
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext?.User.Identity?.IsAuthenticated == true)
                {
                    // کاربر از طریق کوکی احراز هویت شده است (مثلا بعد از ورود موفق)
                    return new AuthenticationState(httpContext.User);
                }

                // اگر کاربر احراز هویت نشده، تلاش می‌کنیم ببینیم توکن JWT در سشن ذخیره شده؟
                var result = await _sessionStorage.GetAsync("jwtToken");
                if (result.Success && !string.IsNullOrEmpty(result.Value?.ToString()))
                {
                    var jwtToken = result.Value.ToString();
                    var principal = CreateClaimsPrincipalFromJwt(jwtToken);
                    return new AuthenticationState(principal);
                }

                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            public async Task MarkUserAsAuthenticated(AuthenticationResult authResult)
            {
                var principal = CreateClaimsPrincipalFromJwt(authResult.Token);

                // ایجاد کوکی احراز هویت برای ASP.NET Core
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // به یاد داشتن کاربر
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // مثلاً اعتبار کوکی
                };

                await _httpContextAccessor.HttpContext!.SignInAsync(
                    "Cookies", // Scheme name (همان که در Program.cs برای AddAuthentication("Cookies") استفاده شده)
                    principal,
                    authProperties);

                // ذخیره JWT در سشن برای استفاده در HttpClient (برای تماس با WebApi)
                await _sessionStorage.SetAsync("jwtToken", authResult.Token);

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
            }

            public async Task MarkUserAsLoggedOut()
            {
                await _httpContextAccessor.HttpContext!.SignOutAsync("Cookies"); // Scheme name
                await _sessionStorage.DeleteAsync("jwtToken");
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
            }

            // یک متد کمکی برای ساخت ClaimsPrincipal از JWT
            private ClaimsPrincipal CreateClaimsPrincipalFromJwt(string jwtToken)
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                var claims = new List(token.Claims);
                // اضافه کردن Claim برای نام کاربری (Sub یا Name) اگر وجود نداشته باشد
                if (!claims.Any(c => c.Type == ClaimTypes.Name))
                {
                    var nameClaim = token.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "name");
                    if (nameClaim != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Name, nameClaim.Value));
                    }
                }
                // اضافه کردن Claim برای نام کاربری (Sub یا NameIdentifier) اگر وجود نداشته باشد
                if (!claims.Any(c => c.Type == ClaimTypes.NameIdentifier))
                {
                    var nameIdentifierClaim = token.Claims.FirstOrDefault(c => c.Type == "sub");
                    if (nameIdentifierClaim != null)
                    {
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifierClaim.Value));
                    }
                }


                var identity = new ClaimsIdentity(claims, "jwt"); // "jwt" می تواند نام Scheme باشد
                return new ClaimsPrincipal(identity);
            }
        }
    }
}
