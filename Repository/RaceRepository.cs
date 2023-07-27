using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.InterfaceRepository;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext context;

        public RaceRepository(ApplicationDbContext  _context)
        {
            context = _context;
        }
        public bool Create(Race race)
        {
            context.Add(race);
            return Save();
        }

        public bool Delete(Race Race)
        {
            context.Remove(Race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAllRaces()
        {
            return await context.Races.ToListAsync();
        }

        public async Task<Race> GetRace(int id)
        {
            return await context.Races.Include(r => r.Address).AsNoTracking().Where(c=>c.Id==id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Race>> GetRaceByCity(string city)
        {
            return await context.Races.Include(r=>r.Address).Where(r => r.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
         var change = context.SaveChanges();
            return change > 0 ? true : false;
                }

        public bool Update(Race race)
        {
            context.Update(race);
            return Save();
        }
    }
}
