using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SR78POS.Data;
using SR78POS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SR78POS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ProductController>/5
        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var data = await (from p in _context.Product
                        join pp in _context.ProductPrice on p.SaleUnit equals pp.UnitId
                        where p.Barcode.Equals(code)
                        select new
                        {
                            p.ProductName,
                            p.ProductId,
                            p.Barcode,
                            pp.Price,
                            p.SaleUnit
                        }).FirstOrDefaultAsync();
            return Ok(data);
        }

        // POST api/<ProductController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
