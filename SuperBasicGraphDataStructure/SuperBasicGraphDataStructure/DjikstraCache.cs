using System;

namespace SuperBasicGraphDataStructure
{
    /// <summary>
    /// Stores a graph node, it's distance between the previous node and the previous node. For use in
    /// djikstra's algorithm
    /// </summary>
    /// <typeparam name="TNodeDataType">The type of data stored in the node</typeparam>
    public class DjikstraCache<TNodeDataType>
    {
        public GraphNode<TNodeDataType> Vertex { get; }
        public int CurrentCost { get; set; } = int.MaxValue;
        public GraphNode<TNodeDataType> PreviousVertex { get; set; } = null;
    }
}