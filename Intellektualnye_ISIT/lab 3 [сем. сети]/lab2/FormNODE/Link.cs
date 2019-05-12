using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormNODE
{
    // Класс связи узлов
    public class Link
    {
        // Конструктор для инициализации
        public Link(Node childNode, Node parentNode, LinksTypes linkType)
        {
            ChildNode = childNode;
            ParentNode = parentNode;
            LinkType = linkType;
        }

        // Родительский узел
        public Node ParentNode
        {
            get;
            set;
        }

        // Дочерний узел
        public Node ChildNode
        {
            get;
            set;

        }

        // Тип связи
        public LinksTypes LinkType
        {
            get;
            set;
        }
    }
}
