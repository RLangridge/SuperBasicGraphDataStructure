using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;
using SuperBasicGraphDataStructure;

namespace SuperBasicGraphDataStructureUnitTests
{
    [TestFixture]
    public class PriorityQueueTests
    {
        private PriorityQueue<int> _newPriorityQueue = new PriorityQueue<int>();
        private readonly SortInt _comparer = new SortInt();

        private class SortInt : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                if (x < y)
                    return -1;
                return x > y ? 1 : 0;
            }
        }

        private int CompareInt(int x, int y)
        {
            if (x < y)
                return -1;
            return x > y ? 1 : 0;
        }

        [SetUp]
        public void Setup()
        {
            _newPriorityQueue = new PriorityQueue<int>();
        }

        #region Add

        [Test]
        public void PriorityQueue_Add_Normal()
        {
            var p = 4;
            Assert.DoesNotThrow(() => _newPriorityQueue.Add(p, _comparer));
            Assert.AreEqual(1, _newPriorityQueue.Count);
            Assert.AreEqual(4, _newPriorityQueue.First());
            Assert.AreEqual(4, _newPriorityQueue.Last());
        }
        
        [Test]
        public void PriorityQueue_Add_RisingValues()
        {
            var a = 1;
            var b = 2; 
            var c = 3;
            _newPriorityQueue.Add(c, _comparer);
            _newPriorityQueue.Add(b, _comparer);
            _newPriorityQueue.Add(a, _comparer);
            Assert.AreEqual(3, _newPriorityQueue.Count);
            Assert.AreEqual(1, _newPriorityQueue.First());
            Assert.AreEqual(3, _newPriorityQueue.Last());
        }
        
        [Test]
        public void PriorityQueue_Add_Falling()
        {
            var a = 1;
            var b = 2; 
            var c = 3;
            _newPriorityQueue.Add(a, _comparer);
            _newPriorityQueue.Add(b, _comparer);
            _newPriorityQueue.Add(c, _comparer);
            Assert.AreEqual(3, _newPriorityQueue.Count);
            Assert.AreEqual(1, _newPriorityQueue.First());
            Assert.AreEqual(3, _newPriorityQueue.Last());
        }
        
        [Test]
        public void PriorityQueue_Add_MixedValues()
        {
            var a = 1;
            var b = 2; 
            var c = 3;
            _newPriorityQueue.Add(a, _comparer);
            _newPriorityQueue.Add(c, _comparer);
            _newPriorityQueue.Add(b, _comparer);
            Assert.AreEqual(3, _newPriorityQueue.Count);
            Assert.AreEqual(1, _newPriorityQueue.First());
            Assert.AreEqual(3, _newPriorityQueue.Last());
        }

        [Test]
        public void PriorityQueue_Add2_Normal()
        {
            var p = 4;
            Assert.DoesNotThrow(() => _newPriorityQueue.Add(p, CompareInt));
            Assert.AreEqual(1, _newPriorityQueue.Count);
            Assert.AreEqual(4, _newPriorityQueue.First());
            Assert.AreEqual(4, _newPriorityQueue.Last());
        }
        
        [Test]
        public void PriorityQueue_Add2_RisingValues()
        {
            var a = 1;
            var b = 2; 
            var c = 3;
            _newPriorityQueue.Add(c, CompareInt);
            _newPriorityQueue.Add(b, CompareInt);
            _newPriorityQueue.Add(a, CompareInt);
            Assert.AreEqual(3, _newPriorityQueue.Count);
            Assert.AreEqual(1, _newPriorityQueue.First());
            Assert.AreEqual(3, _newPriorityQueue.Last());
        }
        
        [Test]
        public void PriorityQueue_Add2_Falling()
        {
            var a = 1;
            var b = 2; 
            var c = 3;
            _newPriorityQueue.Add(a, CompareInt);
            _newPriorityQueue.Add(b, CompareInt);
            _newPriorityQueue.Add(c, CompareInt);
            Assert.AreEqual(3, _newPriorityQueue.Count);
            Assert.AreEqual(1, _newPriorityQueue.First());
            Assert.AreEqual(3, _newPriorityQueue.Last());
        }
        
        [Test]
        public void PriorityQueue_Add2_MixedValues()
        {
            var a = 1;
            var b = 2; 
            var c = 3;
            _newPriorityQueue.Add(a, CompareInt);
            _newPriorityQueue.Add(c, CompareInt);
            _newPriorityQueue.Add(b, CompareInt);
            Assert.AreEqual(3, _newPriorityQueue.Count);
            Assert.AreEqual(1, _newPriorityQueue.First());
            Assert.AreEqual(3, _newPriorityQueue.Last());
        }
        #endregion
        
        #region Pop First
        [Test]
        public void PriorityQueue_PopFirst_Normal()
        {
            var a = 1;
            var b = 2; 
            var c = 3;
            var comparer = new SortInt();
            _newPriorityQueue.Add(a, comparer);
            _newPriorityQueue.Add(c, comparer);
            _newPriorityQueue.Add(b, comparer);

            var item = _newPriorityQueue.PopFirst();
            Assert.AreEqual(1, item);
            Assert.AreEqual(2, _newPriorityQueue.Count);
            Assert.AreEqual(2, _newPriorityQueue.First());
            Assert.AreEqual(3, _newPriorityQueue.Last());
        }
        
        [Test]
        public void PriorityQueue_PopFirst_NoElements()
        {
            Assert.Throws<ConstraintException>(() => _newPriorityQueue.PopFirst());
        }
        #endregion
        #region Pop Last
        [Test]
        public void PriorityQueue_PopLast_Normal()
        {
            var a = 1;
            var b = 2; 
            var c = 3;
            var comparer = new SortInt();
            _newPriorityQueue.Add(a, comparer);
            _newPriorityQueue.Add(c, comparer);
            _newPriorityQueue.Add(b, comparer);

            var item = _newPriorityQueue.PopLast();
            Assert.AreEqual(3, item);
            Assert.AreEqual(2, _newPriorityQueue.Count);
            Assert.AreEqual(1, _newPriorityQueue.First());
            Assert.AreEqual(2, _newPriorityQueue.Last());
        }
        
        [Test]
        public void PriorityQueue_PopLast_NoElements()
        {
            Assert.Throws<ConstraintException>(() => _newPriorityQueue.PopLast());
        }
        #endregion
        #region Find And Replace
        [Test]
        public void PriorityQueue_FindAndReplace_Normal()
        {
            var a = 1;
            var b = 2; 
            var c = 3;
            var comparer = new SortInt();
            _newPriorityQueue.Add(c, comparer);
            _newPriorityQueue.Add(b, comparer);
            _newPriorityQueue.Add(a, comparer);
            _newPriorityQueue.FindAndReplace(1, 55, comparer);
            Assert.AreEqual(3, _newPriorityQueue.Count);
            Assert.AreEqual(2, _newPriorityQueue.First());
            Assert.AreEqual(55, _newPriorityQueue.Last());
        }
        
        [Test]
        public void PriorityQueue_FindAndReplace_Missing()
        {
            var a = 1;
            var b = 2; 
            var c = 3;
            var comparer = new SortInt();
            _newPriorityQueue.Add(c, comparer);
            _newPriorityQueue.Add(b, comparer);
            _newPriorityQueue.Add(a, comparer);
            Assert.Throws<ArgumentException>(() => _newPriorityQueue.FindAndReplace(77, 55, comparer));
            Assert.AreEqual(3, _newPriorityQueue.Count);
            Assert.AreEqual(1, _newPriorityQueue.First());
            Assert.AreEqual(3, _newPriorityQueue.Last());
        }

        [Test]
        public void PriorityQueue_FindAndReplace2_Normal()
        {
            var a = 1;
            var b = 2; 
            var c = 3;
            var comparer = new SortInt();
            _newPriorityQueue.Add(c, comparer);
            _newPriorityQueue.Add(b, comparer);
            _newPriorityQueue.Add(a, comparer);
            _newPriorityQueue.FindAndReplace(1, 55, (i, i1) => i == i1 ? 0 : -1);
            Assert.AreEqual(3, _newPriorityQueue.Count);
            Assert.AreEqual(2, _newPriorityQueue.First());
            Assert.AreEqual(55, _newPriorityQueue.Last());
        }
        
        [Test]
        public void PriorityQueue_FindAndReplace2_Missing()
        {
            var a = 1;
            var b = 2; 
            var c = 3;
            var comparer = new SortInt();
            _newPriorityQueue.Add(c, comparer);
            _newPriorityQueue.Add(b, comparer);
            _newPriorityQueue.Add(a, comparer);
            Assert.Throws<ArgumentException>(() => _newPriorityQueue.FindAndReplace(77, 55, (i, i1) => i == i1 ? 0 : -1));
            Assert.AreEqual(3, _newPriorityQueue.Count);
            Assert.AreEqual(1, _newPriorityQueue.First());
            Assert.AreEqual(3, _newPriorityQueue.Last());
        }
        #endregion
    }
}