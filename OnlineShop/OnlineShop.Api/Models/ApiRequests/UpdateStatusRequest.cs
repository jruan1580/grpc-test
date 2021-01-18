using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Api.Models.ApiRequests
{
    public class UpdateStatusRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }
    }
}
