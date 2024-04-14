public class Event
    {
        public string _id  { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; }
        public List<Sponsor> Sponsors { get; set; }
        public List<object> Sessions { get; set; }
        public Organizer Organizer { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
