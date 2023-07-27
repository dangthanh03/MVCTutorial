using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.InterfaceRepository;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModel;

namespace RunGroopWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository dashboardRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPhotoService photoService;

        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor httpContextAccessor, IPhotoService photoService)
        {

            this.dashboardRepository = dashboardRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.photoService = photoService;
        }
        private void MapUserEdit(AppUser user ,EditUserDashboardViewModel editVM,ImageUploadResult PhotoResult)  {
            user.Id = editVM.Id;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;
            user.ProfileImageUrl = PhotoResult.Url.ToString();
            user.State = editVM.State;
            user.City = editVM.City;
        }

        public async Task<IActionResult> Index()
        {
            var userRaces = await dashboardRepository.GetAllUserRaces();
            var userClubs = await dashboardRepository.GetAllUserClubs();
            var dashboaViewModel = new DashboardViewModel()
            {
                Races = userRaces,
                Clubs = userClubs,

            };
            return View(dashboaViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var curUserId = httpContextAccessor.HttpContext.User.GetUserId();
            var user = await dashboardRepository.GetUserById(curUserId);
            if (user == null) { return View("Error"); }
            var editUserViewmodel = new EditUserDashboardViewModel()
            {
                Id = curUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            return View(editUserViewmodel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editVM);
            }

            AppUser user = await dashboardRepository.GetByIdNoTracking(editVM.Id);

            if (user.ProfileImageUrl == "" || user.ProfileImageUrl == null)
            {
                var PhotoResult = await photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, PhotoResult);
                dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editVM);
                }
                var PhotoResult = await photoService.AddPhotoAsync(editVM.Image);
                MapUserEdit(user, editVM, PhotoResult);
                dashboardRepository.Update(user);
                return RedirectToAction("Index");

            }

        }
    }    
}

