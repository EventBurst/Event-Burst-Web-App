using System;
using System.ComponentModel.DataAnnotations;

namespace Event_Burst_Web_App.Models
{
    public class Session
    {
        [Key]
        public int Id { get; set; }
        
        public int SessionId { get; set; } // Adding Session ID

        public int SpeakerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string AgendaId { get; set; }
    }
}