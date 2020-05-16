using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{

    #region Pamareters

    /// <summary>
    /// music fading intensity
    /// </summary>
    [SerializeField]
    [Range(0, 2)]
    private float fadeMultiplier = 0.3f;

    /// <summary>
    /// Introduction musics
    /// </summary>
    [SerializeField]
    private AudioClip[] introMus = null;

    /// <summary>
    /// Game musics
    /// </summary>
    [SerializeField]
    private AudioClip[] gameMus = null;

    /// <summary>
    /// Menu musics
    /// </summary>
    [SerializeField]
    private AudioClip[] menuMus = null;

    /// <summary>
    /// ? musics
    /// </summary>
    [SerializeField]
    private AudioClip[] overMus = null;

    /// <summary>
    /// Table scratch sound effect
    /// </summary>
    [SerializeField]
    private AudioClip[] scratchSound = null;

    /// <summary>
    /// Bell sound effect
    /// </summary>
    [SerializeField]
    private AudioClip bell = null;

    /// <summary>
    /// Mask swipping sound effect
    /// </summary>
    [SerializeField]
    private AudioClip swapMask = null;

    /// <summary>
    /// Current music playing
    /// </summary>
    private AudioClip currentMusic = null;

    /// <summary>
    /// ?
    /// </summary>
    private AudioSource musicSource = null;
    
    /// <summary>
    /// ?
    /// </summary>
    private AudioSource effectSource = null;

    #endregion

    #region Game Méthodes

    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        effectSource = gameObject.AddComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(this);
        playBgmCorout(introMus);
    }

    #endregion

    #region Méthodes

    /// <summary>
    /// Start bell sound effect
    /// </summary>
    public void playBell()
    {
        effectSource.clip = bell;
        effectSource.Play();
    }

    /// <summary>
    /// Start mask sound effect
    /// </summary>
    public void playSendMask()
    {
        effectSource.clip = swapMask;
        effectSource.Play();
    }

    /// <summary>
    /// Start table scratch sound effect
    /// </summary>
    /// <param name="num">scratch index</param>
    public void playScratchSound(int num)
    {
        effectSource.clip = scratchSound[num];
        effectSource.Play();
    }

    /// <summary>
    /// Start background music
    /// </summary>
    /// <param name="musList"></param>
    private void playBgmCorout(AudioClip[] musList)
    {
        StartCoroutine(playBackgroundMusic(musList));
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Music fading
    /// </summary>
    /// <param name="isFadeIn"></param>
    /// <returns></returns>
    private IEnumerator fadePlayer(bool isFadeIn)
    {
        musicSource.volume = (isFadeIn ? 0 : 1);
        while (isFadeIn ? (musicSource.volume < 0.99) : (musicSource.volume > 0.01))
        {
            musicSource.volume += Time.deltaTime * fadeMultiplier * (isFadeIn ? 1 : -1);
            yield return null;
        }
    }

    /// <summary>
    /// Background music 
    /// </summary>
    /// <param name="musList"></param>
    /// <returns></returns>
    private IEnumerator playBackgroundMusic(AudioClip[] musList)
    {
        int numClip = Random.Range(0, musList.Length - 1);
        AudioClip clip = musList[numClip];

        if (currentMusic != null)
        {
            yield return fadePlayer(false);
            musicSource.Stop();
        }
        musicSource.clip = clip;
        musicSource.Play();
        yield return fadePlayer(true);
        currentMusic = clip;
        yield return null;
    }

    #endregion

    #region Events

    /// <summary>
    /// Event launch when the scene is fully loaded
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);

        switch (scene.name)
        {
            case "StartUpScene": playBgmCorout(introMus); break;
            case "ShopScene": playBgmCorout(gameMus); break;
        }
    }

    #endregion
}
