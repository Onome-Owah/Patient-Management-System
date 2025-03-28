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
        //builder.Services.AddPolicyAuthorisation();

        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            // Configure connection string for selected database in appsettings.json

            options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
            //options.UseMySql(builder.Configuration.GetConnectionString("MySql"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql")));
            //options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
            //options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
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

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

        //app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // ** configure cors to allow full cross origin access to any webapi end points **
        //app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        // ** turn on authentication/authorisation **
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}