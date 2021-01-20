using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Api.Models.ApiRequests;
using OnlineShop.Api.Services;

namespace OnlineShop.Api.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCartByUserId(Guid userId)
        {
            try
            {
                return Ok(await _cartService.GetUsersCart(userId));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody]UpdateCartItemQuantityRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Operation))
                {
                    return BadRequest("Operation is not provided");
                }

                var operation = request.Operation.ToLower().Trim();

                if (operation.Equals("add"))
                {
                    await _cartService.AddItemToCart(request.UserId, request.ItemId, request.Quantity);
                }
                else if (operation.Equals("subtract"))
                {
                    await _cartService.ReduceItemQuantity(request.UserId, request.ItemId, request.Quantity);
                }
                else
                {
                    return BadRequest("Invalid operation provided");
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveItemFromCart([FromBody]RemoveItemFromCartRequest request)
        {
            try
            {
                await _cartService.RemoveItemFromCart(request.UserId, request.ItemId);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
