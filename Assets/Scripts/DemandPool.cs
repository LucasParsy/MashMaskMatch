using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DemandPool : MonoBehaviour
{
    #region Parametters

    /// <summary>
    /// List of the sentence's first words
    /// </summary>
    Dictionary<string, bool> introList;

    /// <summary>
    /// List of the eyes's descriptions
    /// </summary>
    List<string> eyesList;

    /// <summary>
    /// List of the mouth's descriptions
    /// </summary>
    List<string> mouthList;

    /// <summary>
    /// List of the shape's descriptions
    /// </summary>
    List<string> shapeList;

    #endregion

    #region Attributs

    /// <summary>
    /// List of the victorious descriptions
    /// </summary>
    public List<string> victoriousPick;

    #endregion

    #region Game méthodes

    void Start()
    {
        DontDestroyOnLoad(this);

        //List Instanciation
        introList = new Dictionary<string, bool>()
        {
            { "Bonjour, je voudrais ", false },
            { "Auriez-vous ", true },
            { "J'ai besoin d'", false },
            { "Vous vendez ", true }
        };

        eyesList = new List<string>()
        {
            "perçants",
            "impassibles",
            "vides",
            "transcendants",
            "allongés",
            "ronds",
            "dégoulinants",
            "hypnotisants",
            "globuleux",
            "heureux",
            "désolés",
            "agressifs",
            "provocateur",
            "tendus"
        };

        mouthList = new List<string>()
        {
            "effrayante",
            "pulpeuse",
            "coquine",
            "souriante",
            "discrète",
            "squelettique",
            "étroite",
            "charnue",
            "ronde",
            "grimaçante",
            "petite",
            "protégée",
            "moustachue",
            "bougonne"
        };

        shapeList = new List<string>()
        {
            "mysterieuse",
            "tribale",
            "audacieuse",
            "enfantine",
            "étrange",
            "humanoïde",
            "humanoïde",
            "féline",
            "allongée",
            "sculptée",
            "anonyme",
            "bestiale",
            "cornue",
            "pointue"
        };

        victoriousPick = new List<string>();
    }

    #endregion

    #region Méthodes

    /// <summary>
    /// Méthode for the dialogue generation
    /// </summary>
    /// <returns>dialogue sentence</returns>
    public string GeneratedSentence()
    {
        victoriousPick.Clear();

        StringBuilder dialogue = new StringBuilder();
        var intro = DictionaryPick(Random.Range(0, introList.Count));
        dialogue.Append(intro);

        var eyesPick = eyesList[Random.Range(0, eyesList.Count)];
        dialogue.Append("un masque avec des yeux " + eyesPick);
        victoriousPick.Add(eyesPick);

        var mouthPick = mouthList[Random.Range(0, mouthList.Count)];
        dialogue.Append(", une bouche " + mouthPick);
        victoriousPick.Add(mouthPick);

        var shapePick = shapeList[Random.Range(0, shapeList.Count)];
        dialogue.Append(" et une forme " + shapePick);
        victoriousPick.Add(shapePick);

        dialogue.Append(introList[intro] ? "?" : ".");
        return dialogue.ToString();
    }

    /// <summary>
    /// Méthode use to extract data from the dictionnary
    /// </summary>
    /// <param name="pick"></param>
    /// <returns></returns>
    private string DictionaryPick(int pick)
    {
        string result = string.Empty;
        var keys = introList.Keys.ToList();
        result = keys[pick];
        return result;
    }

    #endregion
}
