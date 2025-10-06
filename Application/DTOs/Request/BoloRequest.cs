namespace BoleriaAPI.Application.DTOs.Request
{
    public class BoloRequest
    {
        public string Nome { get; set; } = default!;
        public string Sabor { get; set; } = default!;
        public decimal Preco { get; set; }
    }
}
