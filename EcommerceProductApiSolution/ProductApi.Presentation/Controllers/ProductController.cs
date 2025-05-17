using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Application.DTOs.Convenstions;
using Azure;
using Microsoft.AspNetCore.Authorization;
namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductController(IProduct ProductInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ProductDto>> GetProducts()
        {
            var result = await ProductInterface.GetAllAsync();
            if (result == null)
            {
                return NotFound("No Product in DB");
            }
            var(_,list) = ProductConversion.FromEntity(null!, result);
            return list!.Any() ?Ok(list):NotFound("NO Product found");
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var result = await ProductInterface.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound("No Product in DB");
            }

            var (product, _) = ProductConversion.FromEntity(result, null);
            return product is not null ? Ok(product) : NotFound("No Product found");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> CreateProduct([FromBody]CreateProductDto product )
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var getEntity = ProductConversion.ToEntity(product);
            var response = await ProductInterface.CreateAsync(getEntity);
            return Ok(response);
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateProduct(int id , UpdateProductDto dto )
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await ProductInterface.UpdateAsync(id, dto);
            return Ok(response);
        }
        [HttpDelete]
        public async Task<ActionResult<Response>>DeleteProduct(int Id)
        {
                var response = await ProductInterface.DeleteAsync(Id);
            return Ok(response);
        }
    }
}
