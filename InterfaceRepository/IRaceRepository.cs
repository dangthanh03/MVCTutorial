using RunGroopWebApp.Models;

namespace RunGroopWebApp.InterfaceRepository
{
    public interface IRaceRepository
    {

        Task<IEnumerable<Race>> GetAllRaces();
        Task<Race> GetRace(int id);
        Task<IEnumerable<Race>> GetRaceByCity(string city);

        public bool Create(Race race);
        public bool Update(Race race);
        public bool Delete(Race Race);
        public bool Save();
    }
}
