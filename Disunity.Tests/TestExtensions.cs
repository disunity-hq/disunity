using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;


namespace Disunity.Tests {

    public static class TestExtensions {

        public static void AssertWith<TExpected, TActual>(this IEnumerable<TActual> actual, IEnumerable<TExpected> expected, Action<TExpected, TActual> inspector) {
            Assert.Collection(actual, expected.Select(e => (Action<TActual>) (a => inspector(e, a))).ToArray());
        }

        public static void AssertItemsEqual<T>(this IEnumerable<T> actual, IEnumerable<T> expected) {
            actual.AssertWith(expected, Assert.Equal);
        }

    }

}