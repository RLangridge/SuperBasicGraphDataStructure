using System;
using NUnit.Framework;
using SuperBasicGraphDataStructure;

namespace SuperBasicGraphDataStructureUnitTests
{
    [TestFixture]
    public class GraphTests
    {
        private BasicAdjacencyGraph<int> _testBasicAdjacencyGraph = new BasicAdjacencyGraph<int>();

        [SetUp]
        public void Setup()
        {
            _testBasicAdjacencyGraph = new BasicAdjacencyGraph<int>();
        }

        #region Constructors
        [Test]
        public void Graph_Constructor_Normal()
        {
            _testBasicAdjacencyGraph = new BasicAdjacencyGraph<int>();
        }
        #endregion
        #region Get Adjacent Nodes

        [Test]
        public void Graph_GetAdjacentNodes_Normal()
        {
            var node1 = new GraphNode<int>(0);
            var node2 = new GraphNode<int>(1);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node1, node2);
            var adjacentNodes = _testBasicAdjacencyGraph.GetAdjacentNodes(node1);
            Assert.AreEqual(1, adjacentNodes.Count);
            Assert.AreEqual(node2, adjacentNodes.First.Value);
            Assert.AreEqual(node2.Data, adjacentNodes.First.Value.Data);
        }
        
        [Test]
        public void Graph_GetAdjacentNodes_NoConnections()
        {
            var node1 = new GraphNode<int>(0);
            var adjacentNodes = _testBasicAdjacencyGraph.GetAdjacentNodes(node1);
            Assert.AreEqual(0, adjacentNodes.Count);
        }
        
        [Test]
        public void Graph_GetAdjacentNodes_NullRoot()
        {
            Assert.Throws<ArgumentNullException>(() => _testBasicAdjacencyGraph.GetAdjacentNodes(null));
        }
        #endregion
        #region Add One Directional Connection
        [Test]
        public void Graph_AddOneDirectionalEdge_Normal()
        {
            var node1 = new GraphNode<int>(0);
            var node2 = new GraphNode<int>(1);
            Assert.DoesNotThrow(() => _testBasicAdjacencyGraph.AddOneDirectionalEdge(node1, node2));
            var adjacentNodes = _testBasicAdjacencyGraph.GetAdjacentNodes(node1);
            Assert.AreEqual(1, adjacentNodes.Count);
            Assert.AreEqual(node2, adjacentNodes.First.Value);
            Assert.AreEqual(node2.Data, adjacentNodes.First.Value.Data);
            var otherDirection = _testBasicAdjacencyGraph.GetAdjacentNodes(node2);
            Assert.AreEqual(0, otherDirection.Count);
        }
        
        [Test]
        public void Graph_AddOneDirectionalEdge_NodeBeingConnectedToIsNull()
        {
            var node1 = new GraphNode<int>(0);
            Assert.Throws<ArgumentNullException>(() => _testBasicAdjacencyGraph.AddOneDirectionalEdge(node1, null));
        }
        
        [Test]
        public void Graph_AddOneDirectionalEdge_NodeBeingConnectedFromIsNull()
        {
            var node1 = new GraphNode<int>(0);
            Assert.Throws<ArgumentNullException>(() => _testBasicAdjacencyGraph.AddOneDirectionalEdge(null, node1));
        }
        #endregion
        #region Add Edge
        [Test]
        public void Graph_AddEdge_Normal()
        {
            var node1 = new GraphNode<int>(0);
            var node2 = new GraphNode<int>(1);
            Assert.DoesNotThrow(() => _testBasicAdjacencyGraph.AddEdge(node1, node2));
            var adjacentNodes1 = _testBasicAdjacencyGraph.GetAdjacentNodes(node1);
            Assert.AreEqual(1, adjacentNodes1.Count);
            Assert.AreEqual(node2, adjacentNodes1.First.Value);
            Assert.AreEqual(node2.Data, adjacentNodes1.First.Value.Data);
            var adjacentNodes2 = _testBasicAdjacencyGraph.GetAdjacentNodes(node2);
            Assert.AreEqual(1, adjacentNodes2.Count);
            Assert.AreEqual(node1, adjacentNodes2.First.Value);
            Assert.AreEqual(node1.Data, adjacentNodes2.First.Value.Data);
        }
        #endregion
        #region Breadth First Traversal
        [Test]
        public void Graph_BreadthFirstTraversal_Normal()
        {
            var node1 = new GraphNode<int>(1);
            var node2 = new GraphNode<int>(1);
            var node3 = new GraphNode<int>(1);
            var node4 = new GraphNode<int>(1);
            
            //For this, we just have a graph that has one root node that has 3 bi-directional neighbours
            _testBasicAdjacencyGraph.AddEdge(node1, node2);
            _testBasicAdjacencyGraph.AddEdge(node1, node3);
            _testBasicAdjacencyGraph.AddEdge(node1, node4);

            var counter = 0;
            _testBasicAdjacencyGraph.BreadthFirstTraversal(node1, i => counter += i);
            Assert.AreEqual(4, counter);
        }
        
