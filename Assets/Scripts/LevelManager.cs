using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    #region Parameters

    /// <summary>
    /// Game master accès
    /// </summary>
    GameObject gameMaster;

    /// <summary>
    /// Game manager accès
    /// </summary>
    GameObject gameManager;

    /// <summary>
    /// List of the winning element selected by the player
    /// </summary>
    List<string> winningPart;

    /// <summary>
    /// Number of correct element selected by the player
    /// </summary>
    int victoryPoint;

    #endregion

    #region Attributs
    
    /// <summary>
    /// UI canvas accès
    /// </summary>
    public GameObject canvas;

    /// <summary>
    /// Prefabs : notification for the selected masque or time out
    /// </summary>
    public GameObject playerNotification;

    #endregion

    #region Game Méthodes

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        gameMaster = GameObject.Find("GameMaster");
        winningPart = new List<string>();
    }

    #endregion

    #region Méthodes

    /// <summary>
    /// Do the comparaison between selected element and winning part
    /// also launch player notification for the result
    /// </summary>
    /// <param name="maskParts">Mask parts selected by the player</param>
    /// <returns>true if at least one part is good</returns>
    public bool CheckVictory(List<string> maskParts)
    {
        winningPart.Clear();
        victoryPoint = 0;
        var victoriousPartList = gameMaster.GetComponent<DemandPool>().victoriousPick;


        foreach (var item in maskParts)
        {
            Debug.Log(item);

            if (victoriousPartList.Any(p => p.Equals(item)))
            {
                victoryPoint++;
                winningPart.Add(item);
            }
        }

        StartCoroutine(SuccesAlert());
        
        if (victoryPoint > 0)
        {
            gameManager.GetComponent<SceneEventManager>().NewDemande();
            return true;
        }

        return false;
    }

    #endregion

    #region Coroutine

    /// <summary>
    /// Notification coroutine
    /// instanciate playerNotification element
    /// </summary>
    /// <returns></returns>
    IEnumerator SuccesAlert()
    {
        gameManager.GetComponent<SceneEventManager>().totalVictoryPoint += winningPart.Count;

        if (!winningPart.Any())
        {
            if (gameManager.GetComponent<SceneEventManager>().countDown > 0)
            {
                playerNotification.GetComponent<VictoryPointBehaviours>().text = "Mauvais masque";
            }
            else
            {
                playerNotification.GetComponent<VictoryPointBehaviours>().text = "Time out !";
            }
            Debug.Log(playerNotification.GetComponent<VictoryPointBehaviours>().text);
            playerNotification.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(250, 0, 0, 250);
            Instantiate(playerNotification, canvas.transform);
            yield return new WaitForSeconds(1);
        }
        else
        {
            playerNotification.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(0, 250, 0, 250);
            foreach (var item in winningPart)
            {
                playerNotification.GetComponent<VictoryPointBehaviours>().text = item + " +1";
                
                Instantiate(playerNotification, canvas.transform);
                yield return new WaitForSeconds(1);
            }
        } 
    }

    #endregion
}
