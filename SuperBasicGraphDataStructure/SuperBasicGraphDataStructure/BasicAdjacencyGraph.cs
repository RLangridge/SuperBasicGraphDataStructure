using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SuperBasicGraphDataStructure
{
    /// <summary>
    /// Basic implementation of a graph that uses nodes that have adjacency lists
    /// </summary>
    /// <typeparam name="TNodeDataType">Object data type stored in a node</typeparam>
    public class BasicAdjacencyGraph<TNodeDataType, TCost> : IGraph<TNodeDataType, TCost> where TCost : IComparable
    {
        private Dictionary<GraphNode<TNodeDataType>, LinkedList<(GraphNode<TNodeDataType>, TCost)>> _adjacencyList = 
            new Dictionary<GraphNode<TNodeDataType>, LinkedList<(GraphNode<TNodeDataType>, TCost)>>();

        public BasicAdjacencyGraph()
        {
            
        }

        /// <summary>
        /// Adds in a node without any connections
        /// </summary>
        /// <param name="node">The new node to be added</param>
        /// <exception cref="ArgumentException">Thrown if the node already exists in the graph</exception>
        public void AddNode(GraphNode<TNodeDataType> node)
        {
            if(node == null)
                throw new ArgumentNullException(nameof(node), "A null node can't be added to this graph.");
            if(_adjacencyList.ContainsKey(node))
                throw new ArgumentException("The given node already exists in the graph.", nameof(node));
            _adjacencyList.Add(node, new LinkedList<(GraphNode<TNodeDataType>, TCost)>());
        }

        /// <summary>
        /// Add a two-directional connection between two nodes
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="srcToDstCost"></param>
        /// <param name="dstToSrcCost"></param>
        public void AddEdge(GraphNode<TNodeDataType> src, GraphNode<TNodeDataType> dst, TCost srcToDstCost,
            TCost dstToSrcCost)
        {
            AddOneDirectionalEdge(src, dst, srcToDstCost);
            AddOneDirectionalEdge(dst, src, dstToSrcCost);
        }

        /// <summary>
        /// Add a connection from one node to another. This is not bi-directional
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="cost"></param>
        /// <exception cref="ArgumentNullException">Thrown if either node is null</exception>
        public void AddOneDirectionalEdge(GraphNode<TNodeDataType> src, GraphNode<TNodeDataType> dst, TCost cost)
        {
            if(dst == null)
                throw new ArgumentNullException(nameof(dst), "Node being connected to can't be null");
            if(src == null)
                throw new ArgumentNullException(nameof(src), "Node being connected from can't be null");

            if(!_adjacencyList.ContainsKey(src))
                _adjacencyList.Add(src, new LinkedList<(GraphNode<TNodeDataType>, TCost)>());
            _adjacencyList[src].AddLast((dst, cost));
        }

        /// <summary>
        /// Retrieves a list of nodes that are adjacent to the given root node
        /// (i.e. there's at least a 1-way connection from the root node to the nodes being returned)
        /// </summary>
        /// <param name="root">The node we're getting adjacent nodes for</param>
        /// <returns>A list of nodes that are adjacent to the root node</returns>
        /// <exception cref="ArgumentNullException">Thrown if root is null</exception>
        public LinkedList<(GraphNode<TNodeDataType>, TCost)> GetAdjacentNodes(GraphNode<TNodeDataType> root)
        {
            if(root == null)
                throw new ArgumentNullException(nameof(root), "Root for adjacency list can't be null");
            // If the key doesn't exist, this means that the root given doesn't have any links that it can go to.
            // This doesn't mean however that other nodes can't traverse to it
            return !_adjacencyList.ContainsKey(root) ? new LinkedList<(GraphNode<TNodeDataType>, TCost)>() : _adjacencyList[root];
        }

        /// <summary>
        /// Perform a breadth-first traversal on the graph given a node
        /// </summary>
        /// <param name="root">The node we're starting the search on</param>
        /// <param name="actionOnData">The action we're going to perform on the data in the graph</param>
        public void BreadthFirstTraversal(GraphNode<TNodeDataType> root, Action<TNodeDataType> actionOnData)
        {
            // If this node doesn't connect to any adjacent nodes, we can terminate early
            if (GetAdjacentNodes(root).Count == 0)
                return;
            
            var visited = new LinkedList<GraphNode<TNodeDataType>>();
            var queue = new LinkedList<GraphNode<TNodeDataType>>();

            queue.AddLast(root);

            while (queue.Any())
            {
                var first = queue.First();
                visited.AddLast(first); // So we don't revisit this node in the future
                actionOnData?.Invoke(first.Data);
                queue.RemoveFirst();

                // For all nodes that haven't been visited, queue them up
                GetAdjacentNodes(first).ToList().FindAll(x => 
                    !visited.Contains(x.Item1)).ForEach(x => queue.AddLast(x.Item1));
            }
        }

        /// <summary>
        /// Runs a depth first traversal on the graph given a node. Will terminate if root node given doesn't have neighbours
        /// </summary>
        /// <param name="root">The node we're starting our traversal from</param>
        /// <param name="actionOnData">The action we want to run on the data in the graph</param>
        public void DepthFirstTraversal(GraphNode<TNodeDataType> root, Action<TNodeDataType> actionOnData)
        {
            // If this node doesn't connect to any adjacent nodes, we can terminate early
            if (GetAdjacentNodes(root).Count == 0)
                return;
            
            DepthFirstTraversalHelper(root, new List<GraphNode<TNodeDataType>>(), actionOnData);
        }

        public int GetNumberOfNodesInGraph()
        {
            return _adjacencyList.Count;
        }

        public TCost MinimumCostBetweenTwoNodes(GraphNode<TNodeDataType> src, GraphNode<TNodeDataType> dst)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src), "Source node is null");
            if(dst == null) 
                throw new ArgumentNullException(nameof(dst), "Destination node is null");
            
            if(GetAdjacentNodes(src).Count == 0)
                throw new ConstraintException($"{nameof(src)} doesn't have any neighbours. Can't calculate a distance without adjacent nodes.");
            if(GetAdjacentNodes(dst).Count == 0)
                throw new ConstraintException($"{nameof(dst)} doesn't have any neighbours. Can't calculate a distance without adjacent nodes.");
            
            if (src == dst) // Nodes are the same; can't really go anywhere
                return default(TCost);

            return default(TCost);
        }

        public ICollection<GraphNode<TNodeDataType>> GetPathBetweenTwoNodes(GraphNode<TNodeDataType> src, GraphNode<TNodeDataType> dst)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helper function that runs recursively to iterate through the graph
        /// </summary>
        /// <param name="root">The currently visited node</param>
        /// <param name="visitedNodes">The nodes that have been visited</param>
        /// <param name="actionOnData">The action to run on the current node</param>
        private void DepthFirstTraversalHelper(GraphNode<TNodeDataType> root, List<GraphNode<TNodeDataType>> visitedNodes,
            Action<TNodeDataType> actionOnData)
        {
            // Add this node to the visited node list and run the action
            visitedNodes.Add(root);
            actionOnData?.Invoke(root.Data);
            
            // For each neighbour node, run this function again if the node isn't in the visited node list
            GetAdjacentNodes(root).ToList().FindAll( x => !visitedNodes.Contains(x.Item1)).
                ForEach(x => DepthFirstTraversalHelper(x.Item1, visitedNodes, actionOnData));
        }

        public void PrintGraph()
        {
            
        }
    }
}