using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DemandPool : MonoBehaviour
{
    #region Attributs

    /// <summary>
    /// List of the victorious descriptions
    /// </summary>
    public List<string> victoriousPick = new List<string>();

    #endregion

    #region Game méthodes

    #endregion

    #region Méthodes

    public string GeneratedSentence()
    {

        MaskPartContainer container = DebugUtils.getGameMaster().GetComponent<MaskPartContainer>();
        MaskCompletePool pool = new MaskCompletePool(
        MaskPartContainer.getUnlockedMasks(container.EyesParts),
        MaskPartContainer.getUnlockedMasks(container.ShapeParts),
        MaskPartContainer.getUnlockedMasks(container.MouthParts));

        victoriousPick.Clear();
        int numComponents = Random.Range(1, 3);
        MaskComponents mask = RandomDemandGenerator.GenerateMask(pool, numComponents);
        //add non-empty parts to the victory list
        foreach (var elem in mask.toList().Where(n => n != ""))
                victoriousPick.Add(elem);
        return RandomDemandDialogGen.GenerateSentence(mask, numComponents);
    }
    #endregion
}
