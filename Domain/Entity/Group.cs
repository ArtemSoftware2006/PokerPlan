using Domain.Enum;

namespace Domain.Entity
{
    public class Group
    {
        public Guid Id { get; set; }   
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public StatusEntity Status { get; set; }
    }
}