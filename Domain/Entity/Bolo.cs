namespace BoleriaAPI.Domain.Entity
{
    public class Bolo
    {
        private Bolo() { }

        public System.Guid Id { get; private set; }
        public string Nome { get; private set; } = default!;
        public string Sabor { get; private set; } = default!;
        public decimal Preco { get; private set; }

        public System.Collections.Generic.ICollection<Pedido> Pedidos { get; private set; } = new System.Collections.Generic.List<Pedido>();

        public Bolo(string nome, string sabor, decimal preco)
        {
            Id = System.Guid.NewGuid();
            Atualizar(nome, sabor, preco);
        }

        public void Atualizar(string nome, string sabor, decimal preco)
        {
            if (string.IsNullOrWhiteSpace(nome)) throw new System.ArgumentException("Nome obrigatório");
            if (string.IsNullOrWhiteSpace(sabor)) throw new System.ArgumentException("Sabor obrigatório");
            if (preco <= 0) throw new System.ArgumentException("Preço deve ser maior que zero");

            Nome = nome.Trim();
            Sabor = sabor.Trim();
            Preco = preco;
        }
    }
}
