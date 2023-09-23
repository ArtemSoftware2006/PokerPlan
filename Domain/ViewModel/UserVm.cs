using Domain.Enum;

namespace Domain.ViewModel
{
    public class UserVm
    {
        public string Name { get; set; }
        public Role Role { get; set; }
        public int UserId { get; set; }
        public string  GroupId { get; set; }
    }
}