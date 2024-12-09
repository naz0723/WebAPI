using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Database;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : Controller
    {
        private readonly AppDBContext _context;

        public PersonasController(AppDBContext context)
        {
            {
                _context = context;
            }
        }
        //Metodos
        //Obtener todas las personas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personas>>> GetPersonas()
        {

            return await _context.personas.ToListAsync();

        }
        //Obtener personas por ID 
        [HttpGet("{id}")]
        public async Task<ActionResult<Personas>> GetPersonas(int id)
        {
            var personas = await _context.personas.FindAsync(id);
            if (personas == null) return NotFound();
            return personas;

        }
        // Crear una nueva persona
        [HttpPost]
        public async Task<ActionResult<Personas>> CreatePersonas(Personas personas)
        {

            personas.FechaAdicion = DateTime.UtcNow; // Asignar fecha actual
            _context.personas.Add(personas);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPersonas), new { id = personas.IdPersona }, personas);


        }
        // Actualizar una persona
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersona(int id, Personas personas)
        {
            if (id != personas.IdPersona) return BadRequest();

            var personaExistente = await _context.personas.FindAsync(id);
            if (personaExistente == null) return NotFound();

            personaExistente.Nombre = personas.Nombre;
            personaExistente.Apellido = personas.Apellido;
            personaExistente.Email = personas.Email;
            personaExistente.FechaNacimiento = personas.FechaNacimiento;
            personaExistente.Telefono = personas.Telefono;
            personaExistente.ModificadoPor = personas.ModificadoPor;
            personaExistente.FechaModificacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();


        }
        // Eliminar una persona
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeletePersonas(int id)
        {
            var persona = await _context.personas.FindAsync(id);
            if (persona == null) return NotFound();
            _context.personas.Remove(persona);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}