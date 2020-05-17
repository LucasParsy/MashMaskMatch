using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneEventManager : MonoBehaviour
{
    #region Parameters

    /// <summary>
    /// GameMaster Acces
    /// </summary>
    private GameObject gameMaster;

    /// <summary>
    /// GO Dialogue container
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
    /// Bell image container acces
    /// </summary>
    private GameObject bell;

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
    /// Sprite Used for the bell animation
    /// </summary>
    public Sprite bellIdle;

    /// <summary>
    /// Sprite used for the bell animation
    /// </summary>
    public Sprite bellRing;

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

    /// <summary>
    /// time to wait before showing the text, while the client enter animation plays.
    /// should be 2, but ca  be 0 for debug
    /// </summary>
    [SerializeField]
    public float timeWaitClientEnter = 2;


    /// <summary>
    /// Set up debug mode
    /// </summary>
    public bool isDebug;

    #endregion

    #region Game Méthodes

    void Awake()
    {
        gameMaster = DebugUtils.GetGameMaster();
        pools = GameObject.Find("pools");
        dialogue = GameObject.Find("Dialogue");
        doorAnimator = GameObject.Find("Wall")?.GetComponent<Animator>();
        timerAnimator = GameObject.Find("Timer")?.GetComponent<Animator>();
        clientPool = GameObject.Find("clientPool");
        uiCanvas = GameObject.Find("UiCanvas");
        bell = GameObject.Find("Bell");
        isWriting = dialogue.GetComponent<TextAppearance>().isWritting;
        musicManager = gameMaster.GetComponent<MusicManager>();
    }

    void Start()
    {
        totalVictoryPoint = 0;
    }

    void Update()
    {
        if (isDebug)
        {
            isDebug = false;
            LaunchStartOfDay();
        }
            

        if (state == GameState.Dialogue)
        {
            isWriting = dialogue.GetComponent<TextAppearance>().isWritting;
            if (!isWriting && isStartingNewDemande == false)
            {
                countDown = 25;
                StartCoroutine(startScratchSound());
                timerAnimator.Play("Timer", -1, 0);
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
        timerAnimator.Rebind();
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
            
            state = GameState.Menu;
            return;
        }

        state = GameState.Dialogue;
        doorAnimator.SetTrigger("OpenDoor");
        StartCoroutine(RingTheBell());
        clientPool.GetComponent<ClientPool>().NewClient();
        StartCoroutine(StartingDemande());
    }

    /// <summary>
    /// Set up level ending after five demande
    /// </summary>
    private void LaunchEndOfDay()
    {
        var gain = 10 * totalVictoryPoint;

        StringBuilder builder = new StringBuilder();
        builder.Append(totalVictoryPoint);
        builder.Append("/");
        builder.Append(GetMaxVictoryPoint());
        builder.Append(" élément réussi. \nGain total = ");
        builder.Append(gain);

        gameOverGO.GetComponentInChildren<TextMeshProUGUI>().text
            = builder.ToString();

        gameOverPanel = Instantiate(gameOverGO, uiCanvas.transform);

        GameObject.Find("RestartBtn").GetComponent<Button>().onClick.AddListener(delegate 
        {
            totalVictoryPoint = 0;
            NewDemande(); 
        });

        GameObject.Find("MenuBtn").GetComponent<Button>().onClick.AddListener(delegate
        {
            gameMaster.GetComponent<SceneMaster>().LaunchStartUpScene();
        });

        SaveManager.Instance.state.gold += gain;
        SaveManager.Instance.state.completedDays += 1;
        SaveManager.Instance.Save();
    }

    private int GetMaxVictoryPoint()
    {
        int max = 7;
        int playerExperiences = SaveManager.Instance.state.completedDays;

        if (playerExperiences > 10 && playerExperiences <= 30)
        {
            max = 22;
        }
        else if (playerExperiences > 30)
        {
            max = 15;
        }

        return max;
    }

    /// <summary>
    /// Set up level start fire on sceneLoaded
    /// </summary>
    public void LaunchStartOfDay()
    {
        pools.GetComponent<LevelManager>().SetUpMasks();
        NewDemande();
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
        musicManager.playScratchSound(1);
        yield return new WaitForSeconds(5);
        musicManager.playScratchSound(2);
        yield return new WaitForSeconds(5);
        musicManager.playScratchSound(3);
        yield return new WaitForSeconds(5);
    }

    /// <summary>
    /// Set up new client demande
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartingDemande()
    {
        yield return new WaitForSeconds(timeWaitClientEnter);
        dialogue.GetComponent<TextAppearance>().WriteMessage(gameMaster.GetComponent<DemandPool>().GeneratedSentence(demande));

        isStartingNewDemande = false;
    }

    /// <summary>
    /// Animate the bell and play the sound
    /// </summary>
    /// <returns></returns>
    private IEnumerator RingTheBell()
    {
        bell.GetComponent<Image>().sprite = bellRing;
        musicManager.playBell();
        yield return new WaitForSeconds(0.5f);
        bell.GetComponent<Image>().sprite = bellIdle;
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
