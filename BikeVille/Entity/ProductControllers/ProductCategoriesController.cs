﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BikeVille.Entity;
using BikeVille.Entity.EntityContext;
using Microsoft.AspNetCore.Authorization;

namespace BikeVille.Entity.ProductControllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly AdventureWorksLt2019Context _context;

        public ProductCategoriesController(AdventureWorksLt2019Context context)
        {
            _context = context;
        }

        // GET: api/ProductCategories
        [HttpGet("Index")]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories()
        {
            return await _context.ProductCategories.Include(c=>c.Products).Include(c => c.InverseParentProductCategory).ThenInclude(ip=>ip.Products).ToListAsync();
        }
        [HttpGet("IndexWhithOutProducts")]
        public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategoriesWhithOutProducts()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        // GET: api/ProductCategories/5
        [HttpGet("Details/{id}")]
        public async Task<ActionResult<ProductCategory>> GetProductCategory(int id)
        {
            var productCategory = await _context.ProductCategories.Include(c => c.Products).Include(c=>c.InverseParentProductCategory).ThenInclude(ip => ip.Products).FirstOrDefaultAsync(c=>c.ProductCategoryId==id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return productCategory;
        }

        //// PUT: api/ProductCategories/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutProductCategory(int id, ProductCategory productCategory)
        //{
        //    if (id != productCategory.ProductCategoryId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(productCategory).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductCategoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/ProductCategories
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<ProductCategory>> PostProductCategory(ProductCategory productCategory)
        //{
        //    _context.ProductCategories.Add(productCategory);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetProductCategory", new { id = productCategory.ProductCategoryId }, productCategory);
        //}

        //// DELETE: api/ProductCategories/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProductCategory(int id)
        //{
        //    var productCategory = await _context.ProductCategories.FindAsync(id);
        //    if (productCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.ProductCategories.Remove(productCategory);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool ProductCategoryExists(int id)
        {
            return _context.ProductCategories.Any(e => e.ProductCategoryId == id);
        }
    }
}
