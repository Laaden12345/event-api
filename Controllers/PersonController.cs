using event_api.Models;
using event_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace event_api.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    [HttpGet("{id}")]
    public ActionResult<Object> Get(int id)
    {
        Object data;
        Henkilo henkilo = PersonService.Get(id);
        if (henkilo == null)
        {
            return NotFound();
        } else
        {
            List<Tapahtuma> tapahtumat = EventService.GetByPerson(id);
            data = new {
                henkilo.HenkiloID,
                henkilo.Nimi,
                henkilo.Syntymaaika,
                tapahtumat
            };
            return data;
        }
    }

}
