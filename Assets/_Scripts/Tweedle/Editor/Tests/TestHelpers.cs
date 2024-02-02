using System;
using System.Collections;

public static class TestHelpers {
    public static void WaitOnEnumeratorTree(IEnumerator enumerator) {
        while (enumerator.MoveNext()) {
            try {
                var val = enumerator.Current;
                if (val is IEnumerator val1) {
                    WaitOnEnumeratorTree(val1);
                }
            }
            catch (NullReferenceException nre) {
                // Ignore errors when reading partial test structures
                Console.WriteLine(nre);
            }
        }
    }
}