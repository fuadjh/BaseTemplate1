    using Blazored.SessionStorage; // حتماً این using را اضافه کنید
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Http; // حتماً این using را اضافه کنید
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

    // فرض بر این است که IHttpContextAccessor و ISessionStorageService تزریق شده‌اند
    namespace WebUi.Components.Pages.Auth
{
    public class CustomServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionStorageService _sessionStorage;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly IConfiguration _configuration; // برای خواندن Secret Key

        // برای نگهداری وضعیت احراز هویت فعلی
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomServerAuthenticationStateProvider(
            IHttpContextAccessor httpContextAccessor,
            ISessionStorageService sessionStorage,
            IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _sessionStorage = sessionStorage;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            _configuration = configuration;
        }

        // متد اصلی برای دریافت وضعیت احراز هویت
        // نوع بازگشتی باید Task<AuthenticationState> باشد
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // 1. ابتدا بررسی می‌کنیم که آیا کاربر از طریق کوکی (سشن ASP.NET Core) احراز هویت شده است یا خیر.
            // این حالت بعد از اولین لاگین موفق با JWT و صدور کوکی اتفاق می‌افتد.
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User.Identity?.IsAuthenticated == true)
            {
                _currentUser = httpContext.User;
                return new AuthenticationState(_currentUser);
            }

            // 2. اگر کاربر از طریق کوکی احراز هویت نشده، تلاش می‌کنیم توکن JWT را از SessionStorage بخوانیم.
            string jwtToken = await _sessionStorage.GetItemAsync<string>("jwtToken");

            if (!string.IsNullOrEmpty(jwtToken))
            {
                try
                {
                    // توکن را اعتبارسنجی و ClaimsPrincipal را ایجاد کنید
                    _currentUser = CreateClaimsPrincipalFromJwt(jwtToken);

                    // اگر توکن منقضی شده باشد یا نامعتبر باشد، CreateClaimsPrincipalFromJwt ممکن است یک ClaimsIdentity خالی برگرداند
                    if (_currentUser.Identity?.IsAuthenticated == true)
                    {
                        // اگر توکن معتبر بود، یک کوکی احراز هویت برای Blazor Server صادر کنید
                        await SignInUserWithCookie(_currentUser);
                        return new AuthenticationState(_currentUser);
                    }
                }
                catch (Exception ex)
                {
                    // در صورت بروز خطا (مثلاً توکن نامعتبر)، آن را لاگ کنید و ادامه دهید
                    Console.WriteLine($"Error validating JWT from SessionStorage: {ex.Message}");
                    // توکن نامعتبر را حذف کنید
                    await _sessionStorage.RemoveItemAsync("jwtToken");
                }
            }

            // اگر هیچ یک از روش‌های بالا موفق نبود، کاربر احراز هویت نشده است.
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            return new AuthenticationState(_currentUser);
        }

        // متد برای علامت‌گذاری کاربر به عنوان احراز هویت شده پس از لاگین موفق
        public async Task MarkUserAsAuthenticated(string jwtToken)
        {
            // 1. ClaimsPrincipal را از JWT ایجاد کنید
            var authenticatedUser = CreateClaimsPrincipalFromJwt(jwtToken);

            // 2. یک کوکی احراز هویت برای Blazor Server صادر کنید
            // این باعث می‌شود که در درخواست‌های بعدی به Blazor Server، HttpContext.User پر شود.
            await SignInUserWithCookie(authenticatedUser);

            // 3. به Blazor اطلاع دهید که وضعیت احراز هویت تغییر کرده است
            // این باعث رفرش UI می‌شود (مثلاً نمایش لینک‌های لاگین/لاگ‌اوت)
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(authenticatedUser)));
        }

        // متد برای علامت‌گذاری کاربر به عنوان لاگ‌اوت شده
        public async Task MarkUserAsLoggedOut()
        {
            // 1. کوکی احراز هویت را حذف کنید
            await SignOutUserFromCookie();
            // 2. JWT را از SessionStorage حذف کنید
            await _sessionStorage.RemoveItemAsync("jwtToken");

            // 3. وضعیت کاربر را به "احراز هویت نشده" تغییر دهید
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            _currentUser = anonymousUser;

            // 4. به Blazor اطلاع دهید که وضعیت احراز هویت تغییر کرده است
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
        }

        // متد کمکی برای ایجاد ClaimsPrincipal از JWT
        private ClaimsPrincipal CreateClaimsPrincipalFromJwt(string jwtToken)
        {
            if (string.IsNullOrWhiteSpace(jwtToken))
                return new ClaimsPrincipal(new ClaimsIdentity());

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(jwtToken); // فقط خواندن توکن بدون validate امضاء
            var claims = jwt.Claims.ToList();

            // ممکن است بخواهید claimهایی مثل nameidentifier, name یا roles را تبدیل کنید
            var identity = new ClaimsIdentity(claims, "jwt");
            return new ClaimsPrincipal(identity);
        }

        // متد کمکی برای صدور کوکی احراز هویت ASP.NET Core
        private async Task SignInUserWithCookie(ClaimsPrincipal principal)
        {
            if (_httpContextAccessor.HttpContext == null) return;

            var response = _httpContextAccessor.HttpContext.Response;
            if (response.HasStarted)
            {
                // پاسخ آغاز شده — نمی‌توان هدر ست کرد.
                // گزینه: لاگ کردن و خروج (UI با ClaimsPrincipal به‌روز می‌شود ولی کوکی ایجاد نمی‌شود)
                Console.WriteLine("Response already started — cannot set cookie here.");
                return;
            }

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                });
        }

        // متد کمکی برای حذف کوکی احراز هویت ASP.NET Core
        private async Task SignOutUserFromCookie()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }

}
