/*
https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx
An Extensive Examination of Data Structures Using C# 2.0
Visual Studio 2005

Scott Mitchell
4GuysFromRolla.com
Update January 2005
*/

using System.Collections.Generic;
using System.Linq;

namespace Common.Graph
{
    public class Graph<T>
    {
        private readonly NodeList<T> _nodeSet;

        public Graph() : this(null) { }
        public Graph(NodeList<T> initialNodes)
        {
            if (initialNodes == null)
            {
                this._nodeSet = new NodeList<T>();
            }
            else
            {
                this._nodeSet = initialNodes;
            }
        }

        public void AddNode(GraphNode<T> node)
        {
            // adds a node to the graph
            _nodeSet.Add(node);
        }

        public void AddNode(T value)
        {
            // adds a node to the graph
            _nodeSet.Add(new GraphNode<T>(value));
        }

        public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);
        }

        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);

            to.Neighbors.Add(from);
            to.Costs.Add(cost);
        }

        public bool Contains(T value)
        {
            return _nodeSet.FindByValue(value) != null;
        }

        public bool Remove(T value)
        {
            // first remove the node from the nodeset
            GraphNode<T> nodeToRemove = (GraphNode<T>)_nodeSet.FindByValue(value);
            if (nodeToRemove == null)
            {
                return false;
            }

            _nodeSet.Remove(nodeToRemove);

            // removing edges to this node
            foreach (var node in _nodeSet)
            {
                var gnode = (GraphNode<T>) node;
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    gnode.Neighbors.RemoveAt(index);
                    gnode.Costs.RemoveAt(index);
                }
            }

            return true;
        }

        public NodeList<T> Nodes
        {
            get
            {
                return _nodeSet;
            }
        }

        public int Count
        {
            get { return _nodeSet.Count; }
        }
    }
}
