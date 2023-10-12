using System.ComponentModel.DataAnnotations;

namespace UmbracoProject2.Models
{
	public class User
	{
    [Required]
        public string UserName { get; set; }
		[Required]
		public string Password { get; set; }
    }
}
