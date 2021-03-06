﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace SuperBasicGraphDataStructure
{
    public interface IGraph<TNodeDataType, TCost> where TCost : IComparable
    {
        /// <summary>
        /// Add a two-directional connection between two nodes
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="srcToDstCost"></param>
        /// <param name="dstToSrcCost"></param>
        void AddEdge(GraphNode<TNodeDataType> src, GraphNode<TNodeDataType> dst, TCost srcToDstCost, TCost dstToSrcCost);

        /// <summary>
        /// Adds in a node without any connections
        /// </summary>
        /// <param name="node">The new node to be added</param>
        /// <exception cref="ArgumentException">Thrown if the node already exists in the graph</exception>
        void AddNode(GraphNode<TNodeDataType> node);

        /// <summary>
        /// Add a connection from one node to another. This is not bi-directional
        /// </summary>
        /// <param name="src">The source node to connect from</param>
        /// <param name="dst">The destination node to connect to</param>
        /// <param name="cost">The cost to get from source to destination</param>
        /// <exception cref="ArgumentNullException">Thrown if either node is null</exception>
        void AddOneDirectionalEdge(GraphNode<TNodeDataType> src, GraphNode<TNodeDataType> dst, TCost cost);

        /// <summary>
        /// Retrieves a list of nodes that are adjacent to the given root node
        /// (i.e. there's at least a 1-way connection from the root node to the nodes being returned)
        /// </summary>
        /// <param name="root">The node we're getting adjacent nodes for</param>
        /// <returns>A list of nodes that are adjacent to the root node</returns>
        /// <exception cref="ArgumentNullException">Thrown if root is null</exception>
        LinkedList<(GraphNode<TNodeDataType>, TCost)> GetAdjacentNodes(GraphNode<TNodeDataType> root);

        /// <summary>
        /// Perform a breadth-first traversal on the graph given a node
        /// </summary>
        /// <param name="root">The node we're starting the search on</param>
        /// <param name="actionOnData">The action we're going to perform on the data in the graph</param>
        void BreadthFirstTraversal(GraphNode<TNodeDataType> root, Action<GraphNode<TNodeDataType>> actionOnData);

        /// <summary>
        /// Runs a depth first traversal on the graph given a node. Will terminate if root node given doesn't have neighbours
        /// </summary>
        /// <param name="root">The node we're starting our traversal from</param>
        /// <param name="actionOnData">The action we want to run on the data in the graph</param>
        void DepthFirstTraversal(GraphNode<TNodeDataType> root, Action<GraphNode<TNodeDataType>> actionOnData);

        /// <summary>
        /// Retrieves the number of nodes in the graph
        /// </summary>
        /// <returns>Returns the number of nodes in the graph</returns>
        int GetNumberOfNodesInGraph();

        /// <summary>
        /// Retrieves all nodes within the graph (doesn't retrieve costs)
        /// </summary>
        /// <returns>The nodes within the graph</returns>
        ICollection<GraphNode<TNodeDataType>> GetAllGraphNodes();

        /// <summary>
        /// Retrieve a node that has data that fulfills a given comparison function
        /// </summary>
        /// <param name="comparisonFunction">The comparison function being used to find the node</param>
        /// <returns>A node if the comparison function returns true, null if the comparison function always returns false during the search</returns>
        GraphNode<TNodeDataType> FindNode(Func<TNodeDataType, bool> comparisonFunction);

        /// <summary>
        /// Get the minimum cost between the source node and the destination node on the graph
        /// </summary>
        /// <param name="src">The source node we're starting from</param>
        /// <param name="dst">The node we want to get to</param>
        /// <returns>The minimum cost from source node to destination node</returns>
        TCost MinimumCostBetweenTwoNodes(GraphNode<TNodeDataType> src, GraphNode<TNodeDataType> dst);

        /// <summary>
        /// Get the shortest path between the source node and the destination node on the graph
        /// </summary>
        /// <param name="src">The source node we're starting from</param>
        /// <param name="dst">The destination node we want to reach</param>
        /// <returns>A list of nodes from source to destination</returns>
        ICollection<GraphNode<TNodeDataType>> GetPathBetweenTwoNodes(GraphNode<TNodeDataType> src,
            GraphNode<TNodeDataType> dst);
    }
}