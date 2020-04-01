using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPrimerWebApi.Contexts;
using MiPrimerWebApi.Entities;
using MiPrimerWebApi.Helpers;
using MiPrimerWebApi.Models;
using MiPrimerWebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IClaseB claseB;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IClaseB claseB, IMapper mapper)
        {
            this.context = context;
            this.claseB = claseB;
            this.mapper = mapper;
        }

        //get api/autor (responde a dos endpoints) api/autores/listado y /listado  el / ignora la regla de ruteo del controlador [Route("api/[controller]")]
        [HttpGet("/listado")]
        [HttpGet("listado")]
        [HttpGet]
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        public ActionResult<IEnumerable<AutorDTO>> Get()
        {
            //throw new NotImplementedException();
            claseB.HacerAlgo();

            //var autores = context.Autores.Include(x => x.Libros).ToList();
            var autores = context.Autores.ToList();
            var autoresDTO = mapper.Map<List<AutorDTO>>(autores);

            return autoresDTO;
        }

        [HttpGet("Primer")]
        public ActionResult<Autor> GetPrimerAutor()
        {
            return context.Autores.FirstOrDefault();
        }


        //GET api/autores/5 ó GET api/autores/5/juan  el ? es para que el parametro sea opcional
        [HttpGet("{id}/{param2?}", Name = "ObtenerAutor")]
        public async Task<ActionResult<AutorDTO>> Get(int id, string param2)
        {
            var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x=>x.Id == id);

            if(autor == null)
            {
                return NotFound();
            }

            // se usa para establecer que informacion va a los clientes de  la api
            var autorDTO = mapper.Map<AutorDTO>(autor);

            return autorDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacion)
        {
            //esto no es necesario en asp.net  core 2.1 en adelante
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            var autor = mapper.Map<Autor>(autorCreacion);
            context.Autores.Add(autor);
            await context.SaveChangesAsync();
            var autorDTO = mapper.Map<AutorDTO>(autor);

            //para devolver un 201 created
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id}, autorDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] AutorCreacionDTO autorActualizacion)
        {
            // al usar un DTO, no es necesaria esta comprobación

            //if(id != autor.Id)
            //{
            //    return BadRequest();
            //}

            //se mapea y se asigna el Id
            var autor = mapper.Map<Autor>(autorActualizacion);
            autor.Id = id;

            context.Entry(autor).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<AutorCreacionDTO> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            var autorDeLaBD = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if(autorDeLaBD == null)
            {
                return NotFound();
            }

            var autorDTO = mapper.Map<AutorCreacionDTO>(autorDeLaBD);

            patchDocument.ApplyTo(autorDTO, ModelState);

            mapper.Map(autorDTO, autorDeLaBD);

            var isValid = TryValidateModel(autorDeLaBD);

            if (!isValid)
            {
                return BadRequest(ModelState);
            }

            await context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Autor>> Delete(int id)
        {
            //usa el select para seleccionar solo el campo Id(optimiza la consulta)
            var autorId = await context.Autores.Select(x => x.Id).FirstOrDefaultAsync(x => x == id);

            if(autorId == default(int))
            {
                return NotFound();
            }

            context.Autores.Remove(new Autor { Id = autorId });
            context.SaveChanges();
            return autorId;
        }
    }
}
