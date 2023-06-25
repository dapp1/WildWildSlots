using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Combinations : MonoBehaviour
{
    List<Combination> combinationsForFreeSpin = new List<Combination>()
    {
    new Combination(new List<string> { "Cheese", "Apple", "Pineapple" }),
    new Combination(new List<string> { "Apple", "Cheese", "Pineapple" }),
    new Combination(new List<string> { "Banana", "Apple", "Cheese"}), 
    new Combination(new List<string> { "Raspberries", "Banana", "Apple"}),
    new Combination(new List<string> { "Banana", "Raspberries", "Pineapple"})
    };

    public bool IsCombinationMatched(List<string> objectNames)
    {
        foreach (Combination combination in combinationsForFreeSpin)
        {
            if (Enumerable.SequenceEqual(combination.ObjectNames, objectNames))
            {
                return true;
            }
        }

        return false;
    }
}

public class Combination
{
    public List<string> ObjectNames { get; }

    public Combination(List<string> objectNames)
    {
        ObjectNames = objectNames;
    }
}

