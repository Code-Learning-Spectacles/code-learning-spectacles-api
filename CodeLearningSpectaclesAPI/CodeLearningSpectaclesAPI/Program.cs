using CodeLearningSpectaclesAPI.Auth;
using CodeLearningSpectaclesAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CodeLearningSpectaclesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Retrieve secrets from GitHub Secrets
            var dbServer = Environment.GetEnvironmentVariable("TF_VAR_AWS_RDS_ENDPOINT");
            var dbPort = Environment.GetEnvironmentVariable("TF_VAR_DB_PORT");
            var dbUser = Environment.GetEnvironmentVariable("TF_VAR_DB_USERNAME");
            var dbPassword = Environment.GetEnvironmentVariable("TF_VAR_DB_PASSWORD");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");

            // Build connection string
            var connectionString = $"Server={dbServer};Port={dbPort};User Id={dbUser};Password={dbPassword};Database={dbName};";

            builder.Services.AddDbContext<CodeLearningDbContext>(options => options.UseNpgsql(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            // Include authentication middleware for api
            app.UseWhen(x => (x.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase)),
                builder =>
                {
                    builder.UseMiddleware<RequestFilter>();
                });

            app.MapControllers();

            app.Run();
        }
    }
}
