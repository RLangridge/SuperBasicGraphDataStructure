using System;
using System.Collections.Generic;

namespace SuperBasicGraphDataStructure
{
    /// <summary>
    /// Graph node details the node of a graph in a adjacency list format. 
    /// </summary>
    /// <typeparam name="NodeDataType">The data type of the object that will be stored in the node</typeparam>
    public class GraphNode<NodeDataType>
    {
        public NodeDataType Data { get; }
        public LinkedList<GraphNode<NodeDataType>> AdjacentNodes { get; }

        public GraphNode(NodeDataType data)
        {
            Data = data;
            AdjacentNodes = new LinkedList<GraphNode<NodeDataType>>();
        }
        
        public GraphNode(NodeDataType data, LinkedList<GraphNode<NodeDataType>> adjacentNodes)
        {
            Data = data;
            AdjacentNodes = adjacentNodes;
        }

        /// <summary>
        /// Add a node as an adjacent node. Note this only creates a one directional relationship.
        /// </summary>
        /// <param name="node">The node that this one is going to be adjacent to</param>
        /// <exception cref="ArgumentNullException">Thrown when the node passed through is null</exception>
        /// <exception cref="ArgumentException">Thrown when the node already exists in the adjacency list</exception>
        public void AddAdjacentNode(GraphNode<NodeDataType> node)
        {
            if(node == null)
                throw new ArgumentNullException(nameof(node), "Node passed through to add adjacent is null");
            if(AdjacentNodes.Contains(node))
                throw new ArgumentException("Node is already adjacent to the given node.");
            AdjacentNodes.AddLast(node);
        }

        public bool IsBidirectional(GraphNode<NodeDataType> adjacentNode)
        {
            return AdjacentNodes.Contains(adjacentNode) && adjacentNode.AdjacentNodes.Contains(this);
        }
    }
}