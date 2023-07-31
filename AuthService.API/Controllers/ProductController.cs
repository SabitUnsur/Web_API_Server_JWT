using AuthServer.Core.Dtos;
using AuthServer.Core.Entity;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IGenericService<Product,ProductDto> _service;

        public ProductController(IGenericService<Product, ProductDto> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return ActionResultInstance(await _service.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _service.AddAsync(productDto));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _service.Update(productDto,productDto.Id));
        }

        [HttpPut]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            return ActionResultInstance(await _service.Remove(Id));
        }

    }
}
