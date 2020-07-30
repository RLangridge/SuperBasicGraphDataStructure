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
        

    }
}