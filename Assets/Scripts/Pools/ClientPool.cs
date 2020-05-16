using System.Collections.Generic;
using UnityEngine;

public class ClientPool : MonoBehaviour
{
    #region Parameters

    /// <summary>
    /// Animator acces
    /// </summary>
    Animator clientAnimator;

    #endregion

    #region Attributs

    /// <summary>
    /// Faces list
    /// </summary>
    public List<Sprite> clients;

    /// <summary>
    /// Actual client
    /// </summary>
    public GameObject clientFace;

    #endregion

    #region Game Méthodes

    void Awake()
    {
        clientAnimator = clientFace.GetComponent<Animator>();
    }

    void Start()
    {

    }

    #endregion

    #region Méthodes

    public void NewClient()
    {
        clientFace.transform.localPosition = new Vector3(-1000, 300, -1);

        for (int i = 0; i < clientFace.transform.childCount; i++)
        {
            Destroy(clientFace.transform.GetChild(i).gameObject);
        }

        var spritePick = clients[Random.Range(0, clients.Count)];
        GameObject go = new GameObject(spritePick.name);
        go.transform.SetParent(clientFace.transform);
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = spritePick;
        sr.sortingOrder = 1;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = new Vector3(3.5f, 3.5f, 1);
        clientAnimator.SetTrigger("ClientDeparture");
    }

    #endregion
}
