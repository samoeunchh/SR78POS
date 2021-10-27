using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SR78POS.Data;
using SR78POS.Models;
using System.Linq.Dynamic.Core;

namespace SR78POS.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public JsonResult GetData()
        {
            JsonResult result;
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var data1 = (from p in _context.Product
                             join u in _context.Unit on p.SaleUnit equals u.UnitId
                             join c in _context.Category on p.CategoryId equals c.CategoryId
                             select new { 
                                 p.ProductId,
                                 p.ProductName,
                                 p.Barcode,
                                 p.Description,
                                 p.OnHand,
                                 p.Cost,
                                 CategoryId=c.CategoryName,
                                 SaleUnit=u.UnitName
                             });
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data1 = data1.Where(m => m.ProductName.Contains(searchValue.ToLower()) ||
                    m.Barcode.StartsWith(searchValue.ToLower()));
                }
                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    data1 = data1.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //total number of rows counts   
                recordsTotal = data1.Count();
                //Paging   
                var data = data1.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                result = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
            }
            catch
            {
                result = null;
            }
            return result;
        }
        // GET: Products
        public IActionResult Index()
        {
            return View();
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Unit)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            ViewData["SaleUnit"] = new SelectList(_context.Unit, "UnitId", "UnitName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var pid = Guid.NewGuid();
                if(product.ProductPrices != null)
                {
                    for(int i = 0; i < product.ProductPrices.Count; i++)
                    {
                        product.ProductPrices[i].ProductId = pid;
                        product.ProductPrices[i].ProductPriceId = Guid.NewGuid();
                    }
                }
                product.ProductId = pid;
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["SaleUnit"] = new SelectList(_context.Unit, "UnitId", "UnitName", product.SaleUnit);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["SaleUnit"] = new SelectList(_context.Unit, "UnitId", "UnitName", product.SaleUnit);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductId,CategoryId,SaleUnit,ProductName,Barcode,Description,OnHand,Cost")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["SaleUnit"] = new SelectList(_context.Unit, "UnitId", "UnitName", product.SaleUnit);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.Unit)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<JsonResult> GetUnits()
            => Json(await _context.Unit.ToListAsync());
        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
