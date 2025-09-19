using Microsoft.EntityFrameworkCore;
using TiendaServicios.Api.Libro.Modelo;

namespace TiendaServicios.Api.Libro.Persistencia
{
    public class ContextoLibreria : DbContext
    {
        public ContextoLibreria()  // hay que agregar manualmente este constructor para que los tests funcionen
        {
            
        }

        public ContextoLibreria(DbContextOptions<ContextoLibreria> options) : base(options)
        {                
        }

        public virtual DbSet<LibreriaMaterial> LibreriaMaterial { get; set; }  // el virtual se pone porque en el test vamos a sobrescribir el método

    }
}
