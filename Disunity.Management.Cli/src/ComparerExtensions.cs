using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;


namespace Disunity.Management.Cli {

    public static class ComparerExtensions {

        public static T Max<T>(this Comparer<T> comparer, T x, T y) {
            return comparer.Compare(x, y) > 0 ? x : y;
        }

        public static T Min<T>(this Comparer<T> comparer, T x, T y) {
            return comparer.Compare(x, y) < 0 ? x : y;
        }

    }

}