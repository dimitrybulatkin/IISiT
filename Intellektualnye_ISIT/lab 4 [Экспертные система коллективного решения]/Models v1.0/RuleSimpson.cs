using System.Collections.Generic;

namespace Models_v1._0
{
    // Правило Симпсона
    class RuleSimpson: Basic
    {
        List<Pair> pairs;// Список всех возможных пар

        // Название правила
        public string NameRule { get; set; }

        // Структура пары
        public struct Pair
        {
            // Значения пары и кол-во голосов за данную пару
            public int one, two, value;

            public Pair(int one, int two, int value)
            {
                this.one = one;
                this.two = two;
                this.value = value;
            }
        }

        // Конструктор класса
        public RuleSimpson(List<User> noRepeatPreference, List<int> countVotesPreference, int countVar)
        {
            this.noRepeatPreference = noRepeatPreference;
            this.countVotesPreference = countVotesPreference;
            this.countVar = countVar;
            pairs = new List<Pair>();

            NameRule = "Правило Симпсона";
        }

        // Получение номера варианта-победителя
        public int IndWinner()
        {
            AllPairs();
            CountVotesPairs();

            return MinPairs();
        }

        // Формирование списка всех возможных пар
        private void AllPairs()
        {
            for (int i = 0; i < countVar; i++)
            {
                for (int j = 0; j < countVar; j++)
                {
                    // В паре не могут быть два одинаковых элемента
                    if (i != j) 
                        pairs.Add(new Pair(i, j, 0));
                }
            }
        }

        //Количество голосов, отданных за каждую пару
        private void CountVotesPairs()
        {
            // Циклом просматривается список всех возможных пар
            for (int i = 0; i < pairs.Count; i++)
            {
                // Сумма 
                int summ = 0;

                // Вложеным циклом просматривается список неповторяющихся предпочтений
                for (int k = 0; k < noRepeatPreference.Count; k++)
                {
                    int iOne, iTwo;
                    iOne = iTwo = 0;

                    // Дополнительным вложенным циклом просматривается список предпочтений пользователя
                    for (int j = 0; j < noRepeatPreference[k].GetPreferences.Count; j++)
                    {
                        if (noRepeatPreference[k].GetPreferences[j] == pairs[i].one + 1)
                            iOne = j;
                        if (noRepeatPreference[k].GetPreferences[j] == pairs[i].two + 1)
                            iTwo = j;
                    }
                    if (iOne < iTwo)
                        summ += countVotesPreference[k];
                }
                pairs[i] = new Pair(pairs[i].one, pairs[i].two, summ);
            }
        }

        // Выбирается номер с минимальной парой для каждого из вариантов
        private int MinPairs()
        {
            // Список пар с результатами
            List<Pair> result = new List<Pair>();

            // Перебор гипотез
            for (int i = 0; i < countVar; i++)
            {
                List<Pair> p = new List<Pair>();
                foreach (Pair pair in pairs)
                {
                    if (i == pair.one)
                        p.Add(pair);
                }

                Pair min = p[0];
                for (int j = 0; j < p.Count; j++)
                {
                    if (p[j].value < min.value)
                        min = p[j];
                }
                result.Add(min);
            }

            int[] data = new int[result.Count];
            for (int i = 0; i < result.Count; i++)
                data[i] = result[i].value;

            // Запись результата
            WriteData(data, this.NameRule);

            return iMax(data);
        }
    }
}