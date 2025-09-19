namespace TiendaServicios.Api.Libro.Modelo
{
    public class LibreriaMaterial
    {
        public Guid? LibreriaMaterialId { get; set; }

        public string Titulo { get; set; }

        public DateTime? FechaPublicacion { get; set; }

        //ojo: es la clave primaria del autor, no el autor. Eso está en otro microservice
        public Guid? AutorLibro { get; set; }
    }
}
