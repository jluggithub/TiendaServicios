namespace TiendaServicios.Api.Libro.Aplicacion
{
    public class LibroMaterialDto
    {
        public Guid? LibreriaMaterialId { get; set; }

        public string Titulo { get; set; }

        public DateTime? FechaPublicacion { get; set; }

        //ojo: es la clave primaria del autor, no el autor. Eso está en otro microservice
        public Guid? AutorLibro { get; set; }

    }
}
