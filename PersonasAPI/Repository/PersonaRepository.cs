using PersonasAPI.Entities;
using PersonasAPI.Configuration;
using Microsoft.EntityFrameworkCore;

namespace PersonasAPI.Repository
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly AppDbContext _appDbContext;

        public PersonaRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Personas>> GetAllPersonas()
        {
            IQueryable<Personas> query = _appDbContext.Personas.OrderBy(f => f.NOMBRE);
            return await query.ToListAsync();
        }

    }
}
