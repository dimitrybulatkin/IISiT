using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Models_v1._0
{
    public partial class Form1 : Form
    {
        List<string> variants = new List<string> { "МГУ", "ПГУ", "ПНИПУ", "ТГУ"};// Варианты ответов
        List<User> users = new List<User>();// Список голосующих
        int currentUser;// Номер пользователя, отвечающий в данный момент
        List<int> votes = new List<int>(); // Список голосов
        List<User> preference = new List<User>(); // Список предпочтений пользователей

        // Конструктор класса
        public Form1()
        {
            InitializeComponent();

            // Временное отключение некоторых элементов на форме
            select.Enabled = false;
            clearAnswers.Enabled = false;
            vote.Enabled = false;
            textBoxProfile.ReadOnly = true;

            // Заполнение на форме списка с вариантами ответов
            foreach (string var in variants)
                variantsAnswers.Items.Add(var);
        }

        // Обновление номера голосующего пользователя на форме
        private void CurrentUser(int value)
        {
            labelCurUser.Text = (value + 1).ToString();
        }

        //Вывод результата по каждой из моделей
        private void ViewWinner()
        {
            // Для каждой модели создаются свои объекты
            RuleMajority majority = new RuleMajority(variants.Count, users);
            RuleCondorcet condorcet = new RuleCondorcet(variants.Count, users);
            RuleBorda borda = new RuleBorda(variants.Count, users);

            // Список голосов и предпочтений
            votes = condorcet.GetVotes;
            preference = condorcet.GetPreferences;

            // Вывод результатов по кажой модели
            MessageBox.Show("Победитель по Большенству: " + variants[majority.IndWinner()], majority.NameRule);

            int iWin = condorcet.IndWinner();
            if (iWin != -1)
                MessageBox.Show("Победитель по Кондорсе: " + variants[iWin], condorcet.NameRuleCondorcet);
            else
                MessageBox.Show("Парадокс", condorcet.NameRuleCondorcet);

            MessageBox.Show("Победитель по Компленду: " + variants[condorcet.Coplend()], condorcet.NameRuleCoplend);
            MessageBox.Show("Победитель по Симпсону: " + variants[condorcet.Simpson()], condorcet.NameRuleSimpson);
            MessageBox.Show("Победитель по Борде: " + variants[borda.IndWinner()], borda.NameRule);
        }

        // Сброс формы до первоначального состояния
        private void Clear()
        {
            select.Enabled = false;
            clearAnswers.Enabled = false;
            vote.Enabled = false;
            countUsers.Enabled = true;
            countUsers.Text = string.Empty;
            ok.Enabled = true;
            labelCurUser.Text = string.Empty;

            users.Clear();
        }

        //Обработка нажатия кнопки для добавление одного из вариантов
        private void select_Click(object sender, EventArgs e)
        {
            int iSelect = variantsAnswers.SelectedIndex;

            if (iSelect != -1 && !preferencesUsers.Items.Contains(variants[iSelect]))
                preferencesUsers.Items.Add(variants[iSelect]);
        }

        //Обработка нажатия кнопки для очистки списка предпочтений
        private void clear_Click(object sender, EventArgs e)
        {
            preferencesUsers.Items.Clear();
        }

        //Обработка нажатия кнопки для установки числа голосующих
        private void ok_Click(object sender, EventArgs e)
        {
            // Поле не должно быть пустым
            if (countUsers.Text != string.Empty)
            {
                int count = Convert.ToInt32(countUsers.Text);
                if (count > 0)
                {
                    // Очистка списков
                    votes.Clear();
                    preference.Clear();
                    textBoxProfile.Clear();

                    // Изменение состояний кнопок
                    select.Enabled = true;
                    clearAnswers.Enabled = true;
                    vote.Enabled = true;
                    countUsers.Enabled = false;
                    ok.Enabled = false;

                    // Заполнение списка голосующих пользователей
                    for (int i = 0; i < count; i++)
                        users.Add(new User());

                    currentUser = 0;
                    CurrentUser(currentUser);
                }
                else
                    MessageBox.Show("Количество избирателей должно быть больше 0.", "Внимание");
            }
            else
                MessageBox.Show("Поле не должно быть пустым", "Внимание");
        }

        //Обработка нажатия кнопки для голосования
        private void vote_Click(object sender, EventArgs e)
        {
            // Должен быть выбран хотя бы один пункт для голосования
            if (preferencesUsers.Items.Count != 0)
            {
                // Перебор предпочтений пользователей
                for (int i = 0; i < preferencesUsers.Items.Count; i++)
                {
                    string preference = preferencesUsers.Items[i].ToString();
                    users[currentUser].AddPreference(variants.FindIndex(x => x == preference) + 1);
                }
                // Очистка выбраных вариантов
                preferencesUsers.Items.Clear();

                currentUser++;

                // Если проголосовал последний пользователь, то объявляется результат
                if (currentUser >= users.Count)
                {
                    ViewWinner();
                    Clear();
                    FillProfile();
                }
                // ...иначе  переключение на следующего пользователя
                else
                    CurrentUser(currentUser);
            }
            else
                MessageBox.Show("Выберите один из вариантов", "Внимание");
        }

        // Заполнение плофайла на форме
        private void FillProfile()
        {
            for (int i = 0; i < votes.Count; i++)
            {
                string s = votes[i].ToString() + "   " + GetStringPreference(preference[i]);
                textBoxProfile.Text += s + "\r\n";
            }
        }

        // Функция для получения строки с предпочтениями пользователя
        private string GetStringPreference(User user)
        {
            string s = string.Empty;
            foreach (int p in user.GetPreferences)
                s += variants[p - 1] + " ";

            return s;
        }
    }
}