#region
using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace SFMLStart.Utilities
{
    /// <summary>
    ///   The BinaryTree class implements a simple non-balanced sorted Binary Tree in C#
    /// </summary>
    /// <typeparam name="T"> The tree can contain any type, but you are required to provide your own comparison function. </typeparam>
    public class BinaryTree<T>
    {
        private readonly Comparison<T> _compareFunction;
        private BinaryTreeNode _root;

        /// <summary>
        ///   The BinaryTree constructor requires that we pass a comparison function. We need one as generics can only be compared as equals, but not for order. The solution is to allow the caller to pass a suitable comparison function. We use the C# Comparison delegate for this (found in System.Collections)
        /// </summary>
        /// <param name="theCompareFunction"> Pass a delegate function of the type Comparison to the function </param>
        public BinaryTree(Comparison<T> theCompareFunction)
        {
            _root = null;
            _compareFunction = theCompareFunction;
        }

        /// <summary>
        ///   For integer comparisons we provide a demonstration function.
        /// </summary>
        /// <param name="left"> </param>
        /// <param name="right"> </param>
        /// <returns> &lt;0 for left smaller than right, &gt;0 if they are equal, +1 if right is larger than left </returns>
        public static int CompareFunctionInt(int left, int right) { return left - right; }

        /// <summary>
        ///   For string comparisons we provide a demonstration function
        /// </summary>
        /// <param name="left"> </param>
        /// <param name="right"> </param>
        /// <returns> -1 for left smaller than right, 0 if they are equal, +1 if right is larger than left </returns>
        public static int CompareFunctionString(string left, string right) { return String.Compare(left, right, StringComparison.Ordinal); }

        /// <summary>
        ///   The add function uses non-recursive tree traversal to find the next available insertion point
        /// </summary>
        /// <param name="value"> The value to insert into tree. </param>
        public void Add(T value)
        {
            var child = new BinaryTreeNode {Data = value};

            // Is the tree empty? Make the root the new child
            if (_root == null)
            {
                _root = child;
            }
            else
            {
                // Start from the root of the tree
                var iterator = _root;
                while (true)
                {
                    // Compare the value to insert with the value in the current tree node
                    var compare = _compareFunction(value, iterator.Data);
                    // The value is smaller or equal to the current node, we need to store it on the left side
                    // We test for equivalence as we allow duplicates (!)
                    if (compare <= 0)
                        if (iterator.Left != null)
                        {
                            // Travel further left
                            iterator = iterator.Left;
                            continue;
                        }
                        else
                        {
                            // An empty left leg, add the new node on the left leg
                            iterator.Left = child;
                            child.Parent = iterator;
                            break;
                        }
                    if (compare > 0)
                        if (iterator.Right != null)
                        {
                            // Continue to travel right
                            iterator = iterator.Right;
                        }
                        else
                        {
                            // Add the child to the right leg
                            iterator.Right = child;
                            child.Parent = iterator;
                            break;
                        }
                }
            }
        }

        /// <summary>
        ///   This routine walks through the tree to see if the value given can be found.
        /// </summary>
        /// <param name="value"> The value to look for in the tree </param>
        /// <returns> True if found, False if not found </returns>
        public bool Find(T value)
        {
            var iterator = _root;
            while (iterator != null)
            {
                var compare = _compareFunction(value, iterator.Data);
                // Did we find the value ?
                if (compare == 0) return true;
                if (compare < 0)
                {
                    // Travel left
                    iterator = iterator.Left;
                    continue;
                }
                // Travel right
                iterator = iterator.Right;
            }
            return false;
        }

        /// <summary>
        ///   Given a starting node, this routine will locate the left most node in the sub-tree If no further nodes are found, it returns the starting node
        /// </summary>
        /// <param name="start"> The sub-tree starting point </param>
        /// <returns> </returns>
        private static BinaryTreeNode FindMostLeft(BinaryTreeNode start)
        {
            var node = start;
            while (true)
            {
                if (node.Left != null)
                {
                    node = node.Left;
                    continue;
                }
                break;
            }
            return node;
        }

        /// <summary>
        ///   Returns a list iterator of the elements in the tree implementing the IENumerator interface.
        /// </summary>
        /// <returns> IENumerator </returns>
        public IEnumerator<T> GetEnumerator() { return new BinaryTreeEnumerator(this); }

        #region Nested type: BinaryTreeEnumerator
        /// <summary>
        ///   The BinaryTreeEnumerator implements the IEnumerator allowing foreach enumeration of the tree
        /// </summary>
        private class BinaryTreeEnumerator : IEnumerator<T>
        {
            private readonly BinaryTree<T> _theTree;
            private BinaryTreeNode _current;

            public BinaryTreeEnumerator(BinaryTree<T> tree)
            {
                _theTree = tree;
                _current = null;
            }

            #region IEnumerator<T> Members
            /// <summary>
            ///   The MoveNext function traverses the tree in sorted order.
            /// </summary>
            /// <returns> True if we found a valid entry, False if we have reached the end </returns>
            public bool MoveNext()
            {
                // For the first entry, find the lowest valued node in the tree
                if (_current == null)
                    _current = FindMostLeft(_theTree._root);
                else
                {
                    // Can we go right-left?
                    if (_current.Right != null)
                        _current = FindMostLeft(_current.Right);
                    else
                    {
                        // Note the value we have found
                        var currentValue = _current.Data;

                        // Go up the tree until we find a value larger than the largest we have
                        // already found (or if we reach the root of the tree)
                        while (_current != null)
                        {
                            _current = _current.Parent;
                            if (_current != null)
                            {
                                var compare = _theTree._compareFunction(_current.Data, currentValue);
                                if (compare < 0) continue;
                            }
                            break;
                        }
                    }
                }
                return (_current != null);
            }

            public T Current
            {
                get
                {
                    if (_current == null)
                        throw new InvalidOperationException();
                    return _current.Data;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    if (_current == null)
                        throw new InvalidOperationException();
                    return _current.Data;
                }
            }

            public void Dispose() { }
            public void Reset() { _current = null; }
            #endregion
        }
        #endregion

        #region Nested type: BinaryTreeNode
        /// <summary>
        ///   The tree is build up out of BinaryTreeNode instances
        /// </summary>
        private class BinaryTreeNode
        {
            public T Data;
            public BinaryTreeNode Left;
            public BinaryTreeNode Parent;
            public BinaryTreeNode Right;

            public BinaryTreeNode()
            {
                Left = null;
                Right = null;
                Parent = null;
            }
        }
        #endregion
    }
}