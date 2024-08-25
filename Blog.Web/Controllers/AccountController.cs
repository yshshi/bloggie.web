using Blog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace Blog.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;

		public AccountController(UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

        [HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			var identityUser = new IdentityUser
			{
				UserName = registerViewModel.Username,
				Email = registerViewModel.Email,
			};


			var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);

			if (identityResult.Succeeded)
			{
				// assign user role
				var roleIdentityResult =await userManager.AddToRoleAsync(identityUser, "User");


				

				if (roleIdentityResult.Succeeded)
				{
					// show success notification
					TempData["Success"] = "Your details has been registered, Please Log In";
					return RedirectToAction("Register");

					//send mail to admin after successful registration of new user

				}
			}

			//show error noti
			TempData["Error"] = "Something went wrong!";
			return View();
		}


		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{

			var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.Username,
				loginViewModel.Password, false, false);
			
			if (signInResult != null && signInResult.Succeeded)
			{
				TempData["Success"] = "Welcome back!";
				return RedirectToAction("Index", "Home");
			}

			//show error noti
			TempData["Error"] = "Username or Password is incorrect";
			return View();
		}

		[HttpGet]					
		public async Task<IActionResult> Logout()
		{
             await signInManager.SignOutAsync();
			TempData["Success"] = "You are Logged Out!";
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult AccessDenied()
		{
			return View();
		}
	}
}
