namespace Sokoban.Domain.Helpers
{
    /**
     * This class is ported from the Stanford Algorithms library.
     * 
     *  The <tt>UF</tt> class represents a union-find data data structure.
     *  It supports the <em>union</em> and <em>find</em>
     *  operations, along with a method for determining the number of
     *  disjoint sets.
     *  <p>
     *  This implementation uses weighted quick union.
     *  Creating a data structure with N objects takes linear time.
     *  Afterwards, all operations are logarithmic worst-case time.
     *  <p>
     *  For additional documentation, see <a href="http://algs4.cs.princeton.edu/15uf">Section 1.5</a> of
     *  <i>Algorithms, 4th Edition</i> by Robert Sedgewick and Kevin Wayne.
     */
    class UnionFind
    {
        private readonly int[] _id;    // id[i] = parent of i
        private readonly int[] _sz;    // sz[i] = number of objects in subtree rooted at i
        public int Count { get; private set; }

        /**
          * Create an empty union find data structure with N isolated sets.
          */
        public UnionFind(int n)
        {
            Count = n;
            _id = new int[n];
            _sz = new int[n];
            for (var i = 0; i < n; i++)
            {
                _id[i] = i;
                _sz[i] = 1;
            }
        }

        /**
          * Return the id of component corresponding to object p.
          */
        public int Find(int p)
        {
            while (p != _id[p])
                p = _id[p];
            return p;
        }


        /**
          * Are objects p and q in the same set?
          */
        public bool IsConnected(int p, int q)
        {
            return Find(p) == Find(q);
        }


        /**
          * Replace sets containing p and q with their union.
          */
        public void Union(int p, int q)
        {
            var i = Find(p);
            var j = Find(q);
            if (i == j) return;

            // make smaller root point to larger one
            if (_sz[i] < _sz[j]) { _id[i] = j; _sz[j] += _sz[i]; }
            else { _id[j] = i; _sz[i] += _sz[j]; }
            Count--;
        }
    }
}
