using Domain.Enum;

namespace Domain.ViewModel
{
    public class GroupVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CardSet CardSet { get; set; }
    }
}