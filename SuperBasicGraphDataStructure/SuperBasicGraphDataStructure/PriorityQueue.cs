using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SuperBasicGraphDataStructure
{
    /// <summary>
    /// Priority queue setup. Used in this project for djikstra's algorithm, however should be generic enough to
    /// use anywhere.
    /// </summary>
    /// <typeparam name="TItem">The type of item you want to use. Must inherit from IComparable</typeparam>
    public class PriorityQueue<TItem> : ICollection where TItem : IComparable
    {
        private List<TItem> _itemSet = new List<TItem>(); // The list we're keeping track of

        /// <summary>
        /// Add objects in order
        /// </summary>
        /// <param name="newItem">The item being added</param>
        /// <param name="comparerFunc">The comparison function being run when adding items (used for sorting)</param>
        public void Add(TItem newItem, IComparer<TItem> comparerFunc)
        {
            if(_itemSet.Count == 0) // If this is the case, we can just add it normally
                _itemSet.Add(newItem);
            else
            {
                // While the comparison yields -1, continue it. Once it's done, we can insert the item at the counter position
                var counter = _itemSet.TakeWhile(item => comparerFunc.Compare(item, newItem) == -1).Count();
                _itemSet.Insert(counter, newItem);
            }
        }

        /// <summary>
        /// Remove an item from the list
        /// </summary>
        /// <param name="item">The item to be removed</param>
        public void Remove(TItem item)
        {
            _itemSet.Remove(item);
        }

        /// <summary>
        /// Clear the list of all elements
        /// </summary>
        public void Clear()
        {
            _itemSet.Clear();
        }

        /// <summary>
        /// Retrieve the first value in the list (uses first or default from LINQ)
        /// </summary>
        /// <returns>The first item of the list or if the list is empty, the default item</returns>
        public TItem First()
        {
            return _itemSet.FirstOrDefault();
        }

        /// <summary>
        /// Retrieve the last value in the list (uses last or default from LINQ)
        /// </summary>
        /// <returns>The last item of the list or if the list is empty, the default item</returns>
        public TItem Last()
        {
            return _itemSet.LastOrDefault();
        }

        /// <summary>
        /// Retrieves the list enumerator
        /// </summary>
        /// <returns>The enumerator of the underlying list</returns>
        public IEnumerator GetEnumerator()
        {
            return _itemSet.GetEnumerator();
        }

        /// <summary>
        /// Copies over the data from this list to the given array at a specific index
        /// </summary>
        /// <param name="array">The array we want to copy to</param>
        /// <param name="index">The starting index of where we're copying</param>
        public void CopyTo(Array array, int index)
        {
            array.CopyTo(_itemSet.ToArray(), index);
        }

        /// <summary>
        /// Retrieve the first element and remove it from the list
        /// </summary>
        /// <returns>The first element of the list</returns>
        /// <exception cref="ConstraintException">Thrown when there are no elements in the list</exception>
        public TItem PopFirst()
        {
            if(Count == 0)
                throw new ConstraintException("Can't pop an empty priority queue");

            var first = First();
            _itemSet.RemoveAt(0);
            return first;
        }

        /// <summary>
        /// Retrieve the lsat element and remove it from the list
        /// </summary>
        /// <returns>The last element of the list</returns>
        /// <exception cref="ConstraintException">Thrown when there are no elements in the list</exception>
        public TItem PopLast()
        {
            if(Count == 0)
                throw new ConstraintException("Can't pop an empty priority queue");

            var last = Last();
            _itemSet.RemoveAt(Count - 1);
            return last;
        }

        public int Count => _itemSet.Count;
        public bool IsSynchronized { get; }
        public object SyncRoot { get; }
    }
}