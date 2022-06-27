using PersonasAPI.Entities;

namespace PersonasAPI.Repository
{
    public interface IPersonaRepository
    {
        Task<IEnumerable<Personas>> GetAllPersonas();
    }
}
