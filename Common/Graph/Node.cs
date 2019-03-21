/*
https://msdn.microsoft.com/en-us/library/ms379572(v=vs.80).aspx

An Extensive Examination of Data Structures Using C# 2.0
Visual Studio 2005

Scott Mitchell
4GuysFromRolla.com
Update January 2005
*/

namespace CodingGame.Graph
{
    public class Node<T>
    {
        public Node() { }
        public Node(T data) : this(data, null) { }
        public Node(T data, NodeList<T> neighbors)
        {
            this.Value = data;
            this.Neighbors = neighbors;
        }

        public T Value { get; set; }

        protected NodeList<T> Neighbors { get; set; } = null;
    }
}
