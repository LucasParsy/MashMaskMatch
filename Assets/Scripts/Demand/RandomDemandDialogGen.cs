using System.Collections;
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

    public UnityEngine.Localization.Settings.LocalizedStringDatabase db;

    public List<string> intros;
    #endregion

    #region Methods

    //retreive all the intros localized strings
    public IEnumerator Start()
    {
        var d = db.GetTableAsync(db.DefaultTable);
        yield return d;
        var stringEntryList = d.Result.ToList();

        for (int i = 0; i < stringEntryList.Count; i++)
        {
            var loc = stringEntryList[i].Value.GetLocalizedString();
            intros.Add(loc);
        }
    }

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

    public string GenerateSentence(MaskComponents mask, int numComponents)
    {
        int currentPart = 0;
        StringBuilder dialogue = new StringBuilder();
        StringBuilder question = new StringBuilder();
        addLine(question, "des yeux ", mask.eyes, numComponents, ref currentPart);
        addLine(question, "une bouche ", mask.mouth, numComponents, ref currentPart);
        addLine(question, "une forme ", mask.face, numComponents, ref currentPart);
        string intro = intros[Random.Range(0, intros.Count)];
        dialogue.AppendFormat(intro, question);
        return dialogue.ToString();
    }
    #endregion
}
