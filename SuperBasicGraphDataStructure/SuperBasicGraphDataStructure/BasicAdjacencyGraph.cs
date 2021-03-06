﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SuperBasicGraphDataStructure
{
    /// <summary>
    /// Basic implementation of a graph that uses nodes that have adjacency lists
    /// </summary>
    /// <typeparam name="TNodeDataType">Object data type stored in a node</typeparam>
    /// <typeparam name="TCost">The type that the cost between nodes is in the graph (Needs to inherit from IComparable)</typeparam>
    public class BasicAdjacencyGraph<TNodeDataType> : IGraph<TNodeDataType, int>
    {
        private Dictionary<GraphNode<TNodeDataType>, LinkedList<(GraphNode<TNodeDataType>, int)>> _adjacencyList = 
            new Dictionary<GraphNode<TNodeDataType>, LinkedList<(GraphNode<TNodeDataType>, int)>>();

        private Dictionary<GraphNode<TNodeDataType>, (GraphNode<TNodeDataType>, int)> _shortestPathCache =
            new Dictionary<GraphNode<TNodeDataType>, (GraphNode<TNodeDataType>, int)>();
        private GraphNode<TNodeDataType> _shortestPathCacheNode = null; // Used to mark that the shortest path cache needs to be recalculated

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
            _adjacencyList.Add(node, new LinkedList<(GraphNode<TNodeDataType>, int)>());
        }

        /// <summary>
        /// Add a two-directional connection between two nodes
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="srcToDstCost"></param>
        /// <param name="dstToSrcCost"></param>
        public void AddEdge(GraphNode<TNodeDataType> src, GraphNode<TNodeDataType> dst, int srcToDstCost,
            int dstToSrcCost)
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
        public void AddOneDirectionalEdge(GraphNode<TNodeDataType> src, GraphNode<TNodeDataType> dst, int cost)
        {
            if(dst == null)
                throw new ArgumentNullException(nameof(dst), "Node being connected to can't be null");
            if(src == null)
                throw new ArgumentNullException(nameof(src), "Node being connected from can't be null");

            if(!_adjacencyList.ContainsKey(src))
                _adjacencyList.Add(src, new LinkedList<(GraphNode<TNodeDataType>, int)>());
            _adjacencyList[src].AddLast((dst, cost));
        }

        /// <summary>
        /// Retrieves a list of nodes that are adjacent to the given root node
        /// (i.e. there's at least a 1-way connection from the root node to the nodes being returned)
        /// </summary>
        /// <param name="root">The node we're getting adjacent nodes for</param>
        /// <returns>A list of nodes that are adjacent to the root node</returns>
        /// <exception cref="ArgumentNullException">Thrown if root is null</exception>
        public LinkedList<(GraphNode<TNodeDataType>, int)> GetAdjacentNodes(GraphNode<TNodeDataType> root)
        {
            if(root == null)
                throw new ArgumentNullException(nameof(root), "Root for adjacency list can't be null");
            // If the key doesn't exist, this means that the root given doesn't have any links that it can go to.
            // This doesn't mean however that other nodes can't traverse to it
            return !_adjacencyList.ContainsKey(root) ? new LinkedList<(GraphNode<TNodeDataType>, int)>() : _adjacencyList[root];
        }

        /// <summary>
        /// Perform a breadth-first traversal on the graph given a node
        /// </summary>
        /// <param name="root">The node we're starting the search on</param>
        /// <param name="actionOnData">The action we're going to perform on the data in the graph</param>
        public void BreadthFirstTraversal(GraphNode<TNodeDataType> root, Action<GraphNode<TNodeDataType>> actionOnData)
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
                actionOnData?.Invoke(first);
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
        public void DepthFirstTraversal(GraphNode<TNodeDataType> root, Action<GraphNode<TNodeDataType>> actionOnData)
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

        public ICollection<GraphNode<TNodeDataType>> GetAllGraphNodes()
        {
            return _adjacencyList.Keys.ToList();
        }

        public GraphNode<TNodeDataType> FindNode(Func<TNodeDataType, bool> comparisonFunction)
        {
            return _adjacencyList.Keys.ToList().Find(x => comparisonFunction(x.Data));
        }

        /// <summary>
        /// Retrieve the minimum cost between two nodes in a graph
        /// </summary>
        /// <param name="src">The starting node that we're building the search around</param>
        /// <param name="dst">The destination node that we want to reach</param>
        /// <returns>The smallest distance between the source and destination node</returns>
        /// <exception cref="ArgumentNullException">Thrown if either source or destination are null</exception>
        /// <exception cref="ConstraintException">Thrown if either source or destination don't have any neighbours. Can also be
        /// thrown when the destination node isn't connected to the source node in the graph in some way</exception>
        public int MinimumCostBetweenTwoNodes(GraphNode<TNodeDataType> src, GraphNode<TNodeDataType> dst)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src), "Source node is null");
            if(dst == null) 
                throw new ArgumentNullException(nameof(dst), "Destination node is null");

            if (src == dst) // Nodes are the same; can't really go anywhere
                return 0;

            if (_shortestPathCacheNode != src) // We need to recalculate our graphs paths; not a process we want to repeat often
                CalculateGraphDistanceCache(src, dst);

            return _shortestPathCache[dst].Item2;
        }

        /// <summary>
        /// Calculates the distance between the source node and the destination ndde
        /// </summary>
        /// <param name="src">The node we're starting from</param>
        /// <param name="dst">The node we're going towards</param>
        /// <exception cref="ConstraintException">Thrown if the source or destination don't have neighbour nodes</exception>
        private void CalculateGraphDistanceCache(GraphNode<TNodeDataType> src, GraphNode<TNodeDataType> dst)
        {
            if (GetAdjacentNodes(src).Count == 0)
                throw new ConstraintException(
                    $"{nameof(src)} doesn't have any neighbours. Can't calculate a distance without adjacent nodes.");
            if (GetAdjacentNodes(dst).Count == 0)
                throw new ConstraintException(
                    $"{nameof(dst)} doesn't have any neighbours. Can't calculate a distance without adjacent nodes.");
            
            // First, we can do a breadth-first search to see if the nodes are actually connected
            bool found = false;
            BreadthFirstTraversal(src, node =>
            {
                if (node == dst)
                    found = true;
            });
            
            if(!found) // If we can't find a path, the nodes aren't connected
                throw new ConstraintException("The source node and the destination node given to calculate distance" +
                                              "in the graph are not connected.");

            var visitedNodes = new List<GraphNode<TNodeDataType>>();
            var unvisitedNodes = _adjacencyList.Keys.ToList().Select(x => (x, x == src ? 0 : int.MaxValue)).ToList();

            // Push the nodes into the shortest path cache
            _shortestPathCache.Clear();
            unvisitedNodes.ForEach(x => _shortestPathCache.Add(x.x, (null, x.Item2)));

            var unvisitedNodesPriority = new PriorityQueue<(GraphNode<TNodeDataType>, int)>();
            // Add our nodes into a priority queue, based off of how close they are to our source node
            unvisitedNodesPriority.AddRange(unvisitedNodes, CompareGraphNodeDistances);

            while (unvisitedNodesPriority.Count > 0)
            {
                var currentNode = unvisitedNodesPriority.PopFirst();

                // Get neighbours that haven't already been visited
                var viableNeighbours = GetAdjacentNodes(currentNode.Item1).ToList()
                    .Where(x => !visitedNodes.Contains(x.Item1)).ToList();

                // If we don't have any viable neighbours, ignore this run
                if (viableNeighbours.Count == 0)
                    continue;

                // Add the current node to the nodes we've visited
                visitedNodes.Add(currentNode.Item1);

                // For each viable neighbour, we need to work out the cost to our source node and sort our priority 
                // queue based on that
                foreach (var (graphNode, cost) in viableNeighbours)
                {
                    var currentCost = currentNode.Item2 + cost;
                    if (_shortestPathCache[graphNode].Item2 > currentCost)
                        _shortestPathCache[graphNode] = (currentNode.Item1, currentCost);

                    unvisitedNodesPriority.FindAndReplace((graphNode, cost), (graphNode, currentCost),
                        (tuple, valueTuple) => tuple.Item1 == valueTuple.Item1 ? 0 : -1,
                        CompareGraphNodeDistances);
                }
            }

            _shortestPathCacheNode = src;
        }

        private int CompareGraphNodeDistances((GraphNode<TNodeDataType>, int) nodeA,
            (GraphNode<TNodeDataType>, int) nodeB)
        {
            if (nodeA.Item2 < nodeB.Item2)
                return -1;
            if (nodeA.Item2 > nodeB.Item2)
                return 1;
            return 0;
        }

        /// <summary>
        /// Get the node path between two nodes (Order is dst -> src)
        /// </summary>
        /// <param name="src">The node we're coming from and will build the cache around</param>
        /// <param name="dst">The node we want to reach</param>
        /// <returns>A path from destination node to source node</returns>
        /// <exception cref="ArgumentNullException">Thrown if source or destination are null</exception>
        public ICollection<GraphNode<TNodeDataType>> GetPathBetweenTwoNodes(GraphNode<TNodeDataType> src, GraphNode<TNodeDataType> dst)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src), "Source node is null");
            if(dst == null) 
                throw new ArgumentNullException(nameof(dst), "Destination node is null");
            if(src == dst) // They're the same, just throw back an empty list
                return new List<GraphNode<TNodeDataType>>();

            if (_shortestPathCacheNode != src) // If we haven't built our cache around the source node, build it
                CalculateGraphDistanceCache(src, dst);

            var currentNode = dst;
            var nodePath = new List<GraphNode<TNodeDataType>>();
            while(_shortestPathCache[currentNode].Item1 != null) // Go through the cache until we've hit our source node
            {
                nodePath.Add(currentNode); // And add each node on the way to the path
                currentNode = _shortestPathCache[currentNode].Item1;
            }
            nodePath.Add(src);

            return nodePath;
        }

        /// <summary>
        /// Helper function that runs recursively to iterate through the graph
        /// </summary>
        /// <param name="root">The currently visited node</param>
        /// <param name="visitedNodes">The nodes that have been visited</param>
        /// <param name="actionOnData">The action to run on the current node</param>
        private void DepthFirstTraversalHelper(GraphNode<TNodeDataType> root,
            List<GraphNode<TNodeDataType>> visitedNodes,
            Action<GraphNode<TNodeDataType>> actionOnData)
        {
            // Add this node to the visited node list and run the action
            visitedNodes.Add(root);
            actionOnData?.Invoke(root);
            // For each neighbour node, run this function again if the node isn't in the visited node list
            GetAdjacentNodes(root).ToList().FindAll( x => !visitedNodes.Contains(x.Item1)).
                ForEach(x => DepthFirstTraversalHelper(x.Item1, visitedNodes, actionOnData));
        }

        public void PrintGraph()
        {
            
        }
    }
}