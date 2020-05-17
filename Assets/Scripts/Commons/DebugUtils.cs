using UnityEngine;

public class DebugUtils : MonoBehaviour
{
    [SerializeField]
    private bool alwaysWin;

    [SerializeField]
    private bool skipClientAnimation;


    [SerializeField]
    private bool skipTextAnimation;

    [SerializeField]
    private bool skipMaskGivingAnimation;


    [SerializeField]
    private bool unlockAllMasks;


    private PoolMaskMovement movement;
    private LevelManager manager;
    private TextAppearance textAppearance;
    private SceneEventManager sceneEventManager;

    private float baseMovementSpeed;

    private float baseTimeWaitClientEnter;

    #region GameMethods
    private void Init()
    {
        GameObject pools = GameObject.Find("pools");
        movement = pools.GetComponent<PoolMaskMovement>();
        manager = pools.GetComponent<LevelManager>();
        textAppearance = GameObject.Find("Dialogue").GetComponent<TextAppearance>();
        sceneEventManager = GameObject.Find("GameManager").GetComponent<SceneEventManager>();

        //get default values to allow for live restoration 
        baseMovementSpeed = movement.transitionSpeed;
        baseTimeWaitClientEnter = sceneEventManager.timeWaitClientEnter;
    }

    private void OnValidate()
    {
        //prevent changing variables in editor and erasing them accidentally...
        if (!Application.isPlaying)
            return;
        if (movement == null)
            Init();
        manager.alwaysWin = alwaysWin;
        textAppearance.isInstant = skipTextAnimation;
        movement.transitionSpeed = skipMaskGivingAnimation ? 0.05f : baseMovementSpeed;
        sceneEventManager.timeWaitClientEnter = skipClientAnimation ? 0 : baseTimeWaitClientEnter;
    }

    #endregion
    #region Methods

    static public GameObject GetGameMaster()
    {
        GameObject master = GameObject.Find("GameMasterDEBUG");
        if (master == null)
            master = GameObject.Find("GameMaster");
        return master;
    }

    #endregion
}