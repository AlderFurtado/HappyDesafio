using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HyppeDesafio.Models
{
    public class Event
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatorio")]
        public string Name { get; set; }

        public DateTime Date { get; set; }

        public User UserCreator { get; set; }

        public List<EventUser> EventUsers { get; set; }

    }
}