using System.Runtime.InteropServices;

namespace Domain.Entity
{
    public class UserGroup
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Guid GroupId { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }
    }
}