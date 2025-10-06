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
    [Tags("Pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PedidoController(AppDbContext db) => _db = db;

        private static PedidoResponse ToResponse(Pedido p) => new(p.Id, p.BoloId, p.NomeCliente);

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os pedidos")]
        public async Task<ActionResult<System.Collections.Generic.IEnumerable<PedidoResponse>>> GetAll()
        {
            var items = await _db.Pedidos.AsNoTracking().ToListAsync();
            return Ok(items.Select(ToResponse));
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Obtém um pedido por Id")]
        public async Task<ActionResult<PedidoResponse>> GetById(System.Guid id)
        {
            var p = await _db.Pedidos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return p is null ? NotFound() : Ok(ToResponse(p));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cria um pedido")]
        public async Task<ActionResult<PedidoResponse>> Create([FromBody] PedidoRequest req)
        {
            var boloExists = await _db.Bolos.AnyAsync(b => b.Id == req.BoloId);
            if (!boloExists) return BadRequest("Bolo não encontrado.");

            var e = new Pedido(req.BoloId, req.NomeCliente);
            _db.Pedidos.Add(e);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = e.Id }, ToResponse(e));
        }

        [HttpPut("{id:guid}")]
        [SwaggerOperation(Summary = "Atualiza um pedido")]
        public async Task<IActionResult> Update(System.Guid id, [FromBody] PedidoRequest req)
        {
            var e = await _db.Pedidos.FindAsync(id);
            if (e is null) return NotFound();
            e.Atualizar(req.BoloId, req.NomeCliente);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [SwaggerOperation(Summary = "Exclui um pedido")]
        public async Task<IActionResult> Delete(System.Guid id)
        {
            var e = await _db.Pedidos.FindAsync(id);
            if (e is null) return NotFound();
            _db.Pedidos.Remove(e);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
