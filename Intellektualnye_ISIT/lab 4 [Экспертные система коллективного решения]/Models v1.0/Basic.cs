using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Models_v1._0
{
    // Базовый класс для моделей
    class Basic
    {
        protected int countVar;// Количество гипотез
        protected int[] countVotesVariants;// Массив с количеством голосов для каждой гипотезы
        protected List<User> users;// Список предпочтений пользователей

        protected List<User> noRepeatPreference;// Список неповторяющихся предпочтений
        protected List<int> countVotesPreference;// Список с количеством голосов по каждому из предпочтений

        StreamWriter writer;

        // Получение индекса с самым большим элементом в переданном массиве чисел
        public int iMax(int[] values)
        {
            int max = values[0];
            int iMax = 0;

            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] >= max)
                {
                    max = values[i];
                    iMax = i;
                }
            }

            return iMax;
        }

        // Формирование списка неповторяющихся предпочтений
        protected void NoRepeatPreference()
        {
            // Первый цикл перебирающий пользователей
            foreach (User user_1 in users)
            {
                bool equal = false;

                // Проверка того, что уже был найдено совпадение
                foreach (var pref in noRepeatPreference)
                    if (pref.GetPreferences.SequenceEqual(user_1.GetPreferences))
                    {
                        equal = true;
                        break;
                    }

                // Если уже найден, то переход к следующему пользователю
                if (equal)
                    continue;

                // Счетчик найденых совпадений
                int count = 0;

                // Второй вложеный цикл ищет совпадение в том же списке совпадение по предпочтениям
                foreach (User user_2 in users)
                    if (user_1.GetPreferences.SequenceEqual(user_2.GetPreferences))
                        count++;

                // Добавление в список число одинаковых предпочтений
                countVotesPreference.Add(count);

                if (!noRepeatPreference.Contains(user_1))
                    noRepeatPreference.Add(user_1);
            }
        }

        // Запись 
        protected void WriteData(int[] data, string nameMethod)
        {
            using (writer = new StreamWriter("info.txt", true, Encoding.Default))
            {
                writer.WriteLine(nameMethod);
                foreach (int value in data)
                    writer.WriteLine(value.ToString());
                writer.WriteLine();
            }
        }
    }
}