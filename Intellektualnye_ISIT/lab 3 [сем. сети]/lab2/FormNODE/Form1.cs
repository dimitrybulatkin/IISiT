using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FormNODE
{
    public partial class Form1 : Form
    {
        private InferenceEngine engine; // Экземпляр механизма логического вывода
        private List<Node> nodes; // Список всех узлов
        private List<Question> questions; // Список всех вопросов
        private List<Node> passedNodes; // Список пройденых узлов

        // Конструктор формы с инициализацией переменных
        public Form1()
        {
            InitializeComponent();
            nodes = Filler.NodesFill();
            questions = Filler.QuestionsListFill();
            FillComboboxes();
        }

        // Заполнение выподающих списков на форме
        private void FillComboboxes()
        {
            foreach (var node in nodes)
            {
                NodesCmb1.Items.Add(node.Name);
                NodesCmb2.Items.Add(node.Name);
                NodesCmb3.Items.Add(node.Name);
            }

            foreach (var question in questions)
                QuestionsCmb.Items.Add(question.Text);
        }

        // Обработка нажатия кнопки для проверки связи между указаными узлами
        private void GetConclusionBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string dependentNodeName = NodesCmb1.Text; // Дочерний узел
                string parentNodeName = NodesCmb2.Text; // Родительский узел
                string questionText = QuestionsCmb.Text; // Тип вопроса

                // Выбрасывается исключение, если хотя бы в одном из выпадающих списков не был выбран вариант ответа
                if (dependentNodeName == string.Empty || parentNodeName == string.Empty || questionText == string.Empty)
                    throw new Exception("Выберите вершины и тип вопроса.");

                // При проверке создается новый экземпляр механизма логического вывода
                engine = new InferenceEngine(nodes, questions);

                // Выводится результат проверки наличия такой связи
                bool result = engine.GetConclusion(parentNodeName, dependentNodeName, questionText);

                MessageBox.Show(result.ToString());
            }
            catch (Exception ex)
            {
                // Вывод ошибки в случае ошибки
                MessageBox.Show(ex.Message);
            }
        }

        // Обработка нажатия кнопки для вывода дочерних узлов
        private void GetChildsNodesBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Должне быть выбран один из узлов в выпадающем списке
                string nodeName = NodesCmb3.Text;
                if (nodeName == string.Empty)
                    // Иначе выбрасывается исключение
                    throw new Exception("Выберите вершину.");

                // Предварительная очистка списка
                ChildsNodesListBox.Items.Clear();

                // Создание нового экземпляра МЛВ и получение нового списка узлов
                engine = new InferenceEngine(nodes, questions);
                List<string> nodesNames = engine.GetChildNodesNamesByNodeName(nodeName);

                // Вывод списка на форму
                if (nodesNames.Count == 0)
                    ChildsNodesListBox.Items.Add("Дочерних узлов не обнаружено.");
                else
                    foreach (string name in nodesNames)
                        ChildsNodesListBox.Items.Add(name);
            }
            catch (Exception ex)
            {
                // Вывод ошибки в случае ошибки
                MessageBox.Show(ex.Message);
            }
        }

        // Обработка нажатие кнопки для отображения списка пройденых вершин
        private void ExplanationComponentCallBtn_Click(object sender, EventArgs e)
        {
            // Проверка наличия экземпляра механизма логического вывода и факта того, что была проверка прохождения по узлам
            if (engine != null)
                passedNodes = engine.GetPassedNodes();

            // В случае отсутствия или неудачи проверки выводится сообщение об этом
            if (passedNodes == null || engine == null || passedNodes.Count == 0)
            {
                MessageBox.Show("Выполните хотя бы одну проверку.");
                return;
            }

            // В случае успеха выводится список пройденых узлов
            string result = "Имена пройденных по порядку узлов:\r\n";

            foreach (var node in passedNodes)
                result += node.Name + "\r\n";

            MessageBox.Show(result);
        }
    }
}
