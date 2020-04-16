using System.ComponentModel.DataAnnotations;
using HyppeDesafio.Models;

namespace HyppeDesafio.Models
{
    public class EventUser
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public bool Status { get; set; }
    }
}
