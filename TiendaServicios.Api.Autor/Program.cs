using FluentValidation;
using MediatR;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Reflection;
using TiendaServicios.Api.Autor.Aplicacion;
using TiendaServicios.Api.Autor.Persistencia;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(); 

// han cambiado sustancialmente las librerías respecto a este curso
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Nuevo.Manejador).Assembly)); // (Assembly.GetExecutingAssembly())); línea original 
builder.Services.AddValidatorsFromAssembly(typeof(Nuevo.Manejador).Assembly); //(Assembly.GetExecutingAssembly());
builder.Services.AddFluentValidation(new[] { typeof(Nuevo.Manejador).Assembly });

builder.Services.AddDbContext<ContextoAutor>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConexionDatabase"));
});

builder.Services.AddAutoMapper(typeof(Consulta.Manejador));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);  // para el formato de fecha de Postgresql, si no, te sale error UTC


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
