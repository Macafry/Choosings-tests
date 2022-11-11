using System.Collections.Generic;
using System.Linq;
/*
 This function takes a homogeneous collection of sub-collections (or enumerable of sub-enumerables) .
 It returns the cartesian product between the sub-collections as an enumerable.
 In other words, it comes up with every possible choosing combination.
 */
static IEnumerable<IEnumerable<T>> CartesianProduct<T>
            (IEnumerable<IEnumerable<T>> MetaContainer)
{
    // Getting the length of the collection of collections 
    MetaContainer.TryGetNonEnumeratedCount(out int ContainersAmount);
    // Last Index - used later
    int UpdateIndex = ContainersAmount - 1;

    // An array with all of the sub-containers' enumerators
    var Enumerators = (from grouping in MetaContainer 
                        select grouping.GetEnumerator())
                        .ToArray();
    // Initialize each enumerator
    Array.ForEach(Enumerators, 
                    Enumerator => Enumerator.MoveNext());
    
    // Main loop
    while (UpdateIndex > -1) 
    {             
        yield return from Enumerator in Enumerators 
                        select Enumerator.Current;

        // Resetting the update index 
        UpdateIndex = ContainersAmount - 1;

        // Goes over every finished enumerator, resets it and initializes it
        // The second boolean expression inside the while also moves the enumerator forward

        while (UpdateIndex > -1 && 
                !Enumerators[UpdateIndex].MoveNext()) 
        { 
            // Reset an re-initialize finished enumerator
            Enumerators[UpdateIndex].Reset();
            Enumerators[UpdateIndex--].MoveNext();
        } 
    }
}


List<int[]> test = new();
test.Add(new int[] { 1, 2, 3, 4 });
test.Add(new int[] { 2, 4, 6, 8 });
test.Add(new int[] { 12, 3, 4 });

foreach (var v in CartesianProduct<int>(test)) {
    Console.WriteLine("[{0}]", string.Join(", ", v));
}
