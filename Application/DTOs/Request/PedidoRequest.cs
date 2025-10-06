using System;

namespace BoleriaAPI.Application.DTOs.Request
{
    public class PedidoRequest
    {
        public Guid BoloId { get; set; }
        public string? NomeCliente { get; set; }
    }
}