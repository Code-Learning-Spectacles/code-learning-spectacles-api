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
    [Route("api/[controller]")]
    [ApiController]
    public class ConstructtypesController : ControllerBase
    {
        private readonly CodeLearningDbContext _context;

        public ConstructtypesController(CodeLearningDbContext context)
        {
            _context = context;
        }

        // GET: api/Constructtypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Constructtype>>> GetConstructtypes()
        {
            return await _context.Constructtypes.ToListAsync();
        }

        // GET: api/Constructtypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Constructtype>> GetConstructtype(int id)
        {
            var constructtype = await _context.Constructtypes.FindAsync(id);

            if (constructtype == null)
            {
                return NotFound();
            }

            return constructtype;
        }

        // PUT: api/Constructtypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConstructtype(int id, Constructtype constructtype)
        {
            if (id != constructtype.Constructtypeid)
            {
                return BadRequest();
            }

            _context.Entry(constructtype).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConstructtypeExists(id))
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

        // POST: api/Constructtypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Constructtype>> PostConstructtype(Constructtype constructtype)
        {
            _context.Constructtypes.Add(constructtype);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConstructtype", new { id = constructtype.Constructtypeid }, constructtype);
        }

        // DELETE: api/Constructtypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConstructtype(int id)
        {
            var constructtype = await _context.Constructtypes.FindAsync(id);
            if (constructtype == null)
            {
                return NotFound();
            }

            _context.Constructtypes.Remove(constructtype);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConstructtypeExists(int id)
        {
            return _context.Constructtypes.Any(e => e.Constructtypeid == id);
        }
    }
}
