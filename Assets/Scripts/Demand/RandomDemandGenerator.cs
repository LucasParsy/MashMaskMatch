using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

public class RandomDemandGenerator : MonoBehaviour
{
    #region "methods"

    private static string getPart(List<Sprite> partPool)
    {
        string name =  partPool[Random.Range(0, partPool.Count)].name;
        return name.Substring(name.IndexOf("_") + 1);
    }

    /// <summary>
    /// Generate a random mask from a given pool.
    /// </summary>
    /// <param name="existingPool">the pool to take parts from</param>
    /// <param name="numForms">the number of components of the mask.false between 1 and 3</param>
    /// <returns>A mask with 1 to 3 components</returns>
    public static MaskComponents GenerateMask(MaskCompletePool existingPool, int numForms)
    {
        Assert.IsTrue(numForms >= 1 && numForms <= 3);
        List<string> components = new List<string>();
        components.Add(getPart(existingPool.eyes));
        components.Add(getPart(existingPool.face));
        components.Add(getPart(existingPool.mouth));

        //remove random components
        List<int> indexes = new List<int>() { 0, 1, 2 };
        ListUtils.Shuffle(indexes);
        IEnumerable<int> filteredIndexes = indexes.Take(3 - numForms);
        foreach (var index in filteredIndexes)
            components[index] = "";

        MaskComponents mask = new MaskComponents(components[0], components[1], components[2]);
        return mask;
    }
    #endregion
}
