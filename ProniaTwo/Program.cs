using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProniaTwo.Context;
using ProniaTwo.Models;

namespace ProniaTwo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(option =>
            {

                option.UseSqlServer(builder.Configuration.GetConnectionString("Default")); 
            });


            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = true;


                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = true;

                options.Lockout.MaxFailedAccessAttempts = 3;

                options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromHours(3);
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            var app = builder.Build();

            app.UseStaticFiles();

            app.UseRouting();
           
                app.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );
           

            app.MapDefaultControllerRoute();
            app.Run();
        }
    }
}
