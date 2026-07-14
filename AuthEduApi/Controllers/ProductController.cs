using AuthEduApi.Constants;
using AuthEduApi.Data;
using AuthEduApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthEduApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context = new AppDbContext();
        public ProductController(AppDbContext appDb)
        {
            _context = appDb;
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }


        [Authorize]
        [HttpGet($"get-product")]
        public async Task<IActionResult> GetProducts(Guid id)
        {
            var products = await _context.Products.FirstOrDefaultAsync(x=>x.Id==id);
            return Ok(products);
        }


        [Authorize(Roles =AppRoles.Admin)]
        [HttpPost("update-price")]
        public async Task<IActionResult> UpdatePrice(UpdateProductPriceDto dto)
        {
            var products = _context.Products.FirstOrDefault(x => x.Id == dto.Id);
            products.Price = dto.Price;
            _context.Products.Update(products);
            await _context.SaveChangesAsync();
            return Ok(products);
        }

        [Authorize(policy: AppPolicies.ProUser)]
        [HttpPost("update-Name")]
        public async Task<IActionResult> UpdateName(UpdateProductNameDto dto)
        {
            var products = _context.Products.FirstOrDefault(x => x.Id == dto.Id);
            products.Name = dto.Name;
            _context.Products.Update(products);
            await _context.SaveChangesAsync();
            return Ok(products);
        }



    }
}
