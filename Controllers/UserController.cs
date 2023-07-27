using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.InterfaceRepository;
using RunGroopWebApp.ViewModel;

namespace RunGroopWebApp.Controllers
{

    public class UserController : Controller
    {
        
        private readonly IUserRepository userRepository;


        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IActionResult> Detail(string id ) {
            var user = await userRepository.GetUserById(id);
            var userDetailViewModel = new UserDetailViewModel()
            {
                ProfileImageUrl=user.ProfileImageUrl,
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage,
            };

            return View(userDetailViewModel);
        }


        
        public async Task<IActionResult> Index()
        {
            var users = await userRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModle = new UserViewModel()
                {
                    ProfileImageUrl= user.ProfileImageUrl,
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Mileage = user.Mileage,

                };
                result.Add(userViewModle);
            }
            return View(result);
        }

        
      

    }
}
