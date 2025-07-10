using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VertexTestApi.Interfaces;
using VertexTestApi.Models;

namespace VertexTestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //JWT authentication
    public class ItemsController : ControllerBase
    {
        private readonly IDatabaseRepository _dbRep;

        public ItemsController(IDatabaseRepository dbService)
        {
            _dbRep = dbService;
        }

        //GET : api/items
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var items = await _dbRep.GetItemsAsync();
                return Ok(items);
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }

        //POST : api/items
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _dbRep.AddItemAsync(item); 
                return CreatedAtAction(nameof(Get), item); 
            }
            catch
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
