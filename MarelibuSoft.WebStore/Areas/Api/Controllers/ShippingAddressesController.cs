using MarelibuSoft.WebStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MarelibuSoft.WebStore.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingAddressesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShippingAddressesController(ApplicationDbContext context)
        {
            _context = context;
        }
		[HttpGet("{id}")]
		public async Task<IActionResult>Get([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var shippingAddress = await _context.ShippingAddresses.FindAsync(id);

			if (shippingAddress == null)
			{
				return NotFound();
			}
			var addresses = await _context.ShippingAddresses.Where(a => a.CustomerID == shippingAddress.CustomerID).ToListAsync();

			foreach (var item in addresses)
			{
				if(item.ID == id)
				{
					item.IsMainAddress = true;
				}else
				{
					item.IsMainAddress = false;
				}
			}
			_context.UpdateRange(addresses);
			await _context.SaveChangesAsync();

			return Ok();
		}

        private bool ShippingAddressExists(int id)
        {
            return _context.ShippingAddresses.Any(e => e.ID == id);
        }
    }
}