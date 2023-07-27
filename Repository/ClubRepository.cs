using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class ClubRepository : IClubRepostiroy
    {
        private readonly ApplicationDbContext context;

        public ClubRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public bool Create(Club club )
        {
            context.Add( club );
            return Save();
        
        }

        public bool Delete(Club club)
        {
            context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAllClub()
        {
            return await context.Clubs.ToListAsync();
        }

        public async Task<Club> GetClub(int id)
        {
            return await context.Clubs.Include(c => c.Address).AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync(); 
                }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await context.Clubs.Include(c=>c.Address).Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var change = context.SaveChanges();
            return  change > 0 ?true : false;
        }

        public bool Update( Club newclub)
        {
            context.Update(newclub);
            return Save();
        }
    }
}
