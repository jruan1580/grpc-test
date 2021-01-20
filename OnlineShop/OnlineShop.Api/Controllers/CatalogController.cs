using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Api.Models.ApiRequests;
using OnlineShop.Api.Services;

namespace OnlineShop.Api.Controllers
{
    [Route("api/catalog")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemByItem(Guid id)
        {
            try
            {
                return Ok(await _catalogService.GetItemById(id));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetItemByCategory([FromQuery]string category)
        {
            try
            {
                if (string.IsNullOrEmpty(category))
                {
                    return BadRequest();
                }

                return Ok(await _catalogService.GetItemsByCategory(category));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetItemBySellerEmail([FromQuery] string sellerEmail)
        {
            try
            {
                if (string.IsNullOrEmpty(sellerEmail))
                {
                    return BadRequest();
                }

                return Ok(await _catalogService.GetItemsBySellerEmail(sellerEmail));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddItems([FromBody] NewItemRequests request)
        {
            try
            {
                if(request.NewItems == null || request.NewItems.Count <= 0)
                {
                    return StatusCode(201);
                }

                await _catalogService.AddItem(request.NewItems);

                return StatusCode(201);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateQuantity([FromBody]UpdateItemQuantityRequest request)
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
                    await _catalogService.AddQuantity(request.ItemId, request.Quantity);
                }
                else if (operation.Equals("subtract"))
                {
                    await _catalogService.SubtractQuantity(request.ItemId, request.Quantity);
                }
                else
                {
                    return BadRequest("Invalid operation provided");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
