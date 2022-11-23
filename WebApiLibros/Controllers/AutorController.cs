using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiLibros.Contexto;
using WebApiLibros.Entidades;

namespace WebApiLibros.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutorController(ApplicationDbContext context)
        {
            this.context = context;
        }

       [HttpGet]
        public ActionResult<IEnumerable<Autor>> Get()
        {
           return context.Autores.Include(x => x.Libros).ToList();
        }

        //[HttpGet("/listado")]
        [HttpGet("listado")]        //https://localhost:44352/api/autor/listado
        public ActionResult<IEnumerable<Autor>> GetTodos()
        {
            return context.Autores.ToList();
        }

        //[HttpGet("{id}/{param2}", Name = "ObtenerAutor")]   //parámetros obligatorios
        //[HttpGet("{id}/{param2?}", Name ="ObtenerAutor")]   //segundo parámetro opcional
        [HttpGet("{id}/{param2=hola}", Name = "ObtenerAutor")]   //segundo parámetro opcional con valor por omisión
                                                                 //public ActionResult<Autor> Get(int id, string param2)
        public ActionResult<Autor> Get(int id, string param2)
        {
            var resultado = context.Autores.Include(x => x.Libros).FirstOrDefault(x => x.Id == id);

            if (resultado == null)
            { return NotFound(); }

            return resultado;
        }

        //[HttpGet("primer")]
        //public ActionResult<Autor> GetPrimerAutor() //sincrónico
        //{
        //    return context.Autores.FirstOrDefault();
        //}

        [HttpGet("primer")]
        public async Task<ActionResult<Autor>> GetPrimerAutor() //asincrónico
        {
            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpPost]
        public ActionResult Post([FromBody] Autor autor)
        {
            //Esto no es necesario desde Asp.Net Core 2.1 
            //if (!ModelState.IsValid)
            //{ return BadRequest(ModelState); }

            context.Autores.Add(autor);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autor);
        }


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Autor value)
        {
            if (id != value.Id)
            {
                BadRequest();
            }

            context.Entry(value).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Autor> Delete(int id)
        {
            var resultado = context.Autores.FirstOrDefault(x => x.Id == id);

            if (resultado == null)
            { return NotFound(); }

            context.Autores.Remove(resultado);
            context.SaveChanges();
            return resultado;
        }



    }
}