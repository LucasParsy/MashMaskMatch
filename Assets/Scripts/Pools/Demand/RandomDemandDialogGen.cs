using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RandomDemandDialogGen : MonoBehaviour
{
    #region Parameters

    /// <summary>
    /// List of the sentence's first words
    /// </summary>
    private static readonly string[] introList = {
            "Bonjour, je voudrais un masque avec {0}",
            "Auriez-vous un masque avec {0}?",
            "J'ai besoin d'un masque avec {0}",
            "J'aimerai bien un masque avec {0}, ça serait cool",
            "Un masque qui aurait {0}. vous auriez ça, ce serait génial.",
            "Combien pour un masque contenant {0}?",
            "{0}. Mon masque idéal",
            "Vous vendez un masque avec {0}?"
        };

    #endregion

    #region Methods

    /// to do "I want A,B and C" instead of "I want A B C"
    /// <summary>
    /// gives the prefix to give depending on the place in the sentence
    /// </summary>
    /// <returns>separator </returns>
    private static string getElemSeparator(int numComponents, int currentPart)
    {
        if (currentPart == 0)
            return "";
        if (currentPart == numComponents - 1)
            return " et ";
        else
            return ",";

    }

    private static void addLine(StringBuilder dialog, string prefix, string desc,
                                int numComponents, ref int currentPart)
    {
        if (desc != "")
        {
            dialog.Append(getElemSeparator(numComponents, currentPart));
            dialog.Append(prefix);
            dialog.Append(desc);
            currentPart++;
        }
    }

    public static string GenerateSentence(MaskComponents mask, int numComponents)
    {
        int currentPart = 0;
        StringBuilder dialogue = new StringBuilder();
        var intro = introList[Random.Range(0, introList.Length)];
        StringBuilder question = new StringBuilder();
        addLine(question, "des yeux ", mask.eyes, numComponents, ref currentPart);
        addLine(question, "une bouche ", mask.mouth, numComponents, ref currentPart);
        addLine(question, "une forme ", mask.face, numComponents, ref currentPart);
        dialogue.AppendFormat(intro, question);
        return dialogue.ToString();
    }
    #endregion
}
