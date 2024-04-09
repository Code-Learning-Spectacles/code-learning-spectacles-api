using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeLearningSpectaclesAPI.Data;
using CodeLearningSpectaclesAPI.Models;

namespace CodeLearningSpectaclesAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CodinglanguagesController : ControllerBase
    {
        private readonly CodeLearningDbContext _context;

        public CodinglanguagesController(CodeLearningDbContext context)
        {
            _context = context;
        }

        // GET: api/Codinglanguages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Codinglanguage>>> GetCodinglanguages()
        {
            return Ok(await _context.Codinglanguages.ToListAsync());
        }

        // GET: api/Codinglanguages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Codinglanguage>> GetCodinglanguage(int id)
        {
            var codinglanguage = await _context.Codinglanguages.FindAsync(id);

            if (codinglanguage == null)
            {
                return NotFound();
            }

            return Ok(codinglanguage);
        }

        // PUT: api/Codinglanguages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCodinglanguage(int id, Codinglanguage codinglanguage)
        {
            if (id != codinglanguage.Codinglanguageid)
            {
                return BadRequest();
            }

            _context.Entry(codinglanguage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodinglanguageExists(id))
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

        // POST: api/Codinglanguages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Codinglanguage>> PostCodinglanguage(Codinglanguage codinglanguage)
        {
            _context.Codinglanguages.Add(codinglanguage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCodinglanguage", new { id = codinglanguage.Codinglanguageid }, codinglanguage);
        }

        // DELETE: api/Codinglanguages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCodinglanguage(int id)
        {
            var codinglanguage = await _context.Codinglanguages.FindAsync(id);
            if (codinglanguage == null)
            {
                return NotFound();
            }

            _context.Codinglanguages.Remove(codinglanguage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CodinglanguageExists(int id)
        {
            return _context.Codinglanguages.Any(e => e.Codinglanguageid == id);
        }
    }
}
