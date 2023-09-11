namespace Domain.Entity
{
    public class Vote 
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int Value { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public Voting Voting { get; set; }
    }
}