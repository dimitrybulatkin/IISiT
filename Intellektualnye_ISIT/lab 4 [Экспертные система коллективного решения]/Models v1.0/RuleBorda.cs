using System.Collections.Generic;

namespace Models_v1._0
{
    // Правило Борда
    class RuleBorda: Basic
    {
        List<int> points;// Очки за 1-е, 2-е и другие места
        int[] summPoints;// Количество очков для каждого варианта ответа
        
        // Название правила
        public string NameRule { get; set; }

        // Конструктор класса
        public RuleBorda(int countVar, List<User> users)
        {
            this.countVar = countVar;
            this.users = users;

            noRepeatPreference = new List<User>();
            countVotesPreference = new List<int>();
            points = new List<int>();

            summPoints = new int[countVar];
            NameRule = "Правило Борда";
        }

        // Получение номера варианта-победителя
        public int IndWinner()
        {
            NoRepeatPreference(); // Список неповторяющихся предпочтений
            CalcSummPoints(); // Суммы баллов для каждоый варианта ответа

            WriteData(summPoints, this.NameRule); // Запись в файл
            return iMax(summPoints);
        }

        // Подсчет суммы баллов для каждоый варианта ответа
        private void CalcSummPoints()
        {
            // Заполнение баллами за каждый вариант с учетом числа гипотез
            for (int i = countVar; i >= 0; i--)
                points.Add(i);

            //Перебор гипотез
            for (int i = 0; i < countVar; i++)
            {
                int summ = 0;

                // Перебор голосов
                for (int j = 0; j < countVotesPreference.Count; j++)
                {
                    // Место гипотез среди текущего предпочтения
                    int iPref = noRepeatPreference[j].GetPreferences.FindIndex(y => y == (i + 1));
                    int temp = iPref >= 0 ? points[iPref] : 1;

                    summ += countVotesPreference[j] * temp;
                }

                summPoints[i] = summ;
            }
        }
    }
}