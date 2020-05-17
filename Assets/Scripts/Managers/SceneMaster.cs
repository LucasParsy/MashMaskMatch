using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMaster : MonoBehaviour
{
    #region Parameters

    /// <summary>
    /// GO of the Glossary btn in selection menu
    /// </summary>
    private GameObject _marketBtn;

    /// <summary>
    /// GO of the Glossary btn in selection menu
    /// </summary>
    private GameObject _glossaryBtn;

    /// <summary>
    /// is the glossary scene is use as a market
    /// </summary>
    public bool isPurchase;

    /// <summary>
    /// Go of the return btn in the glossary scene
    /// </summary>
    private GameObject _glossaryReturnBtn;

    /// <summary>
    /// GO of the Shop btn in selection menu
    /// </summary>
    private GameObject _shopBtn;

    /// <summary>
    /// GO of the Option btn in selection menu
    /// </summary>
    private GameObject _optionBtn;

    /// <summary>
    /// GO of the return btn in Option menu
    /// </summary>
    private GameObject _optionReturnBtn;

    /// <summary>
    /// GO of the Reset btn in Option menu
    /// </summary>
    private GameObject _optionResetBtn;

    /// <summary>
    /// SceneEventManager accès
    /// </summary>
    private SceneEventManager _shopSceneManager;
    #endregion

    #region Game Méthodes

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        
    }

    #endregion

    #region Méthodes

    /// <summary>
    /// Launch principal game scene
    /// </summary>
    public void LaunchStartUpScene()
    {
        SceneManager.sceneLoaded += StartUpScene_sceneLoaded;
        SceneManager.LoadScene("StartUpScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// Set up GlossaryScene
    /// </summary>
    public void LaunchGlossaryScene()
    {
        SceneManager.sceneLoaded += GlossaryScene_sceneLoaded;
        SceneManager.LoadScene("MaskGlossaryScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// Set up ShopScene
    /// </summary>
    public void LaunchShopScene()
    {
        SceneManager.sceneLoaded += ShopScene_sceneLoaded;
        SceneManager.LoadScene("ShopScene", LoadSceneMode.Single);
    }

    public void LaunchPlayerSettingsScene()
    {
        SceneManager.sceneLoaded += PlayerSettingsScene_Loaded;
        SceneManager.LoadScene("PlayerSettingsScene", LoadSceneMode.Single);
    }

    #endregion

    #region Event

    /// <summary>
    /// Event fire when SelectionMenuScene is fully loaded
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void StartUpScene_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= StartUpScene_sceneLoaded;
        GameObject.Find("Masks").SetActive(false);
        GameObject.Find("StartMenuManager").GetComponent<StartMenuManager>().SetUpBtnMenu();
    }

    /// <summary>
    /// Event fire when GlossaryScene is fully loaded
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void GlossaryScene_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= GlossaryScene_sceneLoaded;

        var scrollRect = GameObject.Find("Panel").GetComponent<ScrollRect>();
        var manager = GameObject.Find("GlossarySceneManager").GetComponent<GlossaryManager>();
        var goldCount = GameObject.Find("GoldCount");

        if (isPurchase)
        {
            goldCount.GetComponentInChildren<TextMeshProUGUI>().text = 
                SaveManager.Instance.state.gold.ToString();

            scrollRect.horizontal = true;
            scrollRect.vertical = false;

            manager.IsPurchase = true;
            manager.IsVertical = false;
        }
        else
        {
            goldCount.SetActive(false);

            scrollRect.horizontal = false;
            scrollRect.vertical = true;

            manager.IsPurchase = false;
            manager.IsVertical = true;
        }
    }

    /// <summary>
    /// Event fire when ShopScene is fully loaded
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void ShopScene_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= ShopScene_sceneLoaded;
        _shopSceneManager = GameObject.Find("GameManager").GetComponent<SceneEventManager>();
        _shopSceneManager.LaunchStartOfDay();
    }

    /// <summary>
    /// Event fire when PlayerSettingsScene is fullyloaded
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void PlayerSettingsScene_Loaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= PlayerSettingsScene_Loaded;

        GameObject.Find("GoldCount").GetComponent<TextMeshProUGUI>().text = SaveManager.Instance.state.gold.ToString();
        GameObject.Find("DaysCount").GetComponent<TextMeshProUGUI>().text = "\n" + SaveManager.Instance.state.completedDays.ToString();

        var master = DebugUtils.GetGameMaster().GetComponent<MaskPartContainer>();
        var unlockedMaskParts = MaskPartContainer.GetUnlockedMasks(master.ShapeParts).Count;
        unlockedMaskParts += MaskPartContainer.GetUnlockedMasks(master.MouthParts).Count;
        unlockedMaskParts += MaskPartContainer.GetUnlockedMasks(master.EyesParts).Count;

        GameObject.Find("MaskCount").GetComponent<TextMeshProUGUI>().text = "\n" + unlockedMaskParts.ToString();
    }
    #endregion
}
