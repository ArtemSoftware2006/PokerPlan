
namespace Service.Helpers
{
    public class UniqueNameGenerator
    {
        private List<string> names = new List<string>() { "Ёжик", "Кролик", "Тортик", "Котик", "Булочка", "Пандочка" };
        public string GenerateNewName(List<string> AlreadyUsedNames)
        {
            if (AlreadyUsedNames.Count <= this.names.Count)
            {
                List<string> maybeNames = this.names.Except(AlreadyUsedNames).ToList();

                return maybeNames[new Random().Next(0, maybeNames.Count - 1)];
            }

            throw new IndexOutOfRangeException("Количество пользователей в группе больше, чем количество уникальных имён.");
        }
    }
}