using System.Linq;
using System.Threading.Tasks;
using HyppeDesafio.Commands;
using HyppeDesafio.Data;
using HyppeDesafio.Helpers;
using HyppeDesafio.Models;
using HyppeDesafio.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("auth")]
public class UserController : ControllerBase
{
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<dynamic>> Register([FromBody]UserRegister model, [FromServices]DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = await context.Users.AsNoTracking().Where(x => x.Email == model.Email).FirstOrDefaultAsync();
            if (user != null)
                return BadRequest(new { message = "Email já existente" });

            User newUser = new User();
            newUser.Email = model.Email;
            newUser.Password = Cript.CalculateHash(model.Password);


            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            var userResponse = await context.Users.AsNoTracking().Where(x => x.Email == model.Email).FirstOrDefaultAsync();

            var token = TokenService.GenerateToken(newUser);
            model.Password = null;
            return new
            {
                user = userResponse,
                token = token
            };
        }
        catch
        {
            return BadRequest(new { message = "Não foi possivel criar o usuario" });
        }


    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<dynamic>> Login([FromBody]UserLogin model, [FromServices]DataContext context)
    {
        var user = await context.Users.AsNoTracking().Where(x => x.Email == model.Email).FirstOrDefaultAsync();

        if (user == null || !Cript.CheckMatch(user.Password, model.Password))
            return NotFound(new { message = "Usuário ou Senha inválidos" });

        var token = TokenService.GenerateToken(user);
        user.Password = null;
        return new
        {
            user = user,
            token = token
        };

    }
}