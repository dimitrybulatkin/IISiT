using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormNODE
{
    public class Link
    {
        public Link(Node childNode, Node parentNode, LinksTypes linkType)
        {
            ChildNode = childNode;
            ParentNode = parentNode;
            LinkType = linkType;
        }

        public Node ParentNode
        {
            get;
            set;
        }

        public Node ChildNode
        {
            get;
            set;

        }

        public LinksTypes LinkType
        {
            get;
            set;
        }
    }
}
