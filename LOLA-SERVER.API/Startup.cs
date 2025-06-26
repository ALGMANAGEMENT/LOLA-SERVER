using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using LOLA_SERVER.API.Interfaces.Services;
using LOLA_SERVER.API.Services;
using LOLA_SERVER.API.Services.MessagingService;
using LOLA_SERVER.API.Services.NotificationsService;
using LOLA_SERVER.API.Services.PetServicesService;
using LOLA_SERVER.API.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
    }

    private void InitializeFirebase()
    {
        try
        {
            // Verificar si Firebase ya está inicializado
            if (FirebaseApp.DefaultInstance == null)
            {
                var projectId = Configuration["Firebase:ProjectId"] ?? "lola-manager";
                var credentialPath = Configuration["Firebase:CredentialsPath"] ?? "Utils/Credentials/Firebase-Credentials.json";
                
                if (!File.Exists(credentialPath))
                {
                    throw new FileNotFoundException($"No se encontró el archivo de credenciales en: {credentialPath}");
                }

                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(credentialPath),
                    ProjectId = projectId
                });
                
                Console.WriteLine($"Firebase inicializado exitosamente para el proyecto: {projectId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inicializando Firebase: {ex.Message}");
            // En desarrollo, permitir que la aplicación continúe
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                throw;
            }
        }
    }

    private void ConfigureAuthentication(IServiceCollection services)
    {

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = "https://securetoken.google.com/lola-manager";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuers = new[]
                    {
                        "https://securetoken.google.com/lola-manager",
                        "https://accounts.google.com"
                    },
                    ValidateAudience = true,
                    ValidAudiences = new[]
                    {
                          "lola-manager",
                        "358545804776-8givt8sn6ni9lb1taf18svjc5nov59jt.apps.googleusercontent.com",
                        "358545804776-49en63dntddtsvaimgiujl26vm253f09.apps.googleusercontent.com"
                    },
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Reduce la tolerancia predeterminada en el tiempo de expiración del token
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                        logger.LogError("Authentication failed. Exception: {Exception}", context.Exception);
                        logger.LogError("Token: {Token}", context.Request.Headers["Authorization"]);
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers["Authorization"].ToString();
                        if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
                        {
                            context.NoResult();
                            return Task.CompletedTask;
                        }
                        context.Token = token.Substring("Bearer ".Length).Trim();
                        return Task.CompletedTask;
                    }
                };
            });
    }


    private void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<FirebaseMessagingService>();
    }

    private void RegisterControllers(IServiceCollection services)
    {
        services.AddControllers()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                     options.JsonSerializerOptions.PropertyNamingPolicy = null; 
                 });
    }

    private void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "LOLA API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Por favor ingrese el token JWT",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                    },
                    new string[] {}
                }
            });
            
        });
    }


    public void DependencyInyection(IServiceCollection services)
    {
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IPetServicesService, PetServicesService>();
        services.AddScoped<IUsersService,UsersService>();
        services.AddSingleton<INotificationsService, NotificationsService>();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        ConfigureCors(services);
        InitializeFirebase();
        ConfigureAuthentication(services);
        RegisterServices(services);
        RegisterControllers(services);
        ConfigureSwagger(services);
        DependencyInyection(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors("AllowAll");

        app.Use(async (context, next) =>
        {
            await next.Invoke();

            if (context.Response.StatusCode == 401)
            {
                logger.LogWarning("Unauthorized request: " + context.Request.Path);
            }
        });

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "LOLA_SERVER API V1");
            c.RoutePrefix = "swagger";
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
