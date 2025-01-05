using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Web_Chat_App___WeShare.Databases;

namespace Web_Chat_App___WeShare.Pages.Account
{
	public class CreateModel : PageModel
	{
		[BindProperty]
		public AccountAuth Credential { get; set; }

		private readonly CDDB _context;

		public List<AccountAuth> Users { get; set; }

		public CreateModel(CDDB context)
		{
			_context = context;
		}

		public async Task OnGet()
		{
			Users = await _context.Users.ToListAsync();
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid) { return Page(); }
			if (Credential.conID == null) { Credential.conID = "404"; }


			if (Users != null)
			{
				foreach (AccountAuth user in Users)
				{
					if (user.Username == Credential.Username)
					{
						ModelState.AddModelError("", "Invalid Username");
						return Page();
					}
					else if (user.Email == Credential.Email)
					{
						ModelState.AddModelError("", "Invalid Email");
						return Page();
					}
				}
			}

			_context.Users.Add(Credential);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Login");
		}
	}
}
