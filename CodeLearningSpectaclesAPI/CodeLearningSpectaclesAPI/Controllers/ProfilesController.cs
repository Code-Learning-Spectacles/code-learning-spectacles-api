﻿using System;
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
  public class ProfilesController : ControllerBase
  {
    private readonly CodeLearningDbContext _context;

    public ProfilesController(CodeLearningDbContext context)
    {
      _context = context;
    }

    // GET: api/Profiles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Profile>>> GetProfiles([FromQuery] string? name)
    {
      if (!string.IsNullOrEmpty(name))
      {
        var profile = await _context.Profiles.FirstOrDefaultAsync(p => p.Name == name);

        if (profile == null)
        {
          return NotFound();
        }

        return Ok(new List<Profile> { profile });
      }

      return Ok(await _context.Profiles.ToListAsync());
    }

    // GET: api/Profiles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Profile>> GetProfile(int id)
    {
      var profile = await _context.Profiles.FindAsync(id);

      if (profile == null)
      {
        return NotFound();
      }

      return Ok(profile);
    }

    // PUT: api/Profiles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProfile(int id, Profile profile)
    {
      if (id != profile.Profileid)
      {
        return BadRequest();
      }

      _context.Entry(profile).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ProfileExists(id))
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

    // POST: api/Profiles
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Profile>> PostProfile([FromBody] Profile profile)
    {
      _context.Profiles.Add(profile);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetProfile", new { id = profile.Profileid }, profile);
    }

    // DELETE: api/Profiles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProfile(int id)
    {
      var profile = await _context.Profiles.FindAsync(id);
      if (profile == null)
      {
        return NotFound();
      }

      _context.Profiles.Remove(profile);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool ProfileExists(int id)
    {
      return _context.Profiles.Any(e => e.Profileid == id);
    }
  }
}
