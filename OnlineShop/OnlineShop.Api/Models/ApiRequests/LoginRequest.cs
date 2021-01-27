using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Api.Models.ApiRequests
{
    public class LoginRequest : UpdateStatusRequest
    {
        [Required(AllowEmptyStrings = false)]
        [MinLength(8)]
        [MaxLength(32)]
        public string Password { get; set; }
    }
}
