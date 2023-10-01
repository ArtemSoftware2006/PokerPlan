using Domain.Enum;

namespace Domain.ViewModel
{
    public class UserVm
    {
        public int Id { get; set; }
        public Spectator IsSpectator { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
    }
}