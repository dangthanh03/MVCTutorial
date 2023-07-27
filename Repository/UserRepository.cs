using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.InterfaceRepository;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await applicationDbContext.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await applicationDbContext.Users.FindAsync(id); 
        }

        public bool Save()
        {
            var saved = applicationDbContext.SaveChanges();
            return saved > 0 ? true : false;
        }
      

        public bool Update(AppUser user)
        {
           applicationDbContext.Update(user);

            return Save();
        }
    }
}
