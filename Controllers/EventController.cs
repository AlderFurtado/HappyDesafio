

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HyppeDesafio.Commands;
using HyppeDesafio.Data;
using HyppeDesafio.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("events")]
public class EventController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Event>>> getEvents([FromServices]DataContext context)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(this.User.FindFirstValue(ClaimTypes.Sid)));

        try
        {
            var events = await context.Events.AsNoTracking().Where(x => x.UserCreator.Id == user.Id).Include(e => e.EventUsers).ThenInclude(q => q.User).ToListAsync();
            return Ok(events);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possivel encontrar os eventos" });
        }

    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Event>>> getEventbyId(int id, [FromServices]DataContext context)
    {
        try
        {
            var evento = await context.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (evento == null)
                return BadRequest(new { message = "Evento não existe" });
            return Ok(evento);
        }
        catch (System.Exception)
        {
            return BadRequest(new { message = "Não foi possivel encontrar o evento" });
        }

    }

    [HttpPut]
    [Route("addParticipante/{id:int}")]
    public async Task<ActionResult<List<Event>>> addParticipant(int id, [FromServices]DataContext context, [FromBody]List<AddParticipant> participants)
    {
        try
        {

            var evento = await context.Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (evento == null)
                return BadRequest(new { message = "Evento não existe" });


            foreach (var p in participants)
            {
                int idUser = (int)p.id;
                var user = new User();
                user = await context.Users.FirstOrDefaultAsync(x => x.Id == idUser);

                var eventUser = new EventUser
                {
                    EventId = evento.Id,
                    User = user,
                    UserId = user.Id,
                };

                await context.EventUsers.AddAsync(eventUser);
                await context.SaveChangesAsync();
            }

            return Ok(evento);
        }
        catch (System.Exception e)
        {
            return BadRequest(new { message = e });
        }

    }


    [HttpPost]
    [Route("")]
    public async Task<ActionResult<List<Event>>> createEvent([FromBody]EventCreate model, [FromServices]DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == int.Parse(this.User.FindFirstValue(ClaimTypes.Sid)));

            Event newEvent = new Event();
            newEvent.Name = model.Name;
            newEvent.Date = model.Date;

            newEvent.UserCreator = user;
            context.Events.Add(newEvent);
            await context.SaveChangesAsync();
            return Ok(newEvent);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possivel criar o evento" });
        }

    }

    [HttpPut]
    [Route("acceptInvite/{id:int}")]
    public async Task<ActionResult<User>> acceptInvite(int id, [FromServices]DataContext context)
    {


        var eventUser = await context.EventUsers.FirstOrDefaultAsync(x => x.Id == id);

        if (int.Parse(this.User.FindFirstValue(ClaimTypes.Sid)) != eventUser.UserId)
        {

            return Forbid();
        }

        eventUser.Status = true;
        try
        {
            context.Entry<EventUser>(eventUser).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(eventUser);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possivel atualizar o evento" });
        }



    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Event>>> updateEvent(
        int id,
    [FromBody]Event model,
        [FromServices]DataContext context)
    {
        if (id != model.Id)
            return NotFound(new { message = "Evento não encontrado" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Entry<Event>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch
        {

            return BadRequest(new { message = "Não foi possivel atualizar o evento" });

        }

    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Event>>> updateEvent(int id, [FromServices]DataContext context)
    {
        var evento = await context.Events.FirstOrDefaultAsync(x => x.Id == id);
        if (evento == null)
            NotFound(new { message = "Evento não encontrado" });

        try
        {
            context.Events.Remove(evento);
            await context.SaveChangesAsync();
            return Ok(new { message = "Evento excluido com sucesso" });

        }
        catch
        {
            return BadRequest(new { message = "Não foi excluir o evento" });
        }
    }
}