﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneEventManager : MonoBehaviour
{
    #region Parameters

    /// <summary>
    /// Dialogue GameObject
    /// </summary>
    private GameObject dialogueGO;

    /// <summary>
    /// GameMaster Acces
    /// </summary>
    private GameObject gameMaster;

    /// <summary>
    /// Dialogue container
    /// </summary>
    private GameObject dialogue;

    /// <summary>
    /// ClientPool Acces
    /// </summary>
    private GameObject clientPool;

    /// <summary>
    /// Pool Container
    /// </summary>
    private GameObject pools;

    /// <summary>
    /// Ui canvas
    /// </summary>
    private GameObject uiCanvas;

    /// <summary>
    /// GameOver screen
    /// </summary>
    private GameObject gameOverPanel;

    /// <summary>
    /// Is a dialogue is writting on screen
    /// </summary>
    private bool isWriting;
    
    /// <summary>
    /// Is the game is starting a new level
    /// </summary>
    private bool isStartingNewDemande;

    /// <summary>
    /// Door element animator acces
    /// </summary>
    private Animator doorAnimator;

    /// <summary>
    /// Timer element animator acces
    /// </summary>
    private Animator timerAnimator;

    /// <summary>
    /// Actual demande
    /// </summary>
    private int demande;

    /// <summary>
    /// Music manager acces
    /// </summary>
    private MusicManager musicManager;

    #endregion

    #region Attributs

    /// <summary>
    /// Game over screen element
    /// </summary>
    public GameObject gameOverGO;

    /// <summary>
    /// Game state
    /// </summary>
    public GameState state;

    /// <summary>
    /// Level duration
    /// </summary>
    public float countDown;

    /// <summary>
    /// Number of correct mask part
    /// </summary>
    public int totalVictoryPoint;

    #endregion

    #region Game Méthodes

    void Start()
    {
        totalVictoryPoint = 0;
        demande = 0;
        dialogueGO = GameObject.Find("Dialogue");
        gameMaster = GameObject.Find("GameMaster");
        dialogue = GameObject.Find("Dialogue");
        pools = GameObject.Find("pools");
        doorAnimator = GameObject.Find("Wall")?.GetComponent<Animator>();
        timerAnimator = GameObject.Find("Timer")?.GetComponent<Animator>();
        clientPool = GameObject.Find("clientPool");
        uiCanvas = GameObject.Find("UiCanvas");
        isWriting = dialogue.GetComponent<TextAppearance>().isWritting;
        musicManager = gameMaster.GetComponent<MusicManager>();

        NewDemande();
    }

    void Update()
    {
        if (state == GameState.Dialogue)
        {
            isWriting = dialogue.GetComponent<TextAppearance>().isWritting;
            if (!isWriting && isStartingNewDemande == false)
            {
                countDown = 25;
                StartCoroutine(startScratchSound());
                timerAnimator.SetTrigger("StartTimer");
                state = GameState.Playing;
            }
        }

        if (state == GameState.Playing)
        {
            countDown -= Time.deltaTime;

            if (countDown <= 0)
            {
                pools.GetComponent<LevelManager>().CheckVictory(new List<string>());
                NewDemande();
            }
        }
    }

    #endregion

    #region Méthodes

    /// <summary>
    /// Launch a new demande
    /// </summary>
    public void NewDemande()
    {
        if (gameOverPanel != null)
        {
            Destroy(gameOverPanel.gameObject);
            gameOverPanel = null;
        }

        isStartingNewDemande = true;
        demande += 1;

        if (demande > 5)
        {
            LaunchEndOfDay();
            demande = 0;
            timerAnimator.StopPlayback();
            state = GameState.Menu;
            return;
        }

        state = GameState.Dialogue;
        timerAnimator.StopPlayback();
        doorAnimator.SetTrigger("OpenDoor");
        musicManager.playBell();
        clientPool.GetComponent<ClientPool>().NewClient();
        StartCoroutine(StartingDemande());
    }

    /// <summary>
    /// Set up level ending after five demande
    /// </summary>
    private void LaunchEndOfDay()
    {
        gameOverGO.GetComponentInChildren<TextMeshProUGUI>().text
            = totalVictoryPoint + "/15 élément réussi. \nGain total = " + 100 * totalVictoryPoint;

        gameOverPanel = Instantiate(gameOverGO, uiCanvas.transform);

        GameObject.Find("Button").GetComponent<Button>().onClick
            .AddListener(delegate { 
                totalVictoryPoint = 0;  
                NewDemande(); 
            });
    }

    #endregion

    #region Coroutine

    /// <summary>
    /// Play scratch sound for timer
    /// </summary>
    /// <returns></returns>
    private IEnumerator startScratchSound()
    {
        musicManager.playScratchSound(0);
        yield return new WaitForSeconds(5);
        musicManager.playScratchSound(0);
        yield return new WaitForSeconds(6);
        musicManager.playScratchSound(0);
        yield return new WaitForSeconds(6);
        musicManager.playScratchSound(0);
        yield return new WaitForSeconds(8);
    }

    /// <summary>
    /// Set up new client demande
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartingDemande()
    {
        yield return new WaitForSeconds(2);
        dialogueGO.GetComponent<TextAppearance>().WriteMessage(gameMaster.GetComponent<DemandPool>().GeneratedSentence());

        isStartingNewDemande = false;
    }

    #endregion
}

public enum GameState
{
    Menu,
    Dialogue,
    Result,
    Playing
}
