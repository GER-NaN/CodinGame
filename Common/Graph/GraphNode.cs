/*
https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx
An Extensive Examination of Data Structures Using C# 2.0
Visual Studio 2005

Scott Mitchell
4GuysFromRolla.com
Update January 2005
*/

using System.Collections.Generic;

namespace CodingGame.Graph
{
    /*
     * Neighbors and Costs are used as parallel lists where the index of an item relates it to the counterpart.
     * Example: Neighor at 'Neighbors[4]' has a Cost of 'Costs[4]'.
     */
    public class GraphNode<T> : Node<T>
    {
        private List<int> _costs;

        public GraphNode() : base() { }
        public GraphNode(T value) : base(value) { }
        public GraphNode(T value, NodeList<T> neighbors) : base(value, neighbors) { }

        new public NodeList<T> Neighbors
        {
            get
            {
                if (base.Neighbors == null)
                {
                    base.Neighbors = new NodeList<T>();
                }

                return base.Neighbors;
            }
        }

        public List<int> Costs
        {
            get
            {
                if (_costs == null)
                {
                    _costs = new List<int>();
                }

                return _costs;
            }
        }
    }
}
