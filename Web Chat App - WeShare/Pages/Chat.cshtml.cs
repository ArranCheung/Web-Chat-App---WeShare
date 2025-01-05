using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_Chat_App___WeShare.Pages
{
    public class ChatModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Username { get; set; }   
        
        public void OnGet()
        {
        }
    }
}
