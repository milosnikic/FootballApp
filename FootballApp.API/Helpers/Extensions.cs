using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;


namespace FootballApp.API.Helpers
{
    public static class Extensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My FootballApp API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My FootballApp API V1");
                c.RoutePrefix = string.Empty;

            });
            return app;
        }

        public static int CalculateAge(this DateTime date)
        {
            if (date.Year > DateTime.Now.Year)
            {
                return -1;
            }

            int age = DateTime.Now.Year - date.Year;
            if (date.AddYears(age) > DateTime.Today)
            {
                age--;
            }
            return age;
        }
    }
}