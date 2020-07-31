using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperBasicGraphDataStructure
{
    /// <summary>
    /// Basic implementation of a graph that uses nodes that have adjacency lists
    /// </summary>
    /// <typeparam name="NodeDataType">Object data type stored in a node</typeparam>
    public class Graph<NodeDataType>
    {
        private Dictionary<GraphNode<NodeDataType>, LinkedList<GraphNode<NodeDataType>>> _adjacencyList = new Dictionary<GraphNode<NodeDataType>, LinkedList<GraphNode<NodeDataType>>>();

        public Graph()
        {
            
        }
        
        /// <summary>
        /// Add a two-directional connection between two nodes
        /// </summary>
        /// <param name="first">First node to connect up</param>
        /// <param name="second">Second node to connect up</param>
        public void AddEdge(GraphNode<NodeDataType> first, GraphNode<NodeDataType> second)
        {
            AddOneDirectionalEdge(first, second);
            AddOneDirectionalEdge(second, first);
        }

        /// <summary>
        /// Add a connection from one node to another. This is not bi-directional
        /// </summary>
        /// <param name="nodeBeingConnectedFrom">The node that we're connecting from</param>
        /// <param name="nodeBeingConnectedTo">The node we're connecting to</param>
        /// <exception cref="ArgumentNullException">Thrown if either node is null</exception>
        public void AddOneDirectionalEdge(GraphNode<NodeDataType> nodeBeingConnectedFrom,
            GraphNode<NodeDataType> nodeBeingConnectedTo)
        {
            if(nodeBeingConnectedTo == null)
                throw new ArgumentNullException(nameof(nodeBeingConnectedTo), "Node being connected to can't be null");
            if(nodeBeingConnectedFrom == null)
                throw new ArgumentNullException(nameof(nodeBeingConnectedFrom), "Node being connected from can't be null");
            
            if(!_adjacencyList.ContainsKey(nodeBeingConnectedFrom))
                _adjacencyList.Add(nodeBeingConnectedFrom, new LinkedList<GraphNode<NodeDataType>>());
            _adjacencyList[nodeBeingConnectedFrom].AddLast(nodeBeingConnectedTo);
        }

        /// <summary>
        /// Retrieves a list of nodes that are adjacent to the given root node
        /// (i.e. there's at least a 1-way connection from the root node to the nodes being returned)
        /// </summary>
        /// <param name="root">The node we're getting adjacent nodes for</param>
        /// <returns>A list of nodes that are adjacent to the root node</returns>
        /// <exception cref="ArgumentNullException">Thrown if root is null</exception>
        public LinkedList<GraphNode<NodeDataType>> GetAdjacentNodes(GraphNode<NodeDataType> root)
        {
            if(root == null)
                throw new ArgumentNullException(nameof(root), "Root for adjacency list can't be null");
            // If the key doesn't exist, this means that the root given doesn't have any links that it can go to.
            // This doesn't mean however that other nodes can't traverse to it
            return !_adjacencyList.ContainsKey(root) ? new LinkedList<GraphNode<NodeDataType>>() : _adjacencyList[root];
        }

        /// <summary>
        /// Perform a breadth-first traversal on the graph given a node
        /// </summary>
        /// <param name="root">The node we're starting the search on</param>
        /// <param name="actionOnData">The action we're going to perform on the data in the graph</param>
        public void BreadthFirstTraversal(GraphNode<NodeDataType> root, Action<NodeDataType> actionOnData)
        {
            // If this node doesn't connect to any adjacent nodes, we can terminate early
            if (GetAdjacentNodes(root).Count == 0)
                return;
            
            var visited = new LinkedList<GraphNode<NodeDataType>>();
            var queue = new LinkedList<GraphNode<NodeDataType>>();

            queue.AddLast(root);

            while (queue.Any())
            {
                var first = queue.First();
                visited.AddLast(first); // So we don't revisit this node in the future
                actionOnData?.Invoke(first.Data);
                queue.RemoveFirst();

                // For all nodes that haven't been visited, queue them up
                GetAdjacentNodes(first).ToList().FindAll(x => !visited.Contains(x)).ForEach(x => queue.AddLast(x));
            }
        }

        /// <summary>
        /// Runs a depth first traversal on the graph given a node. Will terminate if root node given doesn't have neighbours
        /// </summary>
        /// <param name="root">The node we're starting our traversal from</param>
        /// <param name="actionOnData">The action we want to run on the data in the graph</param>
        public void DepthFirstTraversal(GraphNode<NodeDataType> root, Action<NodeDataType> actionOnData)
        {
            // If this node doesn't connect to any adjacent nodes, we can terminate early
            if (GetAdjacentNodes(root).Count == 0)
                return;
            
            DepthFirstTraversalHelper(root, new List<GraphNode<NodeDataType>>(), actionOnData);
        }

        /// <summary>
        /// Helper function that runs recursively to iterate through the graph
        /// </summary>
        /// <param name="root">The currently visited node</param>
        /// <param name="visitedNodes">The nodes that have been visited</param>
        /// <param name="actionOnData">The action to run on the current node</param>
        private void DepthFirstTraversalHelper(GraphNode<NodeDataType> root, List<GraphNode<NodeDataType>> visitedNodes,
            Action<NodeDataType> actionOnData)
        {
            // Add this node to the visited node list and run the action
            visitedNodes.Add(root);
            actionOnData?.Invoke(root.Data);
            
            // For each neighbour node, run this function again if the node isn't in the visited node list
            GetAdjacentNodes(root).ToList().FindAll( x => !visitedNodes.Contains(x)).
                ForEach(x => DepthFirstTraversalHelper(x, visitedNodes, actionOnData));
        }

        public void PrintGraph()
        {
            
        }
    }
}