using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RunGroopWebApp.Data;
using RunGroopWebApp.InterfaceRepository;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Club>> GetAllUserClubs()
        {
            var curUser = httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = context.Clubs.Where(r => r.AppUser.Id == curUser);
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var curUser = httpContextAccessor.HttpContext?.User.GetUserId();
            var userRaces = context.Races.Where(r => r.AppUser.Id == curUser);
            return userRaces.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await context.Users.FindAsync(id) ;
        }

        public async Task<AppUser> GetByIdNoTracking(string id)
        {
            return await context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public bool Update(AppUser user)
        {
            context.Users.Update(user);
            return Save();
        }
        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
