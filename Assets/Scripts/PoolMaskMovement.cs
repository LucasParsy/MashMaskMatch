using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PoolMaskMovement : MonoBehaviour
{

    #region Parameters

    /// <summary>
    /// Is the game state is at playing
    /// </summary>
    private bool isPlaying;

    /// <summary>
    /// Is the player click on send mask btn
    /// </summary>
    private bool IsClicked;

    /// <summary>
    /// Different mask part selected
    /// </summary>
    private List<MaskPool> poolPart = new List<MaskPool>();

    /// <summary>
    /// Music manager acces
    /// </summary>
    private MusicManager musicManager;

    /// <summary>
    /// Game manager acces
    /// </summary>
    private GameObject gameManager;

    #endregion

    #region Attributs

    [Range(0.2f, 1.1f)]
    public float maskDistance = 0.8f;

    [Range(1f, 10f)]
    public float scale = 5;

    [Range(0.1f, 10f)]
    public float transitionSpeed = 1f;

    public AnimationCurve transitionCurve = null;

    #endregion

    #region Game méthodes

    private void Start()
    {
        poolPart.Add(transform.Find("eyesPool").GetComponent<MaskPool>());
        poolPart.Add(transform.Find("mouthPool").GetComponent<MaskPool>());
        poolPart.Add(transform.Find("maskPool").GetComponent<MaskPool>());

        gameManager = GameObject.Find("GameManager");
        musicManager = GameObject.Find("GameMaster").GetComponent<MusicManager>();

        isPlaying = gameManager.GetComponent<SceneEventManager>().state == GameState.Playing;
    }

    #endregion

    #region Méthodes

    /// <summary>
    /// Give mask to the client
    /// </summary>
    public void giveMask()
    {
        isPlaying = gameManager.GetComponent<SceneEventManager>().state == GameState.Playing;
        if (isPlaying && IsClicked == false)
        {
            IsClicked = true;
            StartCoroutine(giveMaskCorout());
        }
    }

    #endregion

    #region Coroutine
    
    /// <summary>
    /// Send mask to the client
    /// </summary>
    /// <returns></returns>
    public IEnumerator giveMaskCorout()
    {
        GameObject go = new GameObject("givenMask");
        var givenMask = new List<string>();

        foreach (MaskPool mp in poolPart)
        {
            GameObject part = Instantiate(mp.getCurrentMask());
            part.transform.SetParent(go.transform);
            part.transform.position = go.transform.position;

            string title = part.name;
            Debug.Log("gave mask part " + title);
            var splitName = part.name.Split('_');
            var pos = splitName[1].IndexOf("(Clone)");
            if (pos > 0)
                givenMask.Add(splitName[1].Remove(pos));
        }

        go.transform.Translate(new Vector3(0, 0, 1));
        go.transform.localScale = new Vector3(0.7f, 0.7f, 1);
        go.transform.Rotate(70, 0, 0);

        musicManager.playSendMask();
        yield return DOTween.Sequence()
        .Append(go.transform.DOMove(new Vector3(0, 4, 4), transitionSpeed * 2))
        .Insert(0, go.transform.DORotate(new Vector3(0, 0, 0), transitionSpeed * 2))
        .Append(go.transform.DOScale(new Vector3(0, 0, 0), transitionSpeed))
        .SetEase(Ease.OutCirc).WaitForCompletion();
        Destroy(go);

        GetComponent<LevelManager>().CheckVictory(givenMask);
        IsClicked = false;
    }

    #endregion
}
