using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tests_Project__Lab_1_
{
    public partial class Form1 : Form
    {
        private WorkMemory workMemory; // Рабочая память
        private string currentFactName; // Имя переменной, обозначающий текущий факта
        private int clicksCount, // Счетчик нажатий кнопки по кнопке для ответа
            askedQuestionCount; // Счетчик отвеченых вопросов
        private List<string> validAnswersList; // Все возможные ответы для текущего факта

        // Конструктор класса
        public Form1()
        {
            InitializeComponent();

            // Инициализация всех переменных
            clicksCount = 0;
            askedQuestionCount = 0;
            currentFactName = null;
            workMemory = new WorkMemory();
            SystemOutputTB.TabStop = false;
            SystemOutputTB.Text = "Вас приветствует экспертная система по выбору ВУЗа. Отвечайте на вопросы согласно правилам системы, пока не получите рекомендацию системы.\r\n\r\n";
            UserAnswerBTN.Text = "Начать";
        }

        // Обработка ответа пользователя
        private void AnswerTheQuestion() 
        {
            // Получение строки с ответом из поля на форме
            string answerValue = UserAnswerTB.Text;

            // Перебор все возможных ответов для факта
            foreach (string variant in validAnswersList)
            {
                // Если нашлось совпадение с одним из ответов,
                // то записывается ответ в память и очищается поле на форме поле для ввода
                if (variant == answerValue)
                {
                    SystemOutputTB.AppendText(" " + answerValue);
                    workMemory.ResponseProcessing(currentFactName, answerValue);
                    YesRB.Checked = NoRB.Checked = false;
                    SystemOutputTB.AppendText("\r\n\r\n");
                    UserAnswerTB.Focus();
                    UserAnswerTB.Clear();
                    return;
                }
            }

            // Если введеный ответ не является одним из возможных ответов,
            // то выбрасывается исключение и очищается поле для ответа
            UserAnswerTB.Clear();
            throw new Exception("Введите один из предложенных вариантов ответа.");
        }

        // Получение нового вопроса
        private void GetQuestion() 
        {
            // Структура кортежа:
            // 1 - текст вопроса,
            // 2 - имя факта, которому соответствует текущий вопрос,
            // 3 - список допустимых вариантов ответа
            Tuple<string, string, List<string>> result = workMemory.GetQuestionAndCurrentDefruleName();

            // Запись название факта и возможных ответов для него
            SystemOutputTB.AppendText(result.Item1);
            currentFactName = result.Item2;
            validAnswersList = result.Item3;
        }

        // Обработка нажатия кнопки ответа
        private void UserAnswerBTN_Click(object sender, EventArgs e)
        {
            try
            {
                // Если это было первое нажате кнопки "Ответить" (в начале работы программы 
                // она называется "Начать"), то идет переименовывание кнопки и активация кнопки
                // для отправки ответа
                if (++clicksCount == 1)
                {
                    UserAnswerBTN.Text = "Ответить";
                    UserAnswerTB.Enabled = true;
                }
                else
                {
                    // Если поле для ответа на форме пустая, то выбрасывается исключение
                    if (UserAnswerTB.Text == string.Empty)
                        throw new Exception("Поле ввода не должно быть пустым.");

                    // Вызов функции для обработки ответа
                    AnswerTheQuestion();

                    // Увеличение счетчика отвеченых вопросов
                    ++askedQuestionCount;

                    // ===
                    // Проверка условия прекращения работы системы

                    Tuple<string, bool> corteg = workMemory.DefrulesEnumeration();

                    // Флаг того, что очередной факт из рабочей памяти является логическим выводом с ответом
                    bool isFinal = corteg.Item2;

                    // Вывод логического ответа
                    if (isFinal)
                    {
                        SystemOutputTB.AppendText("Система советует поступать в " + corteg.Item1);
                        UserAnswerBTN.Enabled = false;
                        ExplanationComponentCallBtn.Enabled = true;
                        return;
                    }

                    // Если исчерпан лимит на ответы, то 
                    if (askedQuestionCount == workMemory.QuestionsCount)
                    {
                        SystemOutputTB.AppendText("Система не смогла выбрать ВУЗ, попробуйте еще раз.");
                        UserAnswerBTN.Enabled = false;
                        ExplanationComponentCallBtn.Enabled = true;
                        return;
                    }
                }

                // Вызов функции для получение нового вопроса
                // для определения следующего факта
                GetQuestion();
            }
            catch (Exception ex)
            {
                // В случае ошибки обработки ответа выводится соответствующий ответ
                MessageBox.Show(ex.Message);
            }
        }

        // Выбрано включение упрощенных вариантов ответов
        private void YesRB_CheckedChanged(object sender, EventArgs e)
        {
            UserAnswerTB.Text = "да";
        }

        // Выбрано отключение упрощенных вариантов ответов
        private void NoRB_CheckedChanged(object sender, EventArgs e)
        {
            UserAnswerTB.Text = "нет";
        }

        // Обработка нажатия кнопки для вызова компонента объяснения
        private void ExplanationComponentCallBtn_Click(object sender, EventArgs e)
        {
            // Формирования списка фактов (заданые вопросы и ответов на них)
            string result = "Последовательность выведенных фактов: \r\n";

            foreach (var fact in workMemory.GetFactsList())
                result += "Имя факта: " + fact.Name + ", значение: " + fact.Value + "\r\n";

            MessageBox.Show(result);
        }

        // Обработка нажатия кнопки для перезапуска
        private void SystemResetBTN_Click(object sender, EventArgs e)
        {
            clicksCount = 0;
            askedQuestionCount = 0;
            currentFactName = string.Empty;
            workMemory.Clear();
            SystemOutputTB.Text = "Вас приветствует экспертная система по выбору ВУЗа. Отвечайте на вопросы согласно правилам системы, пока не получите рекомендацию системы.\r\n\r\n";
            UserAnswerBTN.Text = "Начать";
            UserAnswerTB.Enabled = false;
            UserAnswerBTN.Enabled = true;
            ExplanationComponentCallBtn.Enabled = false;
            UserAnswerTB.Clear();
        }
    }
}
