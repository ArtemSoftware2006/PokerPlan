
namespace Service.Helpers
{
    public class UniqueNameGenerator 
    {
        private List<string> names = new List<string>() {"Ёжик","Кролик", "Тортик", "Котик", "Булочка", "Пандочка"};
        public string GenerateNewName(List<string> names) {
            if (names.Count <= this.names.Count)
            {
                List<string> maybeNames = this.names.Except(names).ToList();

                return maybeNames[new Random().Next(0, maybeNames.Count - 1)];                
            }

            throw new IndexOutOfRangeException("Количество пользователей в группе больше, чем количество уникальных имён.");
        }
    }
}