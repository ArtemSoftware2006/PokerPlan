using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Vote 
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Value { get; set; }
        public string Key { get; set; }
        public int UserId { get; set; }
        public Guid GroupId { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }
    }
}