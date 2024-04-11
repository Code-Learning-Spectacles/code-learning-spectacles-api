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
    public class ProfilelanguageconstructsController : ControllerBase
    {
        private readonly CodeLearningDbContext _context;

        public ProfilelanguageconstructsController(CodeLearningDbContext context)
        {
            _context = context;
        }

        // GET: api/Profilelanguageconstructs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profilelanguageconstruct>>> GetProfilelanguageconstructs()
        {
            return Ok(await _context.Profilelanguageconstructs.ToListAsync());
        }

        // GET: api/Profilelanguageconstructs/getByProfile/5
        [HttpGet("getByProfile/{profileId}")]
        public async Task<ActionResult<IEnumerable<Profilelanguageconstruct>>> GetProfilelanguageconstructsByProfile(int profileId)
        {
            var profileLanguageConstructs = await _context.Profilelanguageconstructs
                                                .Where(p => p.Profileid == profileId)
                                                .ToListAsync();
            return Ok(profileLanguageConstructs);
        }

        // GET: api/Profilelanguageconstructs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profilelanguageconstruct>> GetProfilelanguageconstruct(int id)
        {
            var profilelanguageconstruct = await _context.Profilelanguageconstructs.FindAsync(id);

            if (profilelanguageconstruct == null)
            {
                return NotFound();
            }

            return Ok(profilelanguageconstruct);
        }

        // PUT: api/Profilelanguageconstructs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfilelanguageconstruct(int id, [FromBody] Profilelanguageconstruct profilelanguageconstruct)
        {
            if (id != profilelanguageconstruct.Profilelanguageconstructid)
            {
                return BadRequest();
            }

            _context.Entry(profilelanguageconstruct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfilelanguageconstructExists(id))
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

        // POST: api/Profilelanguageconstructs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Profilelanguageconstruct>> PostProfilelanguageconstruct(Profilelanguageconstruct profilelanguageconstruct)
        {
            _context.Profilelanguageconstructs.Add(profilelanguageconstruct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfilelanguageconstruct", new { id = profilelanguageconstruct.Profilelanguageconstructid }, profilelanguageconstruct);
        }

        // DELETE: api/Profilelanguageconstructs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfilelanguageconstruct(int id)
        {
            var profilelanguageconstruct = await _context.Profilelanguageconstructs.FindAsync(id);
            if (profilelanguageconstruct == null)
            {
                return NotFound();
            }

            _context.Profilelanguageconstructs.Remove(profilelanguageconstruct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfilelanguageconstructExists(int id)
        {
            return _context.Profilelanguageconstructs.Any(e => e.Profilelanguageconstructid == id);
        }
    }
}
