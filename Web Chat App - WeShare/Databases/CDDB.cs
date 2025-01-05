using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Web_Chat_App___WeShare.Databases
{
	public class CDDB : DbContext
	{
		public CDDB(DbContextOptions<CDDB> options) : base(options) { }

		public DbSet<AccountAuth> Users { get; set; }
	}

	public class AccountAuth
	{
		[Required]
		[Display(Name = "User Name")]
		public string Username { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		public string? conID { get; set; }
		public int id { get; set; }
	}
}
