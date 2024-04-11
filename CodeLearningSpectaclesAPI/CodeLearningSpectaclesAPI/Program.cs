
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

      builder.Services.AddControllers();
      // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();

      builder.Services.AddDbContext<CodeLearningDbContext>(options => options.UseNpgsql(
           "Server=" + "code-learning-postgres-db.c7klvipobgy8.eu-west-1.rds.amazonaws.com"
           + ";Port=" + "5432"
           + ";User Id=" + "CodeLearningSpectacles"
           + ";Password=" + "topsecretpassword"
           + ";Database=" + "codeLearningDB" + ";"
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
