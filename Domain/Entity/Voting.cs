namespace Domain.Entity
{
    public class Voting
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public List<Vote> Votes { get; set; }
    }
}