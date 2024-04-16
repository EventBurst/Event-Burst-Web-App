using Event_Burst_Web_App.Models;

public class Event
    {
        public string _id  { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; }
        public List<Sponsor> Sponsors { get; set; }
        public List<string> SponsorIds { get; set; }
        public List<Session> Sessions { get; set; }
        public List<string> SessionIds { get; set; }
        public Organizer Organizer { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
