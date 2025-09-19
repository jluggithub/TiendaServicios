using AutoMapper;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaServicios.Api.Libro.Aplicacion;
using TiendaServicios.Api.Libro.Modelo;
using TiendaServicios.Api.Libro.Persistencia;
using Xunit;

namespace TiendaServicios.Api.Libro.Tests
{
    public class LibrosServiceTest
    {

        private IEnumerable<LibreriaMaterial> ObtenerDataPrueba()
        {
            A.Configure<LibreriaMaterial>()
                .Fill(x=>x.Titulo).AsArticleTitle()
                .Fill(x=>x.LibreriaMaterialId, () => { return Guid.NewGuid(); });

            var lista = A.ListOf<LibreriaMaterial>(30);

            //pero el primer elemento tendrá un id fijo para pruebas
            lista[0].LibreriaMaterialId = Guid.Empty;

            return lista;
        }


        private Mock<ContextoLibreria> CrearContexto()
        {
            var dataPrueba = ObtenerDataPrueba().AsQueryable();

            //creamos una entidad (EF), por eso le damos todo eso: provider, expression... que son propiedades de EF
            var dbSet = new Mock<DbSet<LibreriaMaterial>>();
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Provider).Returns(dataPrueba.Provider);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.Expression).Returns(dataPrueba.Expression);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.ElementType).Returns(dataPrueba.ElementType);
            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x => x.GetEnumerator()).Returns(dataPrueba.GetEnumerator());

            //pero para que sean async necesitamos que también lo sean las propiedades
            dbSet.As<IAsyncEnumerable<LibreriaMaterial>>().Setup(x => x.GetAsyncEnumerator(new System.Threading.CancellationToken()))
                .Returns(new AsyncEnumerator<LibreriaMaterial>(dataPrueba.GetEnumerator()));

            dbSet.As<IQueryable<LibreriaMaterial>>().Setup(x=>x.Provider).Returns(new AsyncQueryProvider<LibreriaMaterial>(dataPrueba.Provider));
             
            var contexto = new Mock<ContextoLibreria>();
            contexto.Setup(x=>x.LibreriaMaterial).Returns(dbSet.Object);
            return contexto;
        }

        [Fact]
        public async Task GetLibroPorId()  
        {
            var mockContexto = CrearContexto();

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });

            var mapper = mapConfig.CreateMapper();

            var request = new ConsultaFiltro.LibroUnico();

            request.LibroId = Guid.Empty;

            var manejador = new ConsultaFiltro.Manejador(mockContexto.Object, mapper);

            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.NotNull(libro);
            Assert.True(libro.LibreriaMaterialId == Guid.Empty);
        }
            
        [Fact]
        public async Task GetLibros()  //async void is deprecated
        {
            // si queremos debuguear el método, ponemos: 
            System.Diagnostics.Debugger.Launch();  // y seleccionamos el método getLibros, a pelo, botón derecho -> run test


            // 1.- Emular la instancia de EF Core ContextoLibreria (parámetro constructor de la clase Manejador)
            //      para emular las acciones y eventos de un unit test environment
            //      utilizamos objetos de tipo Mock

            var mockContexto = CrearContexto(); //emulamos el contexto

            // 2.- Emular al mapping IMapper

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingTest());
            });  //emulamos el imapper

            var mapper = mapConfig.CreateMapper();

            // 3.- Instanciar a la clase Manejador y pasar los mocks como parámetros

            Consulta.Manejador manejador = new Consulta.Manejador(mockContexto.Object, mapper);

            Consulta.ListaLibros request = new Consulta.ListaLibros();

            var lista = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(lista.Any());  
        }

        [Fact]
        public async Task GuardarLibro()
        {
            // este objeto me representa la configuración que va a tener la BD en memoria
            var options = new DbContextOptionsBuilder<ContextoLibreria>()
                .UseInMemoryDatabase(databaseName: "BaseDatosLibro")
                .Options;

            var contexto = new ContextoLibreria(options);

            var request = new Nuevo.Ejecuta();
            request.Titulo = "Libro de Microservice";
            request.AutorLibro = Guid.Empty;
            request.FechaPublicacion = DateTime.Now;

            var manejador = new Nuevo.Manejador(contexto);

            // si es correcto, esta llamada me devuelve un objeto de tipo Unit, pero si fuera incorrecto devuelve un error
            var libro = await manejador.Handle(request, new System.Threading.CancellationToken());

            Assert.True(libro != null);
        }


    }
}
