using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendeesController : ControllerBase
{
    private readonly DataContext _context;

    public AttendeesController(DataContext context)
    {
        _context = context;
    }
    [HttpGet]
    public ActionResult<IEnumerable<Attendee>> GetTests(string? name = null)
    {
        var query = _context.Attendees!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

        return query.ToList();
    }
    [HttpGet("{id}")]
    public ActionResult<TextReader> GetTest(int id)
    {
        var attendee = _context.Attendees!.Find(id);

        if (attendee == null)
        {
            return NotFound();
        }

        return Ok(attendee);
    }
    [HttpPut("{id}")]
    public IActionResult PutTest(int id, Attendee attendee)
    {
        var dbAttendee = _context.Attendees!.AsNoTracking().FirstOrDefault(x => x.Id == attendee.Id);
        if (id != attendee.Id || dbAttendee == null)
        {
            return NotFound();
        }

        _context.Update(attendee);
        _context.SaveChanges();

        return NoContent();
    }
    [HttpPost]
    public ActionResult<Attendee> PostTest(Attendee attendee)
    {
        var dbExercise = _context.Speakers!.Find(attendee.Id);
        if (dbExercise == null)
        {
            _context.Add(attendee);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTest), new { Id = attendee.Id }, attendee);
        }
        else
        {
            return Conflict();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTest(int id)
    {
        var attendee = _context.Attendees!.Find(id);
        if (attendee == null)
        {
            return NotFound();
        }

        _context.Remove(attendee);
        _context.SaveChanges();

        return NoContent();
    }
}
