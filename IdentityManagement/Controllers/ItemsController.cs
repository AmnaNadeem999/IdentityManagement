using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityManagement.Data;
using IdentityManagement.Model;
using System.Text.RegularExpressions;

namespace IdentityManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IdentityManagementContext _context;

        public ItemsController(IdentityManagementContext context)
        {
            _context = context;
        }

        // GET: api/Items                       //Pagination implemented in GetAll 
        [HttpGet()]
        public async Task<ActionResult<List<Item>>> GetItems(int page)
        {
            if (_context.Item == null)
            {
                return NotFound();
            }
            var pageResults = 3f;   // 3 items per page
            var pageCount = Math.Ceiling(_context.Item.Count() / pageResults);
            var items = await _context.Item
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();
            
            var response = new Paginator
            {
                Items = items,
                current_page = page,
                pages = (int)pageCount
            };
            return Ok(response);
        }

        // GET: api/Items/5                         //Get Item by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            if (_context.Item == null)
            {
                return NotFound();
            }
            var item = await _context.Item.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

      
        // PUT: api/Items/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Items
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
          if (_context.Item == null)
          {
              return Problem("Entity set 'IdentityManagementContext.Item'  is null.");
          }
            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }
       

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            if (_context.Item == null)
            {
                return NotFound();
            }
            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Implementing Universal Search

        [HttpGet("UniversalSearch")]
        public async Task<ActionResult<IEnumerable<Item>>> GetAll(string text)
        {
            if (_context.Item == null)
            {
                return NotFound();
            }
            else if (Regex.IsMatch(text, "^[0-9]*$"))
            {
                var linq = from o in _context.Item.Where(o => o.Price == Int32.Parse(text)) select o;
                return Ok(linq);
            }
            else
            {
                var linq = from o in _context.Item.Where(o => o.Name.Contains(text)) select o;
                return Ok(linq);
            }
        }

        //Implementing Filter By Name 

        [HttpGet("FilterByName")]
        public async Task<ActionResult<IEnumerable<Item>>> GetName(string searchName)
        {
            IQueryable<Item> items = _context.Item;
            if (!string.IsNullOrEmpty(searchName))
            {

                items = items.Where(o => o.Name.Contains(searchName));
            }
            return await items.ToListAsync();
        }


        private bool ItemExists(int id)
        {
            return (_context.Item?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
