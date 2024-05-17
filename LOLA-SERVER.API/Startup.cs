using Microsoft.OpenApi.Models;

namespace LOLA_SERVER.API
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
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

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "LOLA_SERVER API",
                    Version = "v1",
                    Description = "An API to manage LOLA_SERVER operations",
                    Contact = new OpenApiContact
                    {
                        Name = "LOLA Support",
                        Email = "support@lolaservice.com",
                        Url = new Uri("https://lolaservice.com/support"),
                    }
                });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthorization();
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
}
