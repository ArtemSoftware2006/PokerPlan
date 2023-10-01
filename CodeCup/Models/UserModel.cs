using Domain.Enum;

namespace Новая_папка.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        public Spectator isSpectator { get; set; }
    }   
}