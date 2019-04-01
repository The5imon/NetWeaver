using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWeaverServer.Main
{

    public static class CollectionUtils
    {
        public static string ToFormat<T>(this ICollection<T> collection)
        {
            string type = collection.GetType().Name;
            if (collection is List<int> || collection is ISet<int>)
                return type.Substring(0, type.Length - 2)
                    + " [" + string.Join(", ", collection) + "]";
            else
                return type.Substring(0, type.Length - 2)
                    + " [\"" + string.Join("\", \"", collection) + "\"]";
        }

        public static string ToFormat<T>(this T[] array)
        {
            if (array is int[])
                return "[" + string.Join(", ", array) + "]";
            else
                return "[\"" + string.Join("\", \"", array) + "\"]";
        }
    }
}
