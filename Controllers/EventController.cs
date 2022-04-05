using event_api.Models;
using event_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace event_api.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    [HttpGet()]
    public ActionResult<List<Object>> Get([FromQuery] string type = null, [FromQuery] DateTime? date = null)
    {
        List<Tapahtuma> tapahtumat = EventService.Get(type, date);
        List<Object> data = new List<Object>();
        if (tapahtumat.Count == 0)
        {
            return NotFound();
        } else
        {
            foreach (Tapahtuma tapahtuma in tapahtumat) 
            {
                string henkilo = PersonService.Get(tapahtuma.HenkiloID).Nimi;
                data.Add(new {
                    tapahtuma.TapahtumaID,
                    tapahtuma.HenkiloID,
                    henkilo,
                    tapahtuma.Tyyppi,
                    tapahtuma.Aika
                });
            };
            return data;
        }
    }

    [HttpGet("{id}")]
    public ActionResult<Object> Get(int id)
    {
        Tapahtuma tapahtuma = EventService.Get(id);
        if (tapahtuma == null)
        {
            return NotFound();
        } else
        {
            string henkilo = PersonService.Get(tapahtuma.HenkiloID).Nimi;
            return new {
                tapahtuma.TapahtumaID,
                tapahtuma.HenkiloID,
                henkilo,
                tapahtuma.Tyyppi,
                tapahtuma.Aika
            };
        }
    }

    [HttpGet("getByPerson/{personId}")]
    public ActionResult<Object> GetByPerson(int personId)
    {
        List<Tapahtuma> tapahtumat = EventService.GetByPerson(personId);
        List<Object> data = new List<Object>();
        if (tapahtumat.Count == 0)
        {
            return NotFound();
        } else
        {
            foreach (Tapahtuma tapahtuma in tapahtumat) 
            {
                string henkilo = PersonService.Get(tapahtuma.HenkiloID).Nimi;
                data.Add(new {
                    tapahtuma.TapahtumaID,
                    tapahtuma.HenkiloID,
                    henkilo,
                    tapahtuma.Tyyppi,
                    tapahtuma.Aika
                });
            };
            return data;
        }
    }

    [HttpPost()]
    public IActionResult Create([FromBody] Tapahtuma tapahtuma)
    {
        if (tapahtuma == null)
        {
            return BadRequest();
        }
        EventService.AddEvent(tapahtuma);
        return CreatedAtAction(nameof(Create), new { id = tapahtuma.TapahtumaID }, tapahtuma);
    }

    [HttpDelete("{personId}")]
    public IActionResult Delete(int personId)
    {
        List<Tapahtuma> tapahtumat = EventService.GetByPerson(personId);

        if (tapahtumat == null)
        {
            return NotFound();
        }
        foreach (Tapahtuma tapahtuma in tapahtumat)
        {
            EventService.DeleteEvent(tapahtuma.TapahtumaID);
        }
        return NoContent();
    }

}
