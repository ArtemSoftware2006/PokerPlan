using Domain.Enum;
using Domain.Cards;

namespace Новая_папка.Models
{
    public class GroupModel
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string Link { get; set; }
        public string Id { get; set; }
        public KeyValuePair<string, int>[] CardsKey {get;set;}
        public Role Role { get; set; }
    }
}