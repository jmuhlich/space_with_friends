using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace space_with_friends
{
    /// <summary>
    /// This class serves to store all the defined locks
    /// </summary>
    public class Store
    {
        // I'm not sure I like that the LockSystem owns the store vs. having a global
        // one that we use, that we just all listen to (including the network layer).
        // However, I don't want to dive in on that big of a change right now. Just leaving
        // this here as a TODO:
        //public static Store singleton = new Store();

        private ConcurrentDictionary<Guid, Lock> store;

        public Store() => store = new ConcurrentDictionary<Guid, Lock>();

        public event Action<Lock> on_set;
        public event Action<Lock> on_unset;
        public event Action<Lock[]> on_clear;

        /// <summary>
        /// Get a lock given a lock id.
        /// </summary>
        public Lock get(Guid id)
        {
            store.TryGetValue(id, out var l);
            return l;
        }

        /// <summary>
        /// Find the first lock we can (no ordering is guaranteed) that satisfies the test callback.
        /// </summary>
        public Lock find(Func<Lock, bool> test)
        {
            foreach (var l in store.Values)
            {
                if (test(l))
                {
                    return l;
                }
            }

            return null;
        }

        /// <summary>
        /// Find all locks that satisfy the test callback.
        /// </summary>
        public Lock[] all(Func<Lock, bool> test)
        {
            ConcurrentQueue<Lock> q = new ConcurrentQueue<Lock>();
            foreach (var l in store.Values)
            {
                if (test(l))
                {
                    q.Enqueue(l);
                }
            }

            return q.ToArray();
        }

        /// <summary>
        /// Set a lock (if a lock with this id does not already exist)
        /// </summary>
        public Lock set(Lock l)
        {
            if ( !store.TryAdd( l.id, l) )
            {
                store.TryGetValue(l.id, out var existing);
                return existing;
            }

            on_set.Invoke(l);
            return l;
        }

        /// <summary>
        /// Unset a lock (if a lock with this id exists)
        /// </summary>
        public Lock unset(Guid id)
        {
            store.TryRemove(id, out var removed);

            if ( removed != null )
            {
                on_unset.Invoke(removed);
            }

            return removed;
        }

        /// <summary>
        /// Unset a lock (if a lock with this id exists)
        /// </summary>
        public Lock unset(Lock l) => unset(l.id);

        /// <summary>
        /// Unset all locks that satisfy the test callback.
        /// </summary>
        public bool unset(Func<Lock,bool> test, out Lock[] removed)
        {
            bool removed_at_least_one_entry = false;
            ConcurrentQueue<Lock> _removed = new ConcurrentQueue<Lock>();
            foreach (Lock l in store.Values)
            {
                bool remove = test(l);
                if ( remove )
                {
                    store.TryRemove(l.id, out var _l);
                    if (_l != null)
                    {
                        removed_at_least_one_entry = true;
                        _removed.Enqueue(_l);
                        on_unset.Invoke(_l);
                    }
                }
            }

            removed = _removed.ToArray();

            return removed_at_least_one_entry;
        }
        public bool unset( Func< Lock, bool > test )
        {
            return unset( test, out var removed );
        }

        public void to_dictionary( out Dictionary<Guid, Lock> locks )
        {
            locks = new Dictionary<Guid, Lock>(store);
        }

        public void to_array( out Lock[] locks )
        {
            locks = new Lock[store.Count];
            int index = 0;
            foreach( Lock l in store.Values )
            {
                locks[index++] = l;
            }
        }

        /// <summary>
        /// Clear all locks.
        /// </summary>
        public void clear()
        {
            Lock[] locks = new Lock[store.Count];
            store.Values.CopyTo(locks, 0);
            store.Clear();
            on_clear.Invoke(locks);
        }
    }

}
