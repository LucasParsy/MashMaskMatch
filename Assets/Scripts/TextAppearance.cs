using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAppearance : MonoBehaviour
{
    #region Parameters

    /// <summary>
    /// Text mesh pro acces
    /// </summary>
    TextMeshProUGUI guiText;

    /// <summary>
    /// Message to show
    /// </summary>
    string messageStorage;

    #endregion

    #region Attributs

    /// <summary>
    /// Pause between lettes appearances
    /// </summary>
    [Range(0, 0.1f)]
    public float pause = 0.05f;

    /// <summary>
    /// Is all the text is written
    /// </summary>
    public bool isWritting;

    #endregion

    #region Game Méthodes

    void Start()
    {
        guiText = GetComponent<TextMeshProUGUI>();
        isWritting = false;
        guiText.text = string.Empty;
    }

    #endregion

    #region Méthodes

    /// <summary>
    /// Starting methode for slow text apprearance
    /// </summary>
    /// <param name="message">message to show</param>
    public void WriteMessage(string message)
    {
        isWritting = true;
        messageStorage = message;
        guiText.text = string.Empty;
        StartCoroutine(TypeLetters());
    }

    #endregion

    #region Coroutine

    /// <summary>
    /// Type letter slowly
    /// </summary>
    /// <returns></returns>
    IEnumerator TypeLetters()
    {
        var charArray = messageStorage.ToCharArray();

        if (charArray.Length > 200)
            messageStorage = "Je voudrait bdflkjsdfjg dsfg gfdg ...Je ne sais plus ce que je dit !";
        charArray = messageStorage.ToCharArray();

        foreach (char letter in charArray)
        {
            guiText.text += letter;
            yield return new WaitForSeconds(pause);
        }
        isWritting = false;
    }

    #endregion
}
