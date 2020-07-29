using System;
using NUnit.Framework;
using SuperBasicGraphDataStructure;

namespace SuperBasicGraphDataStructureUnitTests
{
    [TestFixture]
    public class GraphNodeTests
    {
        private GraphNode<int> _testPrimitiveGraphNode = new GraphNode<int>(0);
        private GraphNode<int> _adjacentNode = new GraphNode<int>(1);

        [SetUp]
        public void Setup()
        {
            _testPrimitiveGraphNode = new GraphNode<int>(0);
            _adjacentNode = new GraphNode<int>(1);
        }

        #region Constructors
        [Test]
        public void GraphNode_Constructor_Normal()
        {
            Assert.DoesNotThrow(() => _testPrimitiveGraphNode = new GraphNode<int>(1));
            Assert.AreEqual(1, _testPrimitiveGraphNode.Data);
        }
        #endregion
        
        #region Add Adjacent Node

        [Test]
        public void GraphNode_AddAdjacentNode_Normal()
        {
            Assert.DoesNotThrow(() => _testPrimitiveGraphNode.AddAdjacentNode(_adjacentNode));
            Assert.AreEqual(1, _testPrimitiveGraphNode.AdjacentNodes.Count);
            Assert.NotNull(_testPrimitiveGraphNode.AdjacentNodes.First.Value);
            Assert.AreEqual(1, _testPrimitiveGraphNode.AdjacentNodes.First.Value.Data);
        }

        [Test]
        public void GraphNode_AddAdjacentNode_AddDuplicate()
        {
            Assert.DoesNotThrow(() => _testPrimitiveGraphNode.AddAdjacentNode(_adjacentNode));
            Assert.Throws<ArgumentException>(() => _testPrimitiveGraphNode.AddAdjacentNode(_adjacentNode));
        }

        [Test]
        public void GraphNode_AddAdjacentNode_AddNullNode()
        {
            Assert.Throws<ArgumentNullException>(() => _testPrimitiveGraphNode.AddAdjacentNode(null));
        }
        #endregion
        
        #region Is Bidirectional

        [Test]
        public void GraphNode_IsBidirectional_Normal()
        {
            _testPrimitiveGraphNode.AddAdjacentNode(_adjacentNode);
            _adjacentNode.AddAdjacentNode(_testPrimitiveGraphNode);
            
            Assert.True(_testPrimitiveGraphNode.IsBidirectional(_adjacentNode));
            Assert.True(_adjacentNode.IsBidirectional(_testPrimitiveGraphNode));
        }

        [Test]
        public void GraphNode_IsBidirectional_OneWayRelationship()
        {
            Assert.False(_testPrimitiveGraphNode.IsBidirectional(_adjacentNode));
            Assert.False(_adjacentNode.IsBidirectional(_testPrimitiveGraphNode));
        }
        
        [Test]
        public void GraphNode_IsBidirectional_NoRelationship()
        {
            Assert.False(_testPrimitiveGraphNode.IsBidirectional(_adjacentNode));
            Assert.False(_adjacentNode.IsBidirectional(_testPrimitiveGraphNode));
        }
        #endregion
    }
}