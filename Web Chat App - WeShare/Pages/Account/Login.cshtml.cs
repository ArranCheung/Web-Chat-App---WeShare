using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using Web_Chat_App___WeShare.Databases;
using Web_Chat_App___WeShare.Hubs;

namespace Web_Chat_App___WeShare.Pages.Account
{
	public class LoginModel : PageModel
	{
		[BindProperty]
		public AccountAuth Credential { get; set; }

		private readonly CDDB _context;

		public LoginModel(CDDB context)
		{
			_context = context;
		}

		public List<AccountAuth>? Users { get; set; }

		public async Task OnGetAsync()
		{
			try
			{
				Users = await _context.Users.ToListAsync();
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid) return Page();

			// check database for matching credentials
			if (Users == null || Users.Count == 0)
			{
				Users = await _context.Users.ToListAsync();
			}

			var authUser = _context.Users.SingleOrDefault(u => u.Email == Credential.Email && u.Password == Credential.Password);
			if (authUser == null)
			{
				ModelState.AddModelError("", "Invalid Credentials");
				return Page();
			}

			return RedirectToPage("/Chat", new { username = Credential.Username });
		}
	}
}