using ApiRest.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiRestContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sql-string")));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CoreRules", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
   //c.SwaggerEndpoint("/swagger.yaml", "ApiRest v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("CoreRules");

app.Run();
