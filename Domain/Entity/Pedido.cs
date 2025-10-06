namespace BoleriaAPI.Domain.Entity
{
    public class Pedido
    {
        private Pedido() { }

        public System.Guid Id { get; private set; }
        public System.Guid BoloId { get; private set; }
        public Bolo? Bolo { get; private set; }

        public string? NomeCliente { get; private set; }

        public Pedido(System.Guid boloId, string? nomeCliente)
        {
            Id = System.Guid.NewGuid();
            Atualizar(boloId, nomeCliente);
        }

        public void Atualizar(System.Guid boloId, string? nomeCliente)
        {
            BoloId = boloId;
            NomeCliente = string.IsNullOrWhiteSpace(nomeCliente) ? null : nomeCliente.Trim();
        }
    }
}
