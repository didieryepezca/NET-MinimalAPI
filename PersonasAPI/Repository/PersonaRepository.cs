using PersonasAPI.Entities;
using PersonasAPI.Configuration;
using Microsoft.EntityFrameworkCore;

namespace PersonasAPI.Repository
{
    public class PersonaRepository
    {
        public async Task<IEnumerable<Personas>> GetAllPersonas() {

            using (var db = new AppDbContext())
            {
                //IQueryable<Personas> query = await db.Personas.OrderBy(f => f.NOMBRE);
                //return query.ToList();

                return await db.Personas.OrderBy(f => f.NOMBRE).ToListAsync();                
            }
        }

    }
}
