using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HyppeDesafio.Models
{

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatorio")]
        public string Email { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Campo obrigatorio")]
        public string Password { get; set; }

        [JsonIgnore]
        public List<EventUser> EventUsers { get; set; }

    }

}