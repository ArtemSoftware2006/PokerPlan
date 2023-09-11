using Domain.Entity;

namespace Domain.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime DateCreated { get; set; }
        public UserGroup UserGroup { get; set; }
        public List<Vote> Votes { get; set; }
    }
}