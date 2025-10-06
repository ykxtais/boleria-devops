using BoleriaAPI.Domain.Entity;

namespace BoleriaAPI.Domain.Repositories
{
    public interface IBoloRepository
    {
        System.Threading.Tasks.Task<(System.Collections.Generic.IEnumerable<Bolo> Items, int Total)> ListAsync(int page, int pageSize);
        System.Threading.Tasks.Task<Bolo?> GetAsync(System.Guid id);
        System.Threading.Tasks.Task AddAsync(Bolo entity);
        System.Threading.Tasks.Task UpdateAsync(Bolo entity);
        System.Threading.Tasks.Task DeleteAsync(System.Guid id);
    }
}
