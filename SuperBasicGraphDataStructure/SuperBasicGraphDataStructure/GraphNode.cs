using System;
using System.Collections.Generic;

namespace SuperBasicGraphDataStructure
{
    /// <summary>
    /// Graph node details the node of a graph
    /// </summary>
    /// <typeparam name="NodeDataType">The data type of the object that will be stored in the node</typeparam>
    public class GraphNode<NodeDataType>
    {
        public NodeDataType Data { get; }

        public GraphNode(NodeDataType data)
        {
            Data = data;
        }
    }
}