using System;
using System.IO;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace Tests_Project__Lab_1_
{
    class LogicalInferenceMechanism
    {
        private XDocument xml; // Для хранения объекта xml-документа с правилами (test.xml)
        private WorkMemory workMemory; // Ссылка на объект рабочей памяти

        // Инициализация переменных
        public LogicalInferenceMechanism(WorkMemory workMemory)
        {
            string path = Directory.GetCurrentDirectory() + "/test.xml";
            xml = XDocument.Load(path);
            this.workMemory = workMemory;
        }

        // Получение текста следующего вопроса и имени соответствующего правила
        public Tuple<string, string, List<string>> GetQuestionAndCurrentDefruleName() 
        {
            string defName = string.Empty, questionText = string.Empty;
            List<string> validAnswers = new List<string>();

            // Цикл с вложеным циклом для прохода по узлам в xml-файле для получения фактов
            foreach (var defrule in xml.Root.Elements())
                foreach (var attribute in defrule.Attributes())
                {
                    defName = defrule.Attribute("name").Value;
                    defName = defName.Remove(defName.IndexOf("Defrule"), "Defrule".Length);

                    // Добавление нового вопроса для факта, если его еще нет в списке ответов пользователя
                    if (attribute.Name == "question" && !workMemory.IsFactСontained(defName))
                    {     
                        questionText = defrule.Attribute("question").Value;
                        string answersValues = defrule.Attribute("validAnswers").Value;
                        validAnswers = answersValues.Split(',').ToList();
                        return Tuple.Create(questionText, defName, validAnswers);
                    }
                }

            return null;
        }

        // Перебор правил и добавление выведенных фактов в рабочую память 
        public Tuple<string, bool> DefrulesEnumeration() 
        {
            string factName = null;
            bool systemExit = false;

            // Цикл для прохода по узлам в xml-файле
            foreach (XElement defrule in xml.Root.Elements())
            {
                // Если элемент не имеет дочерних элементов (предпосылок), нет смысла проверять его
                if (defrule.HasElements) 
                {
                    int premisesCount = defrule.Elements().Count();
                    int equalsPairsCount = 0;

                    
                    foreach (XElement premise in defrule.Elements())
                        foreach (Fact fact in workMemory.GetFactsList())
                        {
                            // Если имена факта из рабочей памяти и предпосылки совпадают и совпадают их значения, то счетчик сравнения тикает
                            if (fact.Name == premise.Attribute("name").Value && fact.Value == premise.Attribute("value").Value) 
                            {
                                equalsPairsCount++;
                                break;
                            }
                        }

                    factName = defrule.Attribute("name").Value;
                    factName = factName.Remove(factName.IndexOf("Defrule"), "Defrule".Length);

                    // Если кол-во "равных" пар и кол-во предпосылок текущего правила равны, это означает, что можно занести новый выведенный факт в рабочей памяти
                    if (equalsPairsCount == premisesCount && !workMemory.IsFactСontained(factName))
                    {
                        workMemory.Add(new Fact(factName, "да"));

                        // Если факт добавлен в РП, стоит проверить, не является ли он университетом
                        foreach (XAttribute attribute in defrule.Attributes()) 
                            if (attribute.Name == "univName")
                            {
                                systemExit = true;
                                factName = attribute.Value;
                                break;
                            }
                    }

                    if (systemExit)
                        break;
                }
            }

            return Tuple.Create(factName, systemExit);
        }

        // Подсчет числа вопросов
        public int GetQuestionsCount()
        {
            int count = 0;

            // Цикл с вложеным циклом для подсчета узлов в файле с меткой вопроса
            foreach (var elem in xml.Root.Elements())
                foreach (var attribute in elem.Attributes())
                    if (attribute.Name == "question")
                    {
                        count++;
                        break;
                    }

            return count;
        }
    }
}
