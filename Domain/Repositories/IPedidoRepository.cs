using BoleriaAPI.Domain.Entity;

namespace BoleriaAPI.Domain.Repositories
{
    public interface IPedidoRepository
    {
        System.Threading.Tasks.Task<(System.Collections.Generic.IEnumerable<Pedido> Items, int Total)> ListAsync(int page, int pageSize);
        System.Threading.Tasks.Task<Pedido?> GetAsync(System.Guid id);
        System.Threading.Tasks.Task AddAsync(Pedido entity);
        System.Threading.Tasks.Task UpdateAsync(Pedido entity);
        System.Threading.Tasks.Task DeleteAsync(System.Guid id);
    }
}
