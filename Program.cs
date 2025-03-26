using EventManagementWebApp.Data;
using EventManagementWebApp.Models;
using EventManagementWebApp.Repositories;
using EventManagementWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventManagementWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddScoped<IEventService, EventService>()
                .AddScoped<IEventRepository, EventRepository>()
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<IRegistirationRepository, RegistirationRepository>()
                .AddScoped<IFileStorageService, FileStorageService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });

            builder.Services.AddHttpContextAccessor();



            var app = builder.Build();

            await SeedRoles(app.Services);
            await SeedUsers(app.Services);

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Event}/{action=Index}/{id?}");

            


            app.Run();
        }


        private static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            string[] roles = { "Admin", "Organizer", "Member" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
                }
            }
        }

        private static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var adminEmail = "admin@example.com";
            var adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Abdelrahman",
                LastName = "Adel",
                DateJoined = DateTime.UtcNow
            };

            if (userManager.Users.All(u => u.Email != adminEmail))
            {
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var memberEmail = "member@example.com";
            var memberUser = new User
            {
                UserName = memberEmail,
                Email = memberEmail,
                FirstName = "John",
                LastName = "Doe",
                DateJoined = DateTime.UtcNow
            };

            if (userManager.Users.All(u => u.Email != memberEmail))
            {
                var result = await userManager.CreateAsync(memberUser, "Member@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(memberUser, "Member");
                }
            }

            var organizerEmail = "organizer@example.com";
            var organizerUser = new User
            {
                UserName = organizerEmail,
                Email = organizerEmail,
                FirstName = "Jane",
                LastName = "Smith",
                DateJoined = DateTime.UtcNow
            };

            if (userManager.Users.All(u => u.Email != organizerEmail))
            {
                var result = await userManager.CreateAsync(organizerUser, "Organizer@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(organizerUser, "Organizer");
                }
            }
        }

    }
}
