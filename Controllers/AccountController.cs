using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModel;

namespace RunGroopWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> CheckRoleExists(string roleName)
{
    var roleExists = await _roleManager.RoleExistsAsync(roleName);
    
            if (roleExists)
    {
        // Vai trò đã tồn tại
        return Ok("Role exists.");
    }
    else
    {
        // Vai trò không tồn tại
        return NotFound("Role does not exist.");
    }
}

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel  RegisterVM)
        {
            var roleExists = await _roleManager.RoleExistsAsync(UserRoles.User);
                    if (!roleExists)
            {
                var newRole = new IdentityRole(UserRoles.User);
                var result = await _roleManager.CreateAsync(newRole);
                if (!result.Succeeded)
                {
                    // Vai trò đã được tạo thành công

                    TempData["Error"] = "Can't not create role !!!";
                    return View(RegisterVM);
                }
            }

            if (!ModelState.IsValid)
            {
                return View(RegisterVM);
            }

            var user = await userManager.FindByEmailAsync(RegisterVM.EmailAddress);
            if(user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return  View(RegisterVM);
            }
            var NewUser = new AppUser()
            {
                Email = RegisterVM.EmailAddress,
                UserName = RegisterVM.EmailAddress
            
            };

            var NewUserResponse = await userManager.CreateAsync(NewUser,RegisterVM.Password);
            if (NewUserResponse.Succeeded)
            {
                try
                {
                 
                    await userManager.AddToRoleAsync(NewUser, UserRoles.User);
                }
                catch (InvalidOperationException ex)
                {
                    TempData["Error"] = "Error: Role cannot be found. Please check role configuration.";
                    return View(RegisterVM);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "An error occurred: " + ex.Message;
                    return View(RegisterVM);

                }
                }else
            {
                TempData["Error"] = NewUserResponse.ToString();
                return View(RegisterVM);
            }
            
            return RedirectToAction("Index", "Race");
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index","Race");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVm)
        {
            if (!ModelState.IsValid) return View(loginVm);
            var user = await userManager.FindByEmailAsync (loginVm.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(user, loginVm.Password);

                if (passwordCheck)
                {
                    var result = await signInManager.PasswordSignInAsync(user, loginVm.Password,false,false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index","Race");
                    }
                }
                TempData["Error"] = "Wrong credentials. Please, try again";
                return View(loginVm);
            }
            TempData["Error"] = "Wrong credentials. Please, try again";
            return View(loginVm);

        }
    }
}
