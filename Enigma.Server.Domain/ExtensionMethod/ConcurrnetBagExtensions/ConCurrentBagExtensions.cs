using System;
using System.Collections.Concurrent;

namespace Enigma.Server.Domain.ConcurrnetBagExtensions
{
    public static class ConcurrentBagExtensions
    {
        public static void ClearAndDoAction<T>(this ConcurrentBag<T> bag, Action<T> action)
        {
            using (var enumerator = bag.GetEnumerator())
            {
                // This looks weird but get enumerator gets us a snapshot of the current state. So we need to immediately clear it as we start iterating
                // As we don't know if another thread will suddenly interact with us.
                bag.Clear();
                while (enumerator.MoveNext())
                {
                    action(enumerator.Current);
                }
            }
        }
    }
}
