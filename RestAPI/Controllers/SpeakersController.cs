using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpeakersController : ControllerBase
{
    private readonly DataContext _context;

    public SpeakersController(DataContext context)
    {
        _context = context;
    }
    [HttpGet]
    public ActionResult<IEnumerable<Speaker>> GetTests(string? name = null)
    {
        var query = _context.Speakers!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

        return query.ToList();
    }
    [HttpGet("{id}")]
    public ActionResult<TextReader> GetTest(int id)
    {
        var speaker = _context.Speakers!.Find(id);

        if (speaker == null)
        {
            return NotFound();
        }

        return Ok(speaker);
    }
    [HttpPut("{id}")]
    public IActionResult PutTest(int id, Speaker speaker)
    {
        var dbSpeaker = _context.Speakers!.AsNoTracking().FirstOrDefault(x => x.Id == speaker.Id);
        if (id != speaker.Id || dbSpeaker == null)
        {
            return NotFound();
        }

        _context.Update(speaker);
        _context.SaveChanges();

        return NoContent();
    }
    [HttpPost]
    public ActionResult<Speaker> PostTest(Speaker speaker)
    {
        var dbSpeaker = _context.Speakers!.Find(speaker.Id);
        if (!speaker.Email!.Contains("@"))
        {
            return BadRequest();
        }
        if (dbSpeaker == null)
        {
            _context.Add(speaker);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTest), new { Id = speaker.Id }, speaker);
        }
        else 
        {
            return Conflict();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTest(int id)
    {
        var speaker = _context.Speakers!.Find(id);
        if (speaker == null)
        {
            return NotFound();
        }

        _context.Remove(speaker);
        _context.SaveChanges();

        return NoContent();
    }
}
