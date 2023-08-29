using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsDataflow;
using WebApiDataflow.Data;

namespace WebApiDataflow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        ApplicationDbContext Db { get; set; }
        ILogger Logger { get; set; }

        public ProductsController(ApplicationDbContext _db, ILogger<Product> _logger)
        {
            Db = _db;
            Logger = _logger;
        }

        // Obtener todos los productos.
        [HttpGet]
        public async Task<IActionResult> GetProducts() 
        {
            try
            {
                var products = await Db.Products.ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Algo salio mal en {nameof(GetProducts) }");
                return StatusCode(500, "Error interno de de servidor...");
            }
        }

        // Obtener un productyo por su id.
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductById(string id) 
        {
            var product = await Db.Products.FindAsync(id);
            if(product == null) 
            {
                return NotFound();
            }
            return Ok(product);
        }

        // Crear un producto.
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product producto) 
        {
            Db.Products.Add(producto);
            Db.SaveChanges();
            return CreatedAtAction(nameof(GetProductById), new { id = producto.ProductId }, producto);
        }

        [HttpPut]
        [Route("{Id}")]
        public async Task<IActionResult> UpdateProducts(string Id, UpdateProduct ProductUpdate) 
        {
            var producto = await Db.Products.FirstOrDefaultAsync(x => x.ProductId == Id);

            if (producto == null) 
            {
                return NotFound();
            }

            producto.ProductName = ProductUpdate.ProductName;
            producto.ProductPrice = ProductUpdate.ProductPrice;
            producto.Unit = ProductUpdate.Unit;
            producto.ProductCategory = ProductUpdate.ProductCategory;


            Db.Entry(producto).State = EntityState.Modified; 
            Db.SaveChanges();

            return Ok(producto);
        }

        [HttpDelete]
        [Route("Id")]
        public async Task<IActionResult> DeleteProducts(string Id) 
        {
            var ProductDelete = await Db.Products.FirstOrDefaultAsync(x => x.ProductId == Id);

            if (ProductDelete == null) 
            {
                return NotFound();
            }

            Db.Products.Remove(ProductDelete);
            Db.SaveChanges();

            return Ok(ProductDelete);
        }
    }
}
