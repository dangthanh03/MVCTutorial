using AutoMapper;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModel;

namespace RunGroopWebApp.Helper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Club, EditClubViewModel>();
            CreateMap<EditClubViewModel, Club>();
            CreateMap<EditRaceViewModel, Race>();
            CreateMap<Race, EditRaceViewModel>();
        }
    }
}
