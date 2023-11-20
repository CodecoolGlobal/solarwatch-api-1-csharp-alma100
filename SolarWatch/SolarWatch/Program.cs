using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SolarWatch.Context;
using SolarWatch.Repository;
using SolarWatch.Service;
using SolarWatch.Service.Auth;

public class Program  //teszt miatt publikussá kellett tenni.
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

        builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSingleton<IGeocodeProvider, GeocodeProvider>();
        builder.Services.AddSingleton<ISunsetDataProvider, SunsetDataProvider>();
        builder.Services.AddSingleton<IJsonProcessor, JsonProcessor>();
        builder.Services.AddSingleton<ICityRepository, CityRepository>();
        builder.Services.AddSingleton<ISunriseSunsetRepository, SunriseSunsetRepository>();
        builder.Services.AddDbContext<UsersContext>();
        builder.Services
            .AddScoped<IAuthService,
                AuthService>(); // az élettartamot a kérési szintre korlátozza. Azaz minden HTTP kérés során egy új példányt hoz létre a regisztrált szolgáltatásból
// .
        builder.Services.AddScoped<ITokenService, TokenServices>();
        var movieApiKey = builder.Configuration["test"];
        Console.WriteLine(movieApiKey);
//ConfigurationManager configuration = builder.Configuration;
        ConfigureSwagger();
        AddTokens(movieApiKey);
        AddIdentity();

        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication(); //-> ezt hozzá kell adni fontos! felelős a hitelesítés beállításáért és a beérkező tokenek érvényességének ellenőrzéséért
//UseAuthentication() --> előre kell a másik elé!!
        app.UseAuthorization(); // felelős a felhasználók jogosultságainak ellenőrzéséért.

        app.MapControllers();
        AddAdmin();
        AddRoles();
        app.Run();


        #region ConfigureSwagger

        void ConfigureSwagger()
        {
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

        }

        #endregion

        #region JWToken

        void AddTokens(string secret )
        {
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "apiWithAuthBackend",
                        ValidAudience = "apiWithAuthBackend",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(secret)
                        ),
                    };
                });
        }

        #endregion

        #region Role

        void AddRoles()
        {
            using var scope = app.Services.CreateScope(); // RoleManager is a scoped service, therefore we need a scope instance to access it
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var tAdmin = CreateAdminRole(roleManager);
            tAdmin.Wait();

            var tUser = CreateUserRole(roleManager);
            tUser.Wait();
        }

        async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("admin")); //The role string should better be stored as a constant or a value in appsettings
        }

        async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("user")); //The role string should better be stored as a constant or a value in appsettings
        }

        #endregion

        #region AddIdentity

        void AddIdentity()
        {
            builder.Services
                .AddIdentityCore<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddRoles<IdentityRole>() //Enable Identity roles 
                .AddEntityFrameworkStores<UsersContext>(); //EF segítségével a megadott (UsersContext) adatbázisban tároljuk
        } 
        #endregion

        #region admin

        void AddAdmin()
        {
            var tAdmin = CreateAdminIfNotExists();
            tAdmin.Wait();
        }

        async Task CreateAdminIfNotExists()
        {
            using var scope = app.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var adminInDb = await userManager.FindByEmailAsync("admin@admin.com");
            if (adminInDb == null)
            {
                var admin = new IdentityUser { UserName = "admin", Email = "admin@admin.com" };
                var adminCreated = await userManager.CreateAsync(admin, "admin123");

                if (adminCreated.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
        #endregion
    }
}

