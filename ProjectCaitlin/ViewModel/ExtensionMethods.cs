using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectCaitlin.ViewModel
{
    public static class ExtensionMethods
    {
        //public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items,
        //                                               int maxItems)
        //{
        //    return items.Select((item, inx) => (item, inx))
        //                .GroupBy(x => x.inx / maxItems)
        //                .Select(g => g.Select(x => x.item));
        //}

        public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(
                  this IEnumerable<TSource> source, int size)
        {
            TSource[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                    bucket = new TSource[size];

                bucket[count++] = item;
                if (count != size)
                    continue;

                yield return bucket;

                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
                yield return bucket.Take(count).ToArray();
        }
    }
}
