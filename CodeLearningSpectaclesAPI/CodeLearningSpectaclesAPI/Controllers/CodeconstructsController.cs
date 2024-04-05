using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeLearningSpectaclesAPI.Models;
using CodeLearningSpectaclesAPI.Data;
using Microsoft.AspNetCore.Http.Features;
using CodeLearningSpectaclesAPI.Auth;

namespace CodeLearningSpectaclesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeconstructsController : ControllerBase
    {
        private readonly CodeLearningDbContext _context;

        public CodeconstructsController(CodeLearningDbContext context)
        {
            _context = context;
        }

        // GET: api/Codeconstructs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Codeconstruct>>> GetCodeconstructs()
        {
            if (HttpContext.Items.ContainsKey("User") && HttpContext.Items["User"] is AuthObject userObj)
            {
                string username = userObj.login;
                // TODO: Return only items for user
            }
            return await _context.Codeconstructs.ToListAsync();
        }

        // GET: api/Codeconstructs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Codeconstruct>> GetCodeconstruct(int id)
        {
            var codeconstruct = await _context.Codeconstructs.FindAsync(id);

            if (codeconstruct == null)
            {
                return NotFound();
            }

            return codeconstruct;
        }

        // PUT: api/Codeconstructs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCodeconstruct(int id, Codeconstruct codeconstruct)
        {
            if (id != codeconstruct.Codeconstructid)
            {
                return BadRequest();
            }

            _context.Entry(codeconstruct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodeconstructExists(id))
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

        // POST: api/Codeconstructs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Codeconstruct>> PostCodeconstruct(Codeconstruct codeconstruct)
        {
            _context.Codeconstructs.Add(codeconstruct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCodeconstruct", new { id = codeconstruct.Codeconstructid }, codeconstruct);
        }

        // DELETE: api/Codeconstructs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCodeconstruct(int id)
        {
            var codeconstruct = await _context.Codeconstructs.FindAsync(id);
            if (codeconstruct == null)
            {
                return NotFound();
            }

            _context.Codeconstructs.Remove(codeconstruct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CodeconstructExists(int id)
        {
            return _context.Codeconstructs.Any(e => e.Codeconstructid == id);
        }
    }
}
