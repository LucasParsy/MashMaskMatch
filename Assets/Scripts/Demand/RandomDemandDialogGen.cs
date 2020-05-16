using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat;

public class RandomDemandDialogGen : MonoBehaviour
{
    #region Parameters

    /// <summary>
    /// List of the sentence's first words
    /// </summary>

    private List<string> intros = new List<string>();
    #endregion

    #region Methods

    //retreive all the intros localized strings
    private IEnumerator getLocalizedStrings()
    {
        var d = LocalizationSettings.StringDatabase.GetTableAsync(LocalizationSettings.StringDatabase.DefaultTable);
        if (!d.IsDone)
            yield return d;
        var stringEntryList = d.Result.ToList();

        foreach (var entry in d.Result.Values)
            intros.Add(entry.GetLocalizedString());
        Debug.Log("finished query localized str at:" + Time.fixedTime);
    }


    public void Awake()
    {
        StartCoroutine(getLocalizedStrings());
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



        var parts = new List<string>() { "des yeux ronds", "une bouche verte", "une forme carrée" };
        var testSeparators = Smart.Format("Bjr: {Messages:|, | et } s'il vous plait",  new object[] {new {Messages = parts}});

        Debug.Log("intro str asked at: " + Time.fixedTime);
        string intro = intros[Random.Range(0, intros.Count)];
    dialogue.AppendFormat(intro, question);
        return dialogue.ToString();
    }
    #endregion
}
