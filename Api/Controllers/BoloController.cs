using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using BoleriaAPI.Infrastructure.Context;
using BoleriaAPI.Domain.Entity;
using BoleriaAPI.Application.DTOs.Request;
using BoleriaAPI.Application.DTOs.Response;

namespace BoleriaAPI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Bolos")]
    public class BoloController : ControllerBase
    {
        private readonly AppDbContext _db;
        public BoloController(AppDbContext db) => _db = db;

        private static BoloResponse ToResponse(Bolo b) => new(b.Id, b.Nome, b.Sabor, b.Preco);

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os bolos")]
        public async Task<ActionResult<System.Collections.Generic.IEnumerable<BoloResponse>>> GetAll()
        {
            var items = await _db.Bolos.AsNoTracking().ToListAsync();
            return Ok(items.Select(ToResponse));
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Obtém um bolo por Id")]
        public async Task<ActionResult<BoloResponse>> GetById(System.Guid id)
        {
            var b = await _db.Bolos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return b is null ? NotFound() : Ok(ToResponse(b));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cria um bolo")]
        public async Task<ActionResult<BoloResponse>> Create([FromBody] BoloRequest req)
        {
            var e = new Bolo(req.Nome, req.Sabor, req.Preco);
            _db.Bolos.Add(e);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = e.Id }, ToResponse(e));
        }

        [HttpPut("{id:guid}")]
        [SwaggerOperation(Summary = "Atualiza um bolo")]
        public async Task<IActionResult> Update(System.Guid id, [FromBody] BoloRequest req)
        {
            var e = await _db.Bolos.FindAsync(id);
            if (e is null) return NotFound();
            e.Atualizar(req.Nome, req.Sabor, req.Preco);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [SwaggerOperation(Summary = "Exclui um bolo")]
        public async Task<IActionResult> Delete(System.Guid id)
        {
            var e = await _db.Bolos.FindAsync(id);
            if (e is null) return NotFound();
            _db.Bolos.Remove(e);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
