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

    public string GeneratedSentence(int clientNumber)
    {

        MaskPartContainer container = DebugUtils.GetGameMaster().GetComponent<MaskPartContainer>();
        MaskCompletePool pool = new MaskCompletePool(
        MaskPartContainer.GetUnlockedMasks(container.EyesParts),
        MaskPartContainer.GetUnlockedMasks(container.ShapeParts),
        MaskPartContainer.GetUnlockedMasks(container.MouthParts));

        victoriousPick.Clear();

        int numComponents = DifficultyScale(clientNumber);


        MaskComponents mask = RandomDemandGenerator.GenerateMask(pool, numComponents);
        //add non-empty parts to the victory list
        foreach (var elem in mask.toList().Where(n => n != ""))
                victoriousPick.Add(elem);
        return RandomDemandDialogGen.GenerateSentence(mask, numComponents);
    }

    public int DifficultyScale(int clientNumber)
    {
        int numComponents = 1;
        int playerExperiences = SaveManager.Instance.state.completedDays;

        if(playerExperiences <= 10)
        {
            if (clientNumber >= 4)
                numComponents = 2;
        }
        else if(playerExperiences > 10 && playerExperiences <= 30)
        {
            numComponents = 2;

            if (clientNumber > 3)
                numComponents = 3;
        }
        else if(playerExperiences > 30)
        {
            numComponents = 3;
        }
        

        return numComponents;
    }
    #endregion
}
