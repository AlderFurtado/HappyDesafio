using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HyppeDesafio.Commands
{
    public class EventCreate
    {



        [Required(ErrorMessage = "Campo obrigatorio")]
        public string Name { get; set; }

        public DateTime Date { get; set; }

    }
}