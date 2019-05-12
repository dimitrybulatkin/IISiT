using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormNODE
{
    // Класс для типа вопроса
    public class Question
    {
        // Список типов связей для вопроса
        private List<LinksTypes> links; 

        // Конструктор класса с указанием названия 
        public Question(string text)
        {
            Text = text;
            links = new List<LinksTypes>();
        }

        // Добавление типа связи
        public void LinkAdd(LinksTypes link)
        {
            links.Add(link);
        }

        // Получение списка типов связей для данного вопроса
        public List<LinksTypes> GetLinks()
        {
            return links;
        }

        // Название вопроса
        public string Text
        {
            get;
            private set;
        }
    }
}
