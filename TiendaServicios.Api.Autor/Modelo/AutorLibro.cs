namespace TiendaServicios.Api.Autor.Modelo
{
    public class AutorLibro
    {
        public int AutorLibroId { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public ICollection<GradoAcademico> ListaGradosAcademicos { get; set; }

        //para la BD postgre voy a crear una clave:
        public string AutorLibroGuid { get; set; }  // porque es un valor universal entre microservicios
    }
}
