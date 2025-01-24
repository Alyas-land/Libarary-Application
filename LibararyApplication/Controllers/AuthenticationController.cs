using LibararyApplication.DatabaseContext;
using LibararyApplication.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibararyApplication.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly MyDbContext _context;

        public AuthenticationController(MyDbContext context)
        {
            _context = context;
        }
  

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Header = "login";
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            
            if (!ModelState.IsValid) {
                TempData["error"] = "ورود نا موفق، اطلاعات به صورت صحیح وارد نشده است";
                return View(model);
            }
            var newUser = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Role = "user",
            };
            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString()),
                new Claim(ClaimTypes.Name, newUser.Name),
                new Claim(ClaimTypes.Role, newUser.Role)


            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties
            {
                IsPersistent = true,
            };

            

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
            TempData["success"] = "ورود موفقیت آمیز";
            return RedirectToAction("Index", "Home");


        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            ViewBag.Header = "login";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "ورود نا موفق، اطالعات به صورت صحیح وارد نشده است";
                return View(model);
            }
            var target = _context.Users.Where(i => i.Email == model.Email && i.Password == model.Password).SingleOrDefault();
            if (target == null)
            {
                TempData["error"] = "نام کاربری یا پسورد اشنباه می باشد";
                return RedirectToAction("Login");
            }
            if (target != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, target.Id.ToString()),
                new Claim(ClaimTypes.Name, target.Name),
                new Claim(ClaimTypes.Role, target.Role),


            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);
                TempData["success"] = " ورود موفقیت آمیز به حساب کاربری";
                return RedirectToAction("Index", "Home");

            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["success"] = "با موفقیت از حساب کاربری حارج شدید";
            return RedirectToAction("Login");
        }

        [HttpGet]
		public async Task<IActionResult> Account()
		{
			int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var targetUser = await _context.Users.FindAsync(userId);
            var targetUserForViewModel = new RegisterViewModel
            {
                Name = targetUser.Name,
                Email = targetUser.Email,
            };
            ViewBag.Header = "account";
            return View(targetUserForViewModel);
		}

        [HttpPost]
		public async Task<IActionResult> Account(RegisterViewModel model)
		{
            if (!ModelState.IsValid) {
                TempData["error"] = "اطلاعات ناقص می باشد";
                return View(model);
            }
			int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
			var targetUser = await _context.Users.FindAsync(userId);

            targetUser.Name = model.Name;
            targetUser.Email = model.Email;
            targetUser.Password = model.Password;
            await _context.SaveChangesAsync();

			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);


			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, targetUser.Id.ToString()),
				new Claim(ClaimTypes.Name, targetUser.Name),
				new Claim(ClaimTypes.Role, targetUser.Role),


			};
			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			var principal = new ClaimsPrincipal(identity);

			var properties = new AuthenticationProperties
			{
				IsPersistent = true,
			};
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties);


			TempData["success"] = "تغییرات با موفقیت رخیره گردید";
            ViewBag.Header = "account";
			return RedirectToAction("Account");
		}

	}
}
