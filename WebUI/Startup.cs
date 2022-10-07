using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using BlazorTable;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using WebUI.Areas.Identity;
using WebUI.Data;
using WebUI.Email;
using WebUI.Factory;
using WebUI.Services;

namespace WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext<ApplicationUser>>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext<ApplicationUser>>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();
            services.AddTransient<IEmailSender, MailKitEmailSender>();
            services.AddScoped<UploadLogo>();
            services.Configure<MailKitEmailSenderOptions>(options =>
            {
                options.Host_Address = Configuration["SMTP:Address"];
                options.Host_Port = Convert.ToInt32(Configuration["SMTP:Port"]);
                options.Host_Username = Configuration["SMTP:Account"];
                options.Host_Password = Configuration["SMTP:Password"];
                options.Sender_Email = Configuration["SMTP:SenderEmail"];
                options.Sender_Name = Configuration["SMTP:SenderName"];
            });             // ServiceFactory is injected into razor components. The factory itself news up a DB Context on a per-request basis.
            services.AddScoped<ServiceFactory>();

            // Enable notification toasts
            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomCenter;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });

            // Server Side Blazor doesn't register HttpClient by default
            // https://github.com/SamProf/MatBlazor/issues/235
            if (!services.Any(x => x.ServiceType == typeof(HttpClient)))
            {
                // Setup HttpClient for server side in a client side compatible fashion
                services.AddScoped<HttpClient>(s =>
                {
                    // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.
                    var uriHelper = s.GetRequiredService<NavigationManager>();
                    return new HttpClient
                    {
                        BaseAddress = new Uri(uriHelper.BaseUri)
                    };
                });
            }

            // Report export
            services.AddScoped<FileService>();
            services.AddScoped<PdfService>();

            // Now required in BlazorTable 1.14
            services.AddBlazorTable();
            
            // Add blazorise with Bootstrap and Font Awesome Icons.
            services.AddBlazorise(options => { options.Immediate = true; })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();

            // Recommended for .NET 6, see https://github.com/aspnet/Announcements/issues/432
            services.AddDatabaseDeveloperPageExceptionFilter();

            // Invalidate a user session immediately if their security stamp is updated (which happens when user is disabled)
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // See https://github.com/aspnet/Announcements/issues/432
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

     
            // Create a scope so that scoped dependencies can be resolved.
            var scope = app.ApplicationServices.CreateScope();

#if DEBUG
            // Create database if none exists and apply migrations (in development only).
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext<ApplicationUser>>();
            context.Database.Migrate();
#endif

            // Create roles.
            CreateRoles(scope.ServiceProvider).Wait();
        }

        /// <summary>
        /// Creates the Admin and User Roles on startup if they do not exist, and assigns Admin User specified in config to Admin Role.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "SuperAdmin", "Admin", "User", "Disabled" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles if they do not exist
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            try
            {
                //create Admin User with email and username specified in config file (appsettings.json)
                var adminUser = new ApplicationUser
                {
                    //Ensure you have these values in your appsettings.json file, in the AppSettings section (not created by default)
                    // A bug in Microsoft's Identity code requires that the Username is the Email
                    UserName = Configuration["AppSettings:AdminUserEmail"],
                    Email = Configuration["AppSettings:AdminUserEmail"],
                    EmailConfirmed = true,
                };
                string adminUserPassword = Configuration["AppSettings:AdminUserPassword"];
                // Check if the Admin User has already been created
                var exists = await UserManager.FindByEmailAsync(adminUser.Email);
                // Create Admin User if they do not exist
                if (exists == null)
                {
                    var createadminUser = await UserManager.CreateAsync(adminUser, adminUserPassword);
                    if (createadminUser.Succeeded)
                    {
                        // Add Admin User to SuperAdmin Role
                        await UserManager.AddToRoleAsync(adminUser, "SuperAdmin");
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("Failed to create Admin User: make sure Admin credentials are defined in appsettings.json.", ex);
            }
        }
    }
}
