using UnityEngine;

public class DebugUtils : MonoBehaviour
{
    [SerializeField]
    private bool alwaysWin = false;

    [SerializeField]
    private bool skipClientAnimation  = false;


    [SerializeField]
    private bool skipTextAnimation = false;

    [SerializeField]
    private bool skipMaskGivingAnimation = false;

    private PoolMaskMovement movement = null;
    private LevelManager manager = null;
    private TextAppearance textAppearance = null;
    private SceneEventManager sceneEventManager = null;

    private float baseMovementSpeed = 0;

    private float baseTimeWaitClientEnter = 0;

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