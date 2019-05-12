using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormNODE
{
    // Перечисление типов возможных связей
    public enum LinksTypes { Is_Instance, AKO, Required, Located, Contains, NotRequired };

    // Класс механизма логического вывода
    public class InferenceEngine
    {
        private List<Node> nodes; // Список всех узлов
        private List<Question> questions; // Список всех вопросов
        private List<string> nodesNames; // Список названий всех узлов
        private List<Node> passedNodes; // Списох пройденых узлов

        // Конструктор класса
        public InferenceEngine(List<Node> nodes, List<Question> questions)
        {
            this.nodes = nodes;
            this.questions = questions;
            InferenceResult = false;
            nodesNames = new List<string>();
            passedNodes = new List<Node>();
        }

        // Функция для получение логического вывода с указанием родительского и дочернего узлов, и типа вопроса
        public bool GetConclusion(string parentNodeName, string dependentNodeName, string questionText)
        {
            Node parentNode = GetNodeByName(parentNodeName); // Родительский узел
            Node dependentNode = GetNodeByName(dependentNodeName); // Дочерний узел
            Question question = GetQuestionByText(questionText); // Связывающий вопрос

            // Цикл просматривающий все связи и дочерние узлы в них
            foreach (Link link in parentNode.GetLinks())
            {
                // Добовление в список пройденых узлов
                passedNodes.Add(link.ChildNode);

                // Содержится ли в вопросе рассматриваемый тип связи (является ли рассматриваемый тип связи ассоциируемым для заданного вопроса)
                if (question.GetLinks().Contains(link.LinkType)) 
                {
                    passedNodes.Add(link.ChildNode);

                    // Если найден нужный узел с нужной связью, то поднимается флаг об успехе
                    if (link.ChildNode == dependentNode)
                        InferenceResult = true;
                    // ...иначе запускатся рекурсивный просмотр всех дочернего узла у очередной связи
                    else
                        GetConclusion(link.ChildNode.Name, dependentNode.Name, questionText);
                }
            }

            // Возврат результата
            return InferenceResult;
        }

        // Функция для получения списка дочерних узлов по имени 
        public List<string> GetChildNodesNamesByNodeName(string nodeName)
        {
            // Целевого узел
            Node target = GetNodeByName(nodeName);

            // Получение всех дочерних узлов для целевого узла
            foreach (Link link in target.GetLinks())
                if (link.LinkType == LinksTypes.AKO || link.LinkType == LinksTypes.Is_Instance)
                {
                    nodesNames.Add(link.ChildNode.Name);
                    GetChildNodesNamesByNodeName(link.ChildNode.Name);
                }

            return nodesNames;
        }

        // Получение вопроса по названию
        private Question GetQuestionByText(string text)
        {
            foreach (Question question in questions)
                if (question.Text == text)
                    return question;

            return null;
        }

        // Получение узла по названию
        private Node GetNodeByName(string name)
        {
            foreach (var node in nodes)
                if (node.Name == name)
                    return node;

            return null;
        }

        // Флаг для обозначения успеха онаружения двух узлов с нужным типом связи
        private bool InferenceResult
        {
            get;
            set;
        }

        // Список пройденых узлов
        public List<Node> GetPassedNodes()
        {
            return passedNodes;
        }
    }
}
