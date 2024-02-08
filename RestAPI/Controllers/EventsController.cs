using ITB2203Application.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITB2203Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly DataContext _context;

    public EventsController(DataContext context)
    {
        _context = context;
    }
    [HttpGet]
    public ActionResult<IEnumerable<Event>> GetTests(string? name = null)
    {
        var query = _context.Events!.AsQueryable();

        if (name != null)
            query = query.Where(x => x.Name != null && x.Name.ToUpper().Contains(name.ToUpper()));

        return query.ToList();
    }
    [HttpGet("{id}")]
    public ActionResult<TextReader> GetTest(int id)
    {
        var @event = _context.Events!.Find(id);

        if (@event == null)
        {
            return NotFound();
        }

        return Ok(@event);
    }
    [HttpPut("{id}")]
    public IActionResult PutTest(int id, Event @event)
    {
        var dbEvent = _context.Events!.AsNoTracking().FirstOrDefault(x => x.Id == @event.Id);
        if (id != @event.Id || dbEvent == null)
        {
            return NotFound();
        }

        _context.Update(@event);
        _context.SaveChanges();

        return NoContent();
    }
    [HttpPost]
    public ActionResult<Event> PostTest(Event @event)
    {
        var dbExercise = _context.Events!.Find(@event.Id);
        if (dbExercise == null)
        {
            _context.Add(@event);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTest), new { Id = @event.Id }, @event);
        }
        else
        {
            return Conflict();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteTest(int id)
    {
        var @event = _context.Events!.Find(id);
        if (@event == null)
        {
            return NotFound();
        }

        _context.Remove(@event);
        _context.SaveChanges();

        return NoContent();
    }
}
