using Domain.Entity;
using Domain.Enum;

namespace Domain.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public DateTime DateCreated { get; set; }
        public List<Vote> Votes { get; set; }
    }
}