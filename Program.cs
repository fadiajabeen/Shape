using Microsoft.EntityFrameworkCore;
using Shape.Common;

namespace Shape
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Read the connection string and Add DbContext
            Constants.CONNECTION_STRING = builder.Configuration.GetConnectionString(Constants.CONNECTION_STRING_NAME);
            builder.Services.AddDbContext<Models.ShapeContext>(options => {
                options.UseSqlServer(Constants.CONNECTION_STRING);
            });

            //Register User Repository in DI container
            builder.Services.AddScoped<Contracts.IUserRepository, Repositories.UserRepository>();

            //Adding Session Configurations to remember user login
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options => {
                options.Cookie.Name = ".Shape.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Users}/{action=Index}/{id?}");

            app.Run();
        }
    }
}