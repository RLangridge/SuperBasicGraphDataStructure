using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        public void BasicAdjacencyGraph_Constructor_Normal()
        {
            _testBasicAdjacencyGraph = new BasicAdjacencyGraph<int>();
        }
        #endregion
        #region Add Node

        [Test]
        public void BasicAdjacencyGraph_AddNode_Normal()
        {
            var node2 = new GraphNode<int>(1);
            Assert.DoesNotThrow(() => _testBasicAdjacencyGraph.AddNode(node2));
            Assert.AreEqual(1, _testBasicAdjacencyGraph.GetNumberOfNodesInGraph());
        }
        
        [Test]
        public void BasicAdjacencyGraph_AddNode_NodeAlreadyExists()
        {
            var node1 = new GraphNode<int>(0);
            _testBasicAdjacencyGraph.AddNode(node1);
            Assert.Throws<ArgumentException>(() => _testBasicAdjacencyGraph.AddNode(node1));
            Assert.AreEqual(1, _testBasicAdjacencyGraph.GetNumberOfNodesInGraph());
        }
        
        [Test]
        public void BasicAdjacencyGraph_AddNode_NullNodeAdded()
        {
            Assert.Throws<ArgumentNullException>(() => _testBasicAdjacencyGraph.AddNode(null));
            Assert.AreEqual(0, _testBasicAdjacencyGraph.GetNumberOfNodesInGraph());
        }
        #endregion
        #region Get Adjacent Nodes

        [Test]
        public void BasicAdjacencyGraph_GetAdjacentNodes_Normal()
        {
            var node1 = new GraphNode<int>(0);
            var node2 = new GraphNode<int>(1);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node1, node2, 0);
            var adjacentNodes = _testBasicAdjacencyGraph.GetAdjacentNodes(node1);
            Assert.AreEqual(1, adjacentNodes.Count);
            Assert.AreEqual(node2, adjacentNodes.First.Value.Item1);
            Assert.AreEqual(node2.Data, adjacentNodes.First.Value.Item1.Data);
        }
        
        [Test]
        public void BasicAdjacencyGraph_GetAdjacentNodes_NoConnections()
        {
            var node1 = new GraphNode<int>(0);
            var adjacentNodes = _testBasicAdjacencyGraph.GetAdjacentNodes(node1);
            Assert.AreEqual(0, adjacentNodes.Count);
        }
        
        [Test]
        public void BasicAdjacencyGraph_GetAdjacentNodes_NullRoot()
        {
            Assert.Throws<ArgumentNullException>(() => _testBasicAdjacencyGraph.GetAdjacentNodes(null));
        }
        #endregion
        #region Add One Directional Connection
        [Test]
        public void BasicAdjacencyGraph_AddOneDirectionalEdge_Normal()
        {
            var node1 = new GraphNode<int>(0);
            var node2 = new GraphNode<int>(1);
            var costBetweenNodes = 3;
            Assert.DoesNotThrow(() => _testBasicAdjacencyGraph.AddOneDirectionalEdge(node1, node2, costBetweenNodes));
            var adjacentNodes = _testBasicAdjacencyGraph.GetAdjacentNodes(node1);
            Assert.AreEqual(1, adjacentNodes.Count);
            Assert.AreEqual(node2, adjacentNodes.First.Value.Item1);
            Assert.AreEqual(costBetweenNodes, adjacentNodes.First.Value.Item2);
            Assert.AreEqual(node2.Data, adjacentNodes.First.Value.Item1.Data);
            var otherDirection = _testBasicAdjacencyGraph.GetAdjacentNodes(node2);
            Assert.AreEqual(0, otherDirection.Count);
        }
        
        [Test]
        public void BasicAdjacencyGraph_AddOneDirectionalEdge_NodeBeingConnectedToIsNull()
        {
            var node1 = new GraphNode<int>(0);
            Assert.Throws<ArgumentNullException>(() => _testBasicAdjacencyGraph.AddOneDirectionalEdge(node1, null, 0));
        }
        
        [Test]
        public void BasicAdjacencyGraph_AddOneDirectionalEdge_NodeBeingConnectedFromIsNull()
        {
            var node1 = new GraphNode<int>(0);
            Assert.Throws<ArgumentNullException>(() => _testBasicAdjacencyGraph.AddOneDirectionalEdge(null, node1, 0));
        }
        #endregion
        #region Add Edge
        [Test]
        public void BasicAdjacencyGraph_AddEdge_Normal()
        {
            var node1 = new GraphNode<int>(0);
            var node2 = new GraphNode<int>(1);
            var srcToDst = 2;
            var dstToSrc = 3;
            Assert.DoesNotThrow(() => _testBasicAdjacencyGraph.AddEdge(node1, node2, srcToDst, dstToSrc));
            var adjacentNodes1 = _testBasicAdjacencyGraph.GetAdjacentNodes(node1);
            Assert.AreEqual(1, adjacentNodes1.Count);
            Assert.AreEqual(node2, adjacentNodes1.First.Value.Item1);
            Assert.AreEqual(node2.Data, adjacentNodes1.First.Value.Item1.Data);
            Assert.AreEqual(srcToDst, adjacentNodes1.First.Value.Item2);
            var adjacentNodes2 = _testBasicAdjacencyGraph.GetAdjacentNodes(node2);
            Assert.AreEqual(1, adjacentNodes2.Count);
            Assert.AreEqual(node1, adjacentNodes2.First.Value.Item1);
            Assert.AreEqual(node1.Data, adjacentNodes2.First.Value.Item1.Data);
            Assert.AreEqual(dstToSrc, adjacentNodes2.First.Value.Item2);
        }
        #endregion
        #region Breadth First Traversal
        [Test]
        public void BasicAdjacencyGraph_BreadthFirstTraversal_Normal()
        {
            var node1 = new GraphNode<int>(1);
            var node2 = new GraphNode<int>(1);
            var node3 = new GraphNode<int>(1);
            var node4 = new GraphNode<int>(1);
            
            //For this, we just have a graph that has one root node that has 3 bi-directional neighbours
            _testBasicAdjacencyGraph.AddEdge(node1, node2, 0, 0);
            _testBasicAdjacencyGraph.AddEdge(node1, node3, 0, 0);
            _testBasicAdjacencyGraph.AddEdge(node1, node4, 0, 0);

            var counter = 0;
            _testBasicAdjacencyGraph.BreadthFirstTraversal(node1, i => counter += i.Data);
            Assert.AreEqual(4, counter);
        }
        
        [Test]
        public void BasicAdjacencyGraph_BreadthFirstTraversal_RootNodeHasNoNeighbours()
        {
            var node1 = new GraphNode<int>(1);
            var node2 = new GraphNode<int>(1);
            var node3 = new GraphNode<int>(1);
            var node4 = new GraphNode<int>(1);
            
            //For this, we just have a graph that has one root node that has 3 bi-directional neighbours
            _testBasicAdjacencyGraph.AddEdge(node3, node2, 0, 0);
            _testBasicAdjacencyGraph.AddEdge(node4, node3, 0, 0);
            _testBasicAdjacencyGraph.AddEdge(node2, node4, 0, 0);

            var counter = 0;
            _testBasicAdjacencyGraph.BreadthFirstTraversal(node1, i => counter += i.Data);
            Assert.AreEqual(0, counter);
        }
        
        [Test]
        public void BasicAdjacencyGraph_BreadthFirstTraversal_CircularGraph()
        {
            var node1 = new GraphNode<int>(1);
            var node2 = new GraphNode<int>(1);
            var node3 = new GraphNode<int>(1);
            var node4 = new GraphNode<int>(1);
            
            //For this, we just have a graph that has one root node that has 3 bi-directional neighbours
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node1, node2, 0);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node2, node3, 0);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node3, node4, 0);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node4, node1, 0);

            var counter = 0;
            _testBasicAdjacencyGraph.BreadthFirstTraversal(node1, i => counter += i.Data);
            Assert.AreEqual(4, counter);
        }
        #endregion
        #region Depth First Traversal
                [Test]
        public void BasicAdjacencyGraph_DepthFirstTraversal_Normal()
        {
            var node1 = new GraphNode<int>(1);
            var node2 = new GraphNode<int>(1);
            var node3 = new GraphNode<int>(1);
            var node4 = new GraphNode<int>(1);
            
            //For this, we just have a graph that has one root node that has 3 bi-directional neighbours
            _testBasicAdjacencyGraph.AddEdge(node1, node2, 0, 0);
            _testBasicAdjacencyGraph.AddEdge(node1, node3, 0, 0);
            _testBasicAdjacencyGraph.AddEdge(node1, node4, 0, 0);

            var counter = 0;
            _testBasicAdjacencyGraph.DepthFirstTraversal(node1, i => counter += i.Data);
            Assert.AreEqual(4, counter);
        }
        
        [Test]
        public void BasicAdjacencyGraph_DepthFirstTraversal_RootNodeHasNoNeighbours()
        {
            var node1 = new GraphNode<int>(1);
            var node2 = new GraphNode<int>(1);
            var node3 = new GraphNode<int>(1);
            var node4 = new GraphNode<int>(1);
            
            //For this, we just have a graph that has one root node that has 3 bi-directional neighbours
            _testBasicAdjacencyGraph.AddEdge(node3, node2, 0, 0);
            _testBasicAdjacencyGraph.AddEdge(node4, node3, 0, 0);
            _testBasicAdjacencyGraph.AddEdge(node2, node4, 0, 0);

            var counter = 0;
            _testBasicAdjacencyGraph.DepthFirstTraversal(node1, i => counter += i.Data);
            Assert.AreEqual(0, counter);
        }
        
        [Test]
        public void BasicAdjacencyGraph_DepthFirstTraversal_CircularGraph()
        {
            var node1 = new GraphNode<int>(1);
            var node2 = new GraphNode<int>(1);
            var node3 = new GraphNode<int>(1);
            var node4 = new GraphNode<int>(1);
            
            //For this, we just have a graph that has one root node that has 3 bi-directional neighbours
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node1, node2, 0);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node2, node3, 0);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node3, node4, 0);
            _testBasicAdjacencyGraph.AddOneDirectionalEdge(node4, node1, 0);

            var counter = 0;
            _testBasicAdjacencyGraph.DepthFirstTraversal(node1, i => counter += i.Data);
            Assert.AreEqual(4, counter);
        }
        #endregion
        #region Minimum Cost Between Two Nodes
        [Test]
        public void BasicAdjacencyGraph_MinimumCostBetweenTwoNodes_Normal()
        {
            var a = new GraphNode<string>("A");
            var b = new GraphNode<string>("B");
            var c = new GraphNode<string>("C");
            var d = new GraphNode<string>("D");
            var e = new GraphNode<string>("E");
            
            var stringGraph = new BasicAdjacencyGraph<string>();
            
            stringGraph.AddEdge(a, b, 6, 6);
            stringGraph.AddEdge(a, d, 1, 1);
            stringGraph.AddEdge(b, d, 2, 2);
            stringGraph.AddEdge(b, e, 2, 2);
            stringGraph.AddEdge(b, c, 5, 5);
            stringGraph.AddEdge(c, e, 5, 5);
            stringGraph.AddEdge(d, e, 1, 1);
            
            Assert.AreEqual(3, stringGraph.MinimumCostBetweenTwoNodes(a, b));
            Assert.AreEqual(1, stringGraph.MinimumCostBetweenTwoNodes(a, d));
            Assert.AreEqual(7, stringGraph.MinimumCostBetweenTwoNodes(a, c));
        }
        
        [Test]
        public void BasicAdjacencyGraph_MinimumCostBetweenTwoNodes_SecondSearch()
        {
            var a = new GraphNode<string>("A");
            var b = new GraphNode<string>("B");
            var c = new GraphNode<string>("C");
            var d = new GraphNode<string>("D");
            var e = new GraphNode<string>("E");
            
            var stringGraph = new BasicAdjacencyGraph<string>();
            
            stringGraph.AddEdge(a, b, 6, 6);
            stringGraph.AddEdge(a, d, 1, 1);
            stringGraph.AddEdge(b, d, 2, 2);
            stringGraph.AddEdge(b, e, 2, 2);
            stringGraph.AddEdge(b, c, 5, 5);
            stringGraph.AddEdge(c, e, 5, 5);
            stringGraph.AddEdge(d, e, 1, 1);

            stringGraph.MinimumCostBetweenTwoNodes(a, b); // First search which builds the cache around A
            Assert.AreEqual(1, stringGraph.MinimumCostBetweenTwoNodes(d, e));
        }
        
        [Test]
        public void BasicAdjacencyGraph_MinimumCostBetweenTwoNodes_NullSource()
        {
            var a = new GraphNode<string>("A");
            var b = new GraphNode<string>("B");
            var c = new GraphNode<string>("C");
            var d = new GraphNode<string>("D");
            var e = new GraphNode<string>("E");
            
            var stringGraph = new BasicAdjacencyGraph<string>();
            
            stringGraph.AddEdge(a, b, 6, 6);
            stringGraph.AddEdge(a, d, 1, 1);
            stringGraph.AddEdge(b, d, 2, 2);
            stringGraph.AddEdge(b, e, 2, 2);
            stringGraph.AddEdge(b, c, 5, 5);
            stringGraph.AddEdge(c, e, 5, 5);
            stringGraph.AddEdge(d, e, 1, 1);
            
            Assert.Throws<ArgumentNullException>(() => stringGraph.MinimumCostBetweenTwoNodes(null, e));
        }
        
        [Test]
        public void BasicAdjacencyGraph_MinimumCostBetweenTwoNodes_NullDestination()
        {
            var a = new GraphNode<string>("A");
            var b = new GraphNode<string>("B");
            var c = new GraphNode<string>("C");
            var d = new GraphNode<string>("D");
            var e = new GraphNode<string>("E");
            
            var stringGraph = new BasicAdjacencyGraph<string>();
            
            stringGraph.AddEdge(a, b, 6, 6);
            stringGraph.AddEdge(a, d, 1, 1);
            stringGraph.AddEdge(b, d, 2, 2);
            stringGraph.AddEdge(b, e, 2, 2);
            stringGraph.AddEdge(b, c, 5, 5);
            stringGraph.AddEdge(c, e, 5, 5);
            stringGraph.AddEdge(d, e, 1, 1);
            
            Assert.Throws<ArgumentNullException>(() => stringGraph.MinimumCostBetweenTwoNodes(a, null));
        }
        
        [Test]
        public void BasicAdjacencyGraph_MinimumCostBetweenTwoNodes_SourceNodeHasNoNeighbours()
        {
            var a = new GraphNode<string>("A");
            var b = new GraphNode<string>("B");
            var c = new GraphNode<string>("C");
            var d = new GraphNode<string>("D");
            var e = new GraphNode<string>("E");
            
            var stringGraph = new BasicAdjacencyGraph<string>();
            
            stringGraph.AddNode(a);
            stringGraph.AddEdge(b, d, 2, 2);
            stringGraph.AddEdge(b, e, 2, 2);
            stringGraph.AddEdge(b, c, 5, 5);
            stringGraph.AddEdge(c, e, 5, 5);
            stringGraph.AddEdge(d, e, 1, 1);

            Assert.Throws<ConstraintException>(() => stringGraph.MinimumCostBetweenTwoNodes(a, b));
        }
        
        [Test]
        public void BasicAdjacencyGraph_MinimumCostBetweenTwoNodes_DestinationNodeNodeHasNoNeighbours()
        {
            var a = new GraphNode<string>("A");
            var b = new GraphNode<string>("B");
            var c = new GraphNode<string>("C");
            var d = new GraphNode<string>("D");
            var e = new GraphNode<string>("E");
            
            var stringGraph = new BasicAdjacencyGraph<string>();
            
            stringGraph.AddEdge(a, d, 1, 1);
            stringGraph.AddEdge(c, e, 5, 5);
            stringGraph.AddEdge(d, e, 1, 1);

            Assert.Throws<ConstraintException>(() => stringGraph.MinimumCostBetweenTwoNodes(a, b));
        }
        
        [Test]
        public void BasicAdjacencyGraph_MinimumCostBetweenTwoNodes_DisconnectedGraph()
        {
            var a = new GraphNode<string>("A");
            var b = new GraphNode<string>("B");
            var c = new GraphNode<string>("C");
            var d = new GraphNode<string>("D");
            var e = new GraphNode<string>("E");
            var f = new GraphNode<string>("F");
            
            var stringGraph = new BasicAdjacencyGraph<string>();
            
            stringGraph.AddEdge(a, b, 6, 6);
            stringGraph.AddEdge(a, d, 1, 1);
            stringGraph.AddEdge(b, d, 2, 2);
            stringGraph.AddEdge(b, e, 2, 2);
            stringGraph.AddEdge(d, e, 1, 1);
            stringGraph.AddEdge(c, f, 1, 1);
            
            
            Assert.Throws<ConstraintException>(() => stringGraph.MinimumCostBetweenTwoNodes(a, c));
        }
        #endregion
        #region Get Path Between Two Nodes
        [Test]
        public void BasicAdjacencyGraph_GetPathBetweenTwoNodes_Normal()
        {
            var a = new GraphNode<string>("A");
            var b = new GraphNode<string>("B");
            var c = new GraphNode<string>("C");
            var d = new GraphNode<string>("D");
            var e = new GraphNode<string>("E");
            
            var stringGraph = new BasicAdjacencyGraph<string>();
            
            stringGraph.AddEdge(a, b, 6, 6);
            stringGraph.AddEdge(a, d, 1, 1);
            stringGraph.AddEdge(b, d, 2, 2);
            stringGraph.AddEdge(b, e, 2, 2);
            stringGraph.AddEdge(b, c, 5, 5);
            stringGraph.AddEdge(c, e, 5, 5);
            stringGraph.AddEdge(d, e, 1, 1);
            
            List<GraphNode<string>> nodeList = new List<GraphNode<string>>();
            Assert.DoesNotThrow(() => nodeList = (List<GraphNode<string>>)stringGraph.GetPathBetweenTwoNodes(a, b));
            Assert.AreEqual(3, nodeList.Count);
            Assert.AreEqual("A", nodeList.Last().Data);
            Assert.AreEqual("B", nodeList.First().Data);
            Assert.AreEqual("D", nodeList[1].Data);
            
            Assert.DoesNotThrow(() => nodeList = (List<GraphNode<string>>)stringGraph.GetPathBetweenTwoNodes(a, c));
            Assert.AreEqual(4, nodeList.Count);
            var secondPath = new[] {"C", "E", "D", "A"};
            for(int index = 0; index < nodeList.Count; ++index)
                Assert.AreEqual(secondPath[index], nodeList[index].Data);
        }
        #endregion
    }
}