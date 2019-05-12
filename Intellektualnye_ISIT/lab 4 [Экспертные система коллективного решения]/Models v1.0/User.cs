using System.Collections.Generic;

namespace Models_v1._0
{
    // Класс пользователя
    class User
    {
        public User()
        {
            GetPreferences = new List<int>();
        }

        // Список предпочтений пользователя
        public List<int> GetPreferences { get; }

        // Добавить вариант в список предпочтений
        public void AddPreference(int value)
        {
            GetPreferences.Add(value);
        }
    }
}