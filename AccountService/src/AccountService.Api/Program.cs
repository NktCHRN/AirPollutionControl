using AccountService.Api;
using AccountService.Application;
using AccountService.Domain;
using AccountService.Infrastructure;
using AspNetCore.Middlewares;
using FluentValidation;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDomainServices()
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddWebApiServices(builder.Configuration);
ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DEV_CORS");
}
else
{
    app.UseCors("PROD_CORS");
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
