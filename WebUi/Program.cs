using WebUi.Components;
using Microsoft.AspNetCore.Components.Authorization;
using WebUi.Components.Pages.Auth;
using WebUi.Components.Pages.Auth.WebUI.Client.Auth;

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

            // برای احراز هویت
            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomServerAuthenticationStateProvider>(); // تغییر این خط
            builder.Services.AddAuthorization(); // اطمینان از افزودن Authorization

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
