using System;
using System.Collections.Generic;

namespace SuperBasicGraphDataStructure
{
    /// <summary>
    /// Graph node details the node of a graph
    /// </summary>
    /// <typeparam name="TNodeDataType">The data type of the object that will be stored in the node</typeparam>
    public class GraphNode<TNodeDataType>
    {
        public TNodeDataType Data { get; }

        public GraphNode(TNodeDataType data)
        {
            Data = data;
        }
    }
}