        [Test]
        public void Graph_BreadthFirstTraversal_RootNodeHasNoNeighbours()
        {
            var node1 = new GraphNode<int>(1);
            var node2 = new GraphNode<int>(1);
            var node3 = new GraphNode<int>(1);
            var node4 = new GraphNode<int>(1);
            
            //For this, we just have a graph that has one root node that has 3 bi-directional neighbours
            _testBasicAdjacencyGraph.AddEdge(node3, node2);
            _testBasicAdjacencyGraph.AddEdge(node4, node3);
            _testBasicAdjacencyGraph.AddEdge(node2, node4);

            var counter = 0;
            _testBasicAdjacencyGraph.BreadthFirstTraversal(node1, i => counter += i);
            Assert.AreEqual(0, counter);
        }
        
        [Test]
        public void Graph_BreadthFirstTraversal_CircularGraph()
        {
            var node1 = new GraphNode<int>(1);
            var node2 = new GraphNode<int>(1);
            var node3 = new GraphNode<int>(1);
            var node4 = new GraphNode<int>(1);
            
            //For this, we just have a graph that has one root node that has 3 bi-directional neighbours
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node1, node2);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node2, node3);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node3, node4);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node4, node1);

            var counter = 0;
            _testBasicAdjacencyGraph.BreadthFirstTraversal(node1, i => counter += i);
            Assert.AreEqual(4, counter);
        }
        #endregion
        #region Depth First Traversal
                [Test]
        public void Graph_DepthFirstTraversal_Normal()
        {
            var node1 = new GraphNode<int>(1);
            var node2 = new GraphNode<int>(1);
            var node3 = new GraphNode<int>(1);
            var node4 = new GraphNode<int>(1);
            
            //For this, we just have a graph that has one root node that has 3 bi-directional neighbours
            _testBasicAdjacencyGraph.AddEdge(node1, node2);
            _testBasicAdjacencyGraph.AddEdge(node1, node3);
            _testBasicAdjacencyGraph.AddEdge(node1, node4);

            var counter = 0;
            _testBasicAdjacencyGraph.DepthFirstTraversal(node1, i => counter += i);
            Assert.AreEqual(4, counter);
        }
        
        [Test]
        public void Graph_DepthFirstTraversal_RootNodeHasNoNeighbours()
        {
            var node1 = new GraphNode<int>(1);
            var node2 = new GraphNode<int>(1);
            var node3 = new GraphNode<int>(1);
            var node4 = new GraphNode<int>(1);
            
            //For this, we just have a graph that has one root node that has 3 bi-directional neighbours
            _testBasicAdjacencyGraph.AddEdge(node3, node2);
            _testBasicAdjacencyGraph.AddEdge(node4, node3);
            _testBasicAdjacencyGraph.AddEdge(node2, node4);

            var counter = 0;
            _testBasicAdjacencyGraph.DepthFirstTraversal(node1, i => counter += i);
            Assert.AreEqual(0, counter);
        }
        
        [Test]
        public void Graph_DepthFirstTraversal_CircularGraph()
        {
            var node1 = new GraphNode<int>(1);
            var node2 = new GraphNode<int>(1);
            var node3 = new GraphNode<int>(1);
            var node4 = new GraphNode<int>(1);
            
            //For this, we just have a graph that has one root node that has 3 bi-directional neighbours
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node1, node2);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node2, node3);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node3, node4);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node4, node1);

            var counter = 0;
            _testBasicAdjacencyGraph.DepthFirstTraversal(node1, i => counter += i);
            Assert.AreEqual(4, counter);
        }
        #endregion
    }
}