using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

//For puzzle: https://www.codingame.com/ide/puzzle/skynet-revolution-episode-2
namespace SkynetRevolutionEpisode2
{

    /**
     * Auto-generated code below aims at helping you parse
     * the standard input according to the problem statement.
     **/
    class SkynetVirus
    {
        static void Main(string[] args)
        {
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
            int L = int.Parse(inputs[1]); // the number of links
            int E = int.Parse(inputs[2]); // the number of exit gateways

            Graph<int> g = new Graph<int>();
            List<GraphNode<int>> gatewayNodes = new List<GraphNode<int>>();
            List<NodeProp> nodeProps = new List<NodeProp>(N);

            for (int i = 0; i < L; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int N1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
                int N2 = int.Parse(inputs[1]);

                if (!g.Contains(N1))
                {
                    g.AddNode(N1);
                }
                if (!g.Contains(N2))
                {
                    g.AddNode(N2);
                }

                g.AddUndirectedEdge((GraphNode<int>)g.Nodes.FindByValue(N1), (GraphNode<int>)g.Nodes.FindByValue(N2), 1);
            }

            //Create node properties
            foreach (var node in g.Nodes)
            {
                nodeProps.Add(new NodeProp(node.Value));
            }

            //Figure out gateways
            for (int i = 0; i < E; i++)
            {
                int EI = int.Parse(Console.ReadLine()); // the index of a gateway node
                gatewayNodes.Add((GraphNode<int>)g.Nodes.FindByValue(EI));
                nodeProps.Find(np => np.Id == EI).IsGate = true;
            }



            // game loop
            while (true)
            {
                int SI = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn

                GraphNode<int> agent = (GraphNode<int>)g.Nodes.FindByValue(SI);
                GraphNode<int> node1 = null;
                GraphNode<int> node2 = null;

                //If the agent is near, cut his link
                foreach (var agentNeighbor in agent.Neighbors)
                {
                    if (gatewayNodes.Contains(agentNeighbor))
                    {
                        node1 = (GraphNode<int>)agentNeighbor;
                        node2 = agent;
                        Console.Error.WriteLine("Agent is next to gateway : " + agentNeighbor.Value);
                        break;
                    }
                }

                if (node1 == null)
                {
                    //BFS to get distances from agent.
                    GraphNode<int> root = agent;
                    Dictionary<int, int> distances = new Dictionary<int, int>();

                    foreach (GraphNode<int> node in g.Nodes)
                    {
                        distances[node.Value] = int.MaxValue;
                    }

                    Queue<int> queue = new Queue<int>();

                    distances[root.Value] = 0;
                    queue.Enqueue(root.Value);

                    while (queue.Count > 0)
                    {
                        int currentNodeId = queue.Dequeue();
                        GraphNode<int> current = (GraphNode<int>)g.Nodes.FindByValue(currentNodeId);
                        foreach (GraphNode<int> node in current.Neighbors)
                        {
                            if (distances[node.Value] == int.MaxValue)
                            {
                                distances[node.Value] = distances[currentNodeId] + 1;
                                queue.Enqueue(node.Value);
                            }
                        }
                    }

                    //Calculate node properties
                    foreach (GraphNode<int> node in g.Nodes)
                    {
                        NodeProp np = nodeProps.Find(n => n.Id == node.Value);
                        np.IsGate = gatewayNodes.Contains(node);
                        np.IsNextToGate = gatewayNodes.Any(gateWay => node.Neighbors.Any(neighbor => neighbor.Value == gateWay.Value));
                        np.DistanceFromAgent = distances[np.Id];
                        np.IsAccessible = node.Neighbors.Count > 0;
                        np.NeighborCount = node.Neighbors.Count;
                        np.GatewaysConnected = gatewayNodes.Count(gateWay => gateWay.Neighbors.Any(neighbor => neighbor.Value == node.Value));
                    }

                    //Closest node that is connected to gateways (tie breaker being how many its connected to)
                    int nodeWithManyGates = nodeProps.Where(n => n.IsNextToGate).OrderBy(m => m.DistanceFromAgent).ThenByDescending(c => c.GatewaysConnected).First().Id;
                    node1 = (GraphNode<int>)g.Nodes.FindByValue(nodeWithManyGates);

                    //Grab a random gateway its attached to
                    node2 = (GraphNode<int>)node1.Neighbors.First(n => gatewayNodes.Contains((GraphNode<int>)n));


                    //Below seems to work correctly, but isn't always succesfull, need to look ahead where the agent will go

                    ////Find the gateway with the most connections.
                    //int hotGateway = nodeProps.Where(np => np.IsGate).OrderByDescending(gate => gate.NeighborCount).ThenBy(d => d.DistanceFromAgent).First().Id;

                    ////Find the node which is closest
                    //int hotGatewayCloseNode = gatewayNodes.Find(gate => gate.Value == hotGateway)
                    //                                        .Neighbors
                    //                                        .OrderBy(n => nodeProps.Where(np => np.Id == n.Value).First().DistanceFromAgent)
                    //                                        .First().Value;

                    //node1 = (GraphNode<int>)g.Nodes.FindByValue(hotGatewayCloseNode);
                    //node2 = (GraphNode<int>)g.Nodes.FindByValue(hotGateway);
                }

                //Output and sever the link between the 2 nodes in our graph
                Console.WriteLine(node1.Value + " " + node2.Value);
                node1.Neighbors.Remove(node2);
                node2.Neighbors.Remove(node1);
            }
        }
    }

