using Domain.Enum;

namespace Domain.Entity
{
    public class Group
    {
        public int Id { get; set; }   
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public StatusEntity Status { get; set; }
        public Voting Voting { get; set; }
        public UserGroup UserGroup { get; set; }
    }
}