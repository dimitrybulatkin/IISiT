using System;
using System.Collections.Generic;

namespace Tests_Project__Lab_1_
{
    // Класс рабочей памяти
    class WorkMemory
    {
        private List<Fact> factsList; // Список фактов
        private LogicalInferenceMechanism mechanism; // Механизм логического вывода

        // Инициализация переменных
        public WorkMemory()
        {
            factsList = new List<Fact>();
            mechanism = new LogicalInferenceMechanism(this);
            QuestionsCount = mechanism.GetQuestionsCount();
        }

        // Добавление факта в рабочую память
        public void Add(Fact fact) 
        {
            factsList.Add(fact);
        }

        // Проверка наличия факта в рабочей памяти по имени правила, из которого он был выведен
        public bool IsFactСontained(string factName) 
        {
            bool result = false;

            foreach (Fact fact in factsList)
            {
                if (fact.Name == factName)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        // Получение текста следующего вопроса и имени соответствующего правила
        public Tuple<string, string, List<string>> GetQuestionAndCurrentDefruleName() 
        {
            Tuple<string, string, List<string>> result = mechanism.GetQuestionAndCurrentDefruleName();
            string questionText = result.Item1;
            string defName = result.Item2;
            List<string> validAnswers = result.Item3;
            return Tuple.Create(questionText, defName, validAnswers);
        }

        // Обработка ответа пользователя путем добавления факта в рабочая память с заданным именем и значением
        public void ResponseProcessing(string factName, string value) 
        {
            Add(new Fact(factName, value));
        }

        // Возврат списка фактов, содержащихся в рабочей памяти, для механизма логического вывода
        public List<Fact> GetFactsList() 
        {
            return factsList;
        }

        // Перебор правил и добавление в рабочей памяти по критерию выполнения всех предпосылок правила
        public Tuple<string, bool> DefrulesEnumeration() 
        {
            return mechanism.DefrulesEnumeration();
        }

        // Очистка рабочей памяти
        public void Clear() 
        {
            factsList.Clear();
        }

        // Количество вопросов
        public int QuestionsCount
        {
            get;
            private set;
        }
    }
}
