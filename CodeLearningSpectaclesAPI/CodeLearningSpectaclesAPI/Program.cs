
using CodeLearningSpectaclesAPI.Auth;
using CodeLearningSpectaclesAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeLearningSpectaclesAPI
{
    public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
      DotNetEnv.Env.Load();
      builder.Services.AddControllers();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen(); builder.Services.AddDbContext<CodeLearningDbContext>(options => options.UseNpgsql(
        "Server=" + Environment.GetEnvironmentVariable("SERVER")
        + ";Port=" + Environment.GetEnvironmentVariable("PORT")
        + ";User Id=" + Environment.GetEnvironmentVariable("USER_ID")
        + ";Password=" + Environment.GetEnvironmentVariable("PASSWORD")
        + ";Database=" + Environment.GetEnvironmentVariable("DATABASE") + ";"
      ));

            var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();

      app.UseAuthorization();

      //// Include authentication middleware for api
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