    public class NodeProp
    {
        public readonly int Id;
        public bool IsGate;
        public bool IsNextToGate;
        public int DistanceFromAgent;
        public bool IsAccessible;
        public int NeighborCount;
        public int GatewaysConnected;

        public NodeProp(int id)
        {
            Id = id;
        }
    }


    /**************************************************************************************************************/




    /*
    https://msdn.microsoft.com/en-us/library/ms379572(v=vs.80).aspx

    An Extensive Examination of Data Structures Using C# 2.0
    Visual Studio 2005

    Scott Mitchell
    4GuysFromRolla.com
    Update January 2005
    */


    public class Node<T>
    {
        // Private member-variables
        private T data;
        private NodeList<T> neighbors = null;

        public Node() { }
        public Node(T data) : this(data, null) { }
        public Node(T data, NodeList<T> neighbors)
        {
            this.data = data;
            this.neighbors = neighbors;
        }

        public T Value
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        protected NodeList<T> Neighbors
        {
            get
            {
                return neighbors;
            }
            set
            {
                neighbors = value;
            }
        }
    }

    /*
    https://msdn.microsoft.com/en-us/library/ms379572(v=vs.80).aspx
    An Extensive Examination of Data Structures Using C# 2.0
    Visual Studio 2005

    Scott Mitchell
    4GuysFromRolla.com
    Update January 2005
    */
    public class NodeList<T> : Collection<Node<T>>
    {
        public NodeList() : base() { }

        public NodeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(Node<T>));
        }

        public Node<T> FindByValue(T value)
        {
            // search the list for the value
            foreach (Node<T> node in Items)
                if (node.Value.Equals(value))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }
    }


    /*
    https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx
    An Extensive Examination of Data Structures Using C# 2.0
    Visual Studio 2005

    Scott Mitchell
    4GuysFromRolla.com
    Update January 2005
    */

    public class GraphNode<T> : Node<T>
    {
        private List<int> costs;

        public GraphNode() : base() { }
        public GraphNode(T value) : base(value) { }
        public GraphNode(T value, NodeList<T> neighbors) : base(value, neighbors) { }

        new public NodeList<T> Neighbors
        {
            get
            {
                if (base.Neighbors == null)
                    base.Neighbors = new NodeList<T>();

                return base.Neighbors;
            }
        }

        public List<int> Costs
        {
            get
            {
                if (costs == null)
                    costs = new List<int>();

                return costs;
            }
        }
    }

    /*
    https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx
    An Extensive Examination of Data Structures Using C# 2.0
    Visual Studio 2005

    Scott Mitchell
    4GuysFromRolla.com
    Update January 2005
    */

    public class Graph<T> : IEnumerable<T>
    {
        private NodeList<T> nodeSet;

        public Graph() : this(null) { }
        public Graph(NodeList<T> nodeSet)
        {
            if (nodeSet == null)
                this.nodeSet = new NodeList<T>();
            else
                this.nodeSet = nodeSet;
        }

        public void AddNode(GraphNode<T> node)
        {
            // adds a node to the graph
            nodeSet.Add(node);
        }

        public void AddNode(T value)
        {
            // adds a node to the graph
            nodeSet.Add(new GraphNode<T>(value));
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
            return nodeSet.FindByValue(value) != null;
        }

        public bool Remove(T value)
        {
            // first remove the node from the nodeset
            GraphNode<T> nodeToRemove = (GraphNode<T>)nodeSet.FindByValue(value);
            if (nodeToRemove == null)
                // node wasn't found
                return false;

            // otherwise, the node was found
            nodeSet.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (GraphNode<T> gnode in nodeSet)
            {
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node and associated cost
                    gnode.Neighbors.RemoveAt(index);
                    gnode.Costs.RemoveAt(index);
                }
            }

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)nodeSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return nodeSet.GetEnumerator();
        }

        public NodeList<T> Nodes
        {
            get
            {
                return nodeSet;
            }
        }

        public int Count
        {
            get { return nodeSet.Count; }
        }


    }
}
