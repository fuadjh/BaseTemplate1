using Blazored.SessionStorage;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using WebUi.Components;
using WebUi.Components.Pages.Auth;

//using WebUi.Components.Pages.Auth.WebUI.Client.Auth;

namespace WebUi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            //===============================================
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // تنظیمات کوکی (اختیاری):
        options.LoginPath = "/"; // مسیری که Blazor به آن هدایت می کند اگر کاربر احراز هویت نشده باشد.
        options.LogoutPath = "/logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // مدت زمان اعتبار کوکی
        options.SlidingExpiration = true; // با هر درخواست، زمان اعتبار کوکی تمدید می شود.
        // سایر تنظیمات...
    });
            // برای احراز هویت
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomServerAuthenticationStateProvider>(); // تغییر این خط
            builder.Services.AddAuthorization(); // اطمینان از افزودن Authorization
            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddDistributedMemoryCache();
            // برای ارتباط با WebAPI
            builder.Services.AddHttpClient("WebApi", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["WebApiSettings:BaseUrl"] ?? throw new InvalidOperationException("WebApi BaseUrl not configured."));
            });

            // برای دسترسی به HttpContext و Session در Blazor Server
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // مدت زمان نگهداری سشن
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // این کوکی برای عملکرد برنامه ضروری است
            });

            //===============================================


            var app = builder.Build();

            // ...
            app.UseSession(); // باید قبل از UseAuthentication و UseAuthorization باشد

            app.UseAuthentication();
            app.UseAuthorization();

            // ...

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
