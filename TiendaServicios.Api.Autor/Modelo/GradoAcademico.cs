namespace TiendaServicios.Api.Autor.Modelo
{
    public class GradoAcademico
    {
        public int GradoAcademicoId { get; set; }

        public string Nombre { get; set; }

        public string CentroAcademico { get; set; }

        public DateTime? FechaGrado { get; set; }

        // De la siguiente forma establezco la relación uno a muchos:
        public int AutorLibroId { get; set; }  // es el "ancla" para que autor libro me referencie
        public AutorLibro AutorLibro { get; set; }

        public string GradoAcademicoGuid { get; set; }  // porque es un valor universal entre microservicios

    }
}
