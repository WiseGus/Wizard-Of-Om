using SQLtoOM.Engine.Models;
using System.Collections.Generic;
using System.Collections;

namespace SQLtoOM.Engine {

    internal class SqlParameters : IList<gsParameter> {

        private static int _ID = 0;

        private static SqlParameters _Instance = new SqlParameters();
        public static SqlParameters Instance
        {
            get
            {
                _ID = 0;
                return _Instance;
            }
        }

        private List<gsParameter> _Parameters = new List<gsParameter>();

        public int Count { get { return _Parameters.Count; } }

        public bool IsReadOnly { get { return false; } }

        public gsParameter this[int index]
        {
            get { return _Parameters[index]; }
            set { _Parameters[index] = value; }
        }

        private SqlParameters() {
        }

        public int IndexOf(gsParameter item) {
            return _Parameters.IndexOf(item);
        }

        public void Insert(int index, gsParameter item) {
            _Parameters.Insert(index, item);
        }

        public void RemoveAt(int index) {
            _Parameters.RemoveAt(index);
        }

        public void Add(gsParameter item) {
            if (_Parameters.Find(p => p.Name == item.Name) != null) return;

            _Parameters.Add(item);
        }

        public void Clear() {
            _Parameters.Clear();
        }

        public bool Contains(gsParameter item) {
            return _Parameters.Contains(item);
        }

        public void CopyTo(gsParameter[] array, int arrayIndex) {
            _Parameters.CopyTo(array, arrayIndex);
        }

        public bool Remove(gsParameter item) {
            return _Parameters.Remove(item);
        }

        public IEnumerator<gsParameter> GetEnumerator() {
            return _Parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        internal static int NewID() {
            return ++_ID;
        }
    }
}
