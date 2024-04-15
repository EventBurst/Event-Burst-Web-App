using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Event_Burst_Web_App.Models;

public class Sponsor
{
    [Key] public int Id { get; set; }
    
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? SponsorId { get; set; }

    [Required] public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required] [Phone] public string PhoneNumber { get; set; }

    [Required] public string Address { get; set; }

    [Required] public decimal Contribution { get; set; }
}