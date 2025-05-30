using MindSpace.Model;
using MindSpace.Presentation.Mvc.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MindSpace.Ui.Mvc.Controllers
{
    public class IdentityController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;

        public IdentityController(SignInManager<User> signInManager, UserManager<User> userManager, IWebHostEnvironment env)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _env = env;
        }


        [HttpGet]
        public async Task<IActionResult> SignIn(string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "/";
            }
            ViewBag.ReturnUrl = returnUrl;

            // Clear the existing external cookie to ensure a clean login process
            await _signInManager.SignOutAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel signInModel, string? returnUrl = null)
        {


            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "/";
            }
            ViewBag.ReturnUrl = returnUrl;


            if (!ModelState.IsValid)
            {
                return View();
            }
            User? user = await _userManager.FindByNameAsync(signInModel.Login);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(signInModel.Login);
            }

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, signInModel.Password, signInModel.RememberMe, lockoutOnFailure: false);
            
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username/Password combination is incorrect");
                return View();
            }



            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "/";
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = "/";
            }
            ViewBag.ReturnUrl = returnUrl;

            string? avatarPath = null;
            if (registerModel.ImageFile is not null)
            {


                var fileName = Path.GetFileName(registerModel.ImageFile.FileName);
                avatarPath = Path.Combine("images/avatar", fileName); // Relative path for use in HTML
                var filePath = Path.Combine(_env.WebRootPath, avatarPath); // Physical path for saving

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await registerModel.ImageFile.CopyToAsync(stream);
                }

            }

            var newUser = new User()
            {
                UserName = registerModel.UserName,
                AvatarUrl = "/" + (avatarPath?.Replace("\\", "/") ?? "images/avatar/user.png"), // e.g. "/images/avatar/avatar1.png"
                Email = registerModel.Email,
                Newsletter = registerModel.NewsLetter
            };

            var result = await _userManager.CreateAsync(newUser, registerModel.Password);

            if (!result.Succeeded)
            {

                foreach (var identityError in result.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }


                return View();
            }

            await _signInManager.SignInAsync(newUser, false);

            return LocalRedirect(returnUrl);

        }


        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");

        }
    }
}
