using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HyppeDesafio.Commands
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Campo obrigatorio")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatorio")]
        public string Password { get; set; }
    }
}