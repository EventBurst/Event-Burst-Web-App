using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Event_Burst_Web_App.Models
{
    public class Session
    {
        [Key]
        public int Id { get; set; }
        
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? SessionId { get; set; } // Adding Session ID

        public List<Speaker> Speakers { get; set; }
        public String speakerId { get; set; }
        public String agendaId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public List<Agenda> Agendas { get; set; }
    }
}