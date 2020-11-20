using System;
using System.Collections.Generic;
using System.Text;

namespace Trabalho_LFA.Extensions
{
    public static class Extensions
    {
        /*
         Source: https://stackoverflow.com/questions/489258/linqs-distinct-on-a-particular-property
         Motivo: Distinct by field a partir de lista de objeto
             */
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
