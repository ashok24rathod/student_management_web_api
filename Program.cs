using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

using StudentManagementApi;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();


        builder.Services.AddSingleton<IConfiguration>(config);
        // Add services to the container.

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllHeaders",
                   builder =>
               {
                   builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
               });
        });

        builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseCors("AllowAllHeaders");

        // Configure the HTTP request pipeline.
        /*if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        } */

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}