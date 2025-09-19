using FluentValidation;
using MediatR;
using TiendaServicios.Api.CarritoCompra.Modelo;
using TiendaServicios.Api.CarritoCompra.Persistencia;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        //patrón CQRS para dividir responsabilidades

        public class Ejecuta : IRequest<Unit>
        {
            public DateTime? FechaCreacionSesion { get; set; }

            public List<string> ProductoLista { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, Unit>
        {
            public readonly CarritoContexto _contexto;

            public Manejador(CarritoContexto contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var carritoSesion = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacionSesion
                };

                _contexto.CarritoSesion.Add(carritoSesion);
                var valor = await _contexto.SaveChangesAsync();

                if (valor == 0)
                {
                    throw new Exception("Errores en la inserción del carrito de compra");                    
                }

                //dicho Id ya viene del save anterior:
                int id = carritoSesion.CarritoSesionId;

                foreach (var obj in request.ProductoLista)
                {
                    var detalleSesion = new CarritoSesionDetalle
                    {
                        FechaCreacion = DateTime.Now,
                        CarritoSesionId = id,
                        ProductoSeleccionado = obj                       
                    };

                    _contexto.CarritoSesionDetalles.Add(detalleSesion);
                }

                valor = await _contexto.SaveChangesAsync();

                if (valor > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo insertar el detalle del carrito de compras");
            }
        }
    }
}
