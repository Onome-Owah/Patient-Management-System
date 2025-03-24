using PMS.Web;
using PMS.Data.Services;
using PMS.Data.Repositories;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure Authentication / Authorisation via extension methods 
        builder.Services.AddCookieAuthentication();

        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            // Configure connection string for selected database in appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("Sqlite");
            if (string.IsNullOrEmpty(connectionString))
            {
                // Fallback to environment variable in Render
                connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            }
            options.UseSqlite(connectionString);
        });

        // Add UserService to DI   
        builder.Services.AddTransient<IUserService, UserServiceDb>();
        builder.Services.AddTransient<IMailService, SmtpMailService>();
        builder.Services.AddScoped<IPatientService, PatientServiceDb>();

        // ** Required to enable asp-authorize Taghelper **            
        builder.Services.AddHttpContextAccessor();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        else
        {
            using (var scope = app.Services.CreateScope())
            {
                Seeder.Seed(
                    scope.ServiceProvider.GetService<IUserService>(), 
                    scope.ServiceProvider.GetService<IPatientService>()
                );
            }
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // ** Render requires dynamic port handling **
        var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
        app.Urls.Add($"http://0.0.0.0:{port}");

        app.Run();
    }
}
