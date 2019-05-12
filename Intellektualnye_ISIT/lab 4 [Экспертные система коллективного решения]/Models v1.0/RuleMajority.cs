using System;
using System.Collections.Generic;

namespace Models_v1._0
{
    // Правило Большинства
    class RuleMajority: Basic
    {
        // Название правила
        public string NameRule { get; set; }

        // Конструктор класса
        public RuleMajority(int countVar, List<User> users)
        {
            this.countVar = countVar;
            countVotesVariants = new int[countVar];
            this.users = users;
            NameRule = "Правило большинства";
        }

        // Получение номера варианта-победителя
        public int IndWinner()
        {
            // Подсчёт кол-ва голосов
            for (int i = 0; i < countVar; i++)
            {
                foreach (User user in users)
                {
                    if (user.GetPreferences[0] == (i + 1))
                        countVotesVariants[i]++;
                }
            }

            // Запись результата
            WriteData(countVotesVariants, this.NameRule);

            return iMax(countVotesVariants);
        }
    }
}