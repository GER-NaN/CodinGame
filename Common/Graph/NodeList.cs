/*
https://msdn.microsoft.com/en-us/library/ms379572(v=vs.80).aspx
An Extensive Examination of Data Structures Using C# 2.0
Visual Studio 2005

Scott Mitchell
4GuysFromRolla.com
Update January 2005
*/

using System.Collections.ObjectModel;

namespace CodingGame.Graph
{
    public class NodeList<T> : Collection<Node<T>>
    {
        public NodeList() : base() { }

        public NodeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
            {
                base.Items.Add(default(Node<T>));
            }
        }

        public Node<T> FindByValue(T value)
        {
            // search the list for the value
            foreach (Node<T> node in Items)
            {
                if (node.Value.Equals(value))
                {
                    return node;
                } 
            }

            // if we reached here, we didn't find a matching node
            return null;
        }
    }
}
