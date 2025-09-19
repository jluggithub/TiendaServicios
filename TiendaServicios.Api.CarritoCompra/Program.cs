using FluentValidation;
using MediatR.Extensions.FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.CarritoCompra.Aplicacion;
using TiendaServicios.Api.CarritoCompra.Persistencia;
using TiendaServicios.Api.CarritoCompra.RemoteInterface;
using TiendaServicios.Api.CarritoCompra.RemoteService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ILibrosService, LibrosService>();

// Add services to the container.
builder.Services.AddControllers(); //declaro el registro de este servicio para luego instanciarlo.

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Nuevo.Manejador).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(Nuevo.Manejador).Assembly);
builder.Services.AddFluentValidation(new[] { typeof(Nuevo.Manejador).Assembly });

builder.Services.AddDbContext<CarritoContexto>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("ConexionDatabase"));
});

builder.Services.AddHttpClient("Libros", config =>
{
    config.BaseAddress = new Uri(builder.Configuration["Services:Libros"]);
});










// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
//    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
