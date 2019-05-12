using System.Collections.Generic;
using System.Linq;

namespace Models_v1._0
{
    // Модель Кондорсе (Кондорет, Копленда и Симпсона)
    class RuleCondorcet : Basic
    {
        List<Pair> pairs; // Список всех возможнных варинтов пар
        List<Pair> resultPairs; // Список пар, из которых делается вывод

        RuleSimpson simpson; // Объект для правила Симпсона

        // Имена правил
        public string NameRuleCondorcet { get; set; }
        public string NameRuleCoplend { get; set; }
        public string NameRuleSimpson;

        // Структура для хранения информации о паре
        public struct Pair
        {
            public int one, two;

            public Pair(int one, int two)
            {
                this.one = one;
                this.two = two;
            }
        }

        // Конструктор класса
        public RuleCondorcet(int countVar, List<User> users)
        {
            this.users = users;
            noRepeatPreference = new List<User>();
            countVotesPreference = new List<int>();
            pairs = new List<Pair>();
            resultPairs = new List<Pair>();
            this.countVar = countVar;
            countVotesVariants = new int[countVar];

            simpson = new RuleSimpson(noRepeatPreference, countVotesPreference, countVar);

            NameRuleCondorcet = "Правило Кондорсе";
            NameRuleCoplend = "Правило Копленда";
            NameRuleSimpson = simpson.NameRule;
        }

        // Получение номера варианта-победителя 
        public int IndWinner()
        {
            NoRepeatPreference(); // Список неповторяющихся предпочтений
            AllPair(); // Формирование комбинаций всех возможных пар
            FindResultPairs(); // Обработка пар с результатами
            return iMax(); // Получение ответа
        }

        // Формирование комбинаций всех возможных пар. Например: a,b  a,c  b,c
        private void AllPair()
        {
            for (int i = 0; i < countVar; i++)
            {
                for (int j = 0; j < countVar; j++)
                {
                    Pair pair = new Pair(i, j);
                    if (i != j && !FindPair(pair))
                        pairs.Add(pair);
                }
            }
        }

        // Проверка наличия существования пары в списке
        private bool FindPair(Pair pair)
        {
            bool check = false;

            foreach (Pair p in pairs)
            {
                if (p.one == pair.one && p.two == pair.two || 
                    p.two == pair.one && p.one == pair.two)
                {
                    check = true;
                    break;
                }
            }

            return check;
        }

        // Обработка пар с результатами
        private void FindResultPairs()
        {
            // Цикл с проходом по списку пар
            foreach (Pair pair in pairs)
            {
                int maxOne, maxTwo, summ;
                maxOne = maxTwo = summ = 0;

                // Обработка каждого элемента в паре
                for (int k = 0; k < 2; k++)
                {
                    int one, two, iOne, iTwo;
                    iOne = iTwo = 0;

                    one = pair.two + 1;
                    two = pair.one + 1;

                    // Цикл для прохода списка неповторяющихся предпочтений
                    for (int i = 0; i < noRepeatPreference.Count; i++)
                    {
                        // Вложенный цикл для прохода списка неповторяющихся предпочтений для очередного предпочтения
                        for (int j = 0; j < noRepeatPreference[i].GetPreferences.Count; j++)
                        {
                            if (noRepeatPreference[i].GetPreferences[j] == one)
                                iOne = j;
                            if (noRepeatPreference[i].GetPreferences[j] == two)
                                iTwo = j;
                        }
                        if (iOne < iTwo)
                        {
                            summ += countVotesPreference[i];
                        }

                        if (k == 0)
                        {
                            maxOne = summ;
                        }
                        else
                        {
                            maxTwo = summ;
                        }
                    }
                    summ = 0;
                }

                // Добавление в список с результатами
                if (maxOne > maxTwo)
                    resultPairs.Add(new Pair(pair.one, pair.two));
                else
                    resultPairs.Add(new Pair(pair.two, pair.one));
            }
        }

        //Функция для получения ответа
        private int iMax()
        {
            //Подсчет кол-ва голосов
            for (int i = 0; i < countVar; i++)
            {
                foreach (Pair pair in resultPairs)
                {
                    if (pair.one == i)
                        countVotesVariants[i]++;
                }
            }

            // Поиск наибольшего предпочтения с наибольшим числом голосов 
            int max = countVotesVariants.Max();
            int countMax = 0;

            // Запись в файл результата
            WriteData(countVotesVariants, NameRuleCondorcet);

            //Есть ли парадокс
            foreach (int num in countVotesVariants)
            {
                if (num == max)
                    countMax++;
            }

            // Найден парадокс
            if (countMax > 1)
                return -1;
            //...если нет, то найден явный победитель
            else
                return iMax(countVotesVariants);
        }

        // Функция получения победителя по правилу Симпсона
        public int Simpson()
        {
            return simpson.IndWinner();
        }

        // Функция получения победителя по правилу Копленда
        public int Coplend()
        {
            int[] coplend = new int[countVar];

            for (int i = 0; i < countVar; i++)
                coplend[i] = countVotesVariants[i] - ((countVar - 1) - countVotesVariants[i]);

            WriteData(coplend, NameRuleCoplend);

            return iMax(coplend);
        }

        // Функция получения списка с количеством голосов по каждому из предпочтений
        public List<int> GetVotes
        {
            get { return countVotesPreference; }
        }

        // Функция для получения списка неповторяющихся предпочтений
        public List<User> GetPreferences
        {
            get { return noRepeatPreference; }
        }
    }
}