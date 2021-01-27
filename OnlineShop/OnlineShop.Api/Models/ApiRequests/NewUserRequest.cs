using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Api.Models.ApiRequests
{
    public class NewUserRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MinLength(8)]
        [MaxLength(32)]
        public string Password { get; set; }
    }
}
