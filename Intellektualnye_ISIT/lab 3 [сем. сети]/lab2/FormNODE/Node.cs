using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormNODE
{
    // Класс узла
    public class Node
    {
        // Списко связей с другими узлами
        private List<Link> links;

        // Конструктор класса 
        public Node(string nodeName)
        {
            Name = nodeName;
            links = new List<Link>();
        }

        // Добавление в список новой связи к одному из узлов
        public void LinkAdd(Link link)
        {
            links.Add(link);
        }

        // Получение списка всех связей
        public List<Link> GetLinks()
        {
            return links;
        }

        // Имя узла
        public string Name
        {
            get;
            private set;
        }
    }
}
