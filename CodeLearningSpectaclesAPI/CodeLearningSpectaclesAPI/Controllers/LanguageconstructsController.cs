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
    public class LanguageconstructsController : ControllerBase
    {
        private readonly CodeLearningDbContext _context;

        public LanguageconstructsController(CodeLearningDbContext context)
        {
            _context = context;
        }

        // GET: api/Languageconstructs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Languageconstruct>>> GetLanguageconstructs()
        {
            return Ok(await _context.Languageconstructs.ToListAsync());
        }

        // GET: api/Languageconstructs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Languageconstruct>> GetLanguageconstruct(int id)
        {
            var languageconstruct = await _context.Languageconstructs.FindAsync(id);

            if (languageconstruct == null)
            {
                return NotFound();
            }

            return Ok(languageconstruct);
        }

        // GET: api/Languageconstructs/getByLanguage/5
        [HttpGet("getByLanguage/{languageId}")]
        public async Task<ActionResult<IEnumerable<Languageconstruct>>> GetLanguageconstructsByLanguage(int languageId)
        {
            var languageConstructs = await _context.Languageconstructs
                                                .Where(l => l.Codinglanguageid == languageId)
                                                .ToListAsync();
            return Ok(languageConstructs);
        }

        // GET: api/Languageconstructs/getByLanguage/getByConstructId/3/5
        [HttpGet("getByLanguage/getByConstructId/{languageId}/{constructId}")]
        public async Task<ActionResult<IEnumerable<Languageconstruct>>> GetLanguageconstructsByLanguageByConstructId(int languageId, int constructId)
        {
            var languageConstructs = await _context.Languageconstructs
                                                .Where(l => l.Codinglanguageid == languageId)
                                                .Where(c => c.Codeconstructid == constructId)
                                                .ToListAsync();
            return Ok(languageConstructs);
        }

        // PUT: api/Languageconstructs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLanguageconstruct(int id, Languageconstruct languageconstruct)
        {
            if (id != languageconstruct.Languageconstructid)
            {
                return BadRequest();
            }

            _context.Entry(languageconstruct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageconstructExists(id))
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

        // POST: api/Languageconstructs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Languageconstruct>> PostLanguageconstruct(Languageconstruct languageconstruct)
        {
            _context.Languageconstructs.Add(languageconstruct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLanguageconstruct", new { id = languageconstruct.Languageconstructid }, languageconstruct);
        }

        // DELETE: api/Languageconstructs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLanguageconstruct(int id)
        {
            var languageconstruct = await _context.Languageconstructs.FindAsync(id);
            if (languageconstruct == null)
            {
                return NotFound();
            }

            _context.Languageconstructs.Remove(languageconstruct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LanguageconstructExists(int id)
        {
            return _context.Languageconstructs.Any(e => e.Languageconstructid == id);
        }
    }
}
