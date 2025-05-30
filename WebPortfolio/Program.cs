using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebPortfolio
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // ➕ Authentication xidmətini əlavə et
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login/Index"; // login səhifəsi
                    options.AccessDeniedPath = "/Login/AccessDenied"; // optional
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // ➕ Authentication və Authorization middleware-ləri əlavə et
            app.UseAuthentication(); // BUNU ƏLAVƏ ETMƏK VACİBDİR!
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Menu}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
