using API.Exceptions;
using Application.Behaviors;
using Application.Common.Interfaces;
using FluentValidation;
using Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers();

builder.Services.AddMediatR(Options=>
{
    Options.RegisterServicesFromAssembly(typeof(Application.IAssemblyMarker).Assembly);
});

builder.Services.AddValidatorsFromAssembly(typeof(Application.IAssemblyMarker).Assembly);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddDbContext<AppDBContext> (options => options.UseSqlite("Data Source = app.db"));

builder.Services.AddScoped<IAppDbContext, AppDBContext>();

var app = builder.Build();

app.UseExceptionHandler();

app.MapControllers();

app.Run();
