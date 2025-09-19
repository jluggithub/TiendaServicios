using FluentValidation;
using MediatR;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;

namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class Nuevo
    {
        //patrón CQRS para dividir responsabilidades

        public class Ejecuta : IRequest<Unit>
        {
            public string Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }

            public Guid? AutorLibro { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {

            public EjecutaValidacion()
            {
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.AutorLibro).NotEmpty();
            }

        }

        public class Manejador : IRequestHandler<Ejecuta, Unit>
        {
            //Aquí necesito una instancia del EFCore, que realmente es una instancia de ContextoAutor
            public readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libro = new LibreriaMaterial
                {
                    Titulo = request.Titulo,
                    FechaPublicacion = request.FechaPublicacion,
                    AutorLibro = request.AutorLibro  // Guid.NewGuid().ToString() 
                };

                _contexto.LibreriaMaterial.Add(libro);
                var valor = await _contexto.SaveChangesAsync();

                if (valor > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el libro");

            }

        }
    }
}
