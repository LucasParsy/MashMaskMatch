using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool isPurchase;

    /// <summary>
    /// Go of the return btn in the glossary scene
    /// </summary>
    private GameObject _glossaryReturnBtn;

    /// <summary>
    /// GO of the Shop btn in selection menu
    /// </summary>
    private GameObject _shopBtn;

    /// <summary>
    /// SceneEventManager accès
    /// </summary>
    private SceneEventManager _shopSceneManager;
    #endregion

    #region Game Méthodes

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    #endregion

    #region Méthodes

    /// <summary>
    /// Launch principal game scene
    /// </summary>
    public void LaunchSelectionMenuScene()
    {
        SceneManager.sceneLoaded += SelectionMenu_sceneLoaded;
        SceneManager.LoadScene("SelectionMenuScene", LoadSceneMode.Single);
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

    #endregion

    #region Event

    /// <summary>
    /// Event fire when SelectionMenuScene is fully loaded
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void SelectionMenu_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= SelectionMenu_sceneLoaded;

        _glossaryBtn = GameObject.Find("GlossaryBtn");
        _glossaryBtn.GetComponent<Button>().onClick.AddListener(delegate 
            { 
                isPurchase = false; 
                LaunchGlossaryScene(); 
            });

        _marketBtn = GameObject.Find("MarketBtn");
        _marketBtn.GetComponent<Button>().onClick.AddListener(delegate
        {
            isPurchase = true;
            LaunchGlossaryScene();
        });

        _shopBtn = GameObject.Find("ShopBtn");
        _shopBtn.GetComponent<Button>().onClick.AddListener(delegate { LaunchShopScene(); });
    }

    /// <summary>
    /// Event fire when GlossaryScene is fully loaded
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void GlossaryScene_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.sceneLoaded -= GlossaryScene_sceneLoaded;
        _glossaryReturnBtn = GameObject.Find("ReturnBtn");
        _glossaryReturnBtn.GetComponent<Button>().onClick.AddListener(delegate { LaunchSelectionMenuScene(); });

        var scrollRect = GameObject.Find("Panel").GetComponent<ScrollRect>();
        var manager = GameObject.Find("GlossarySceneManager").GetComponent<GlossaryManager>();

        if (isPurchase)
        {
            scrollRect.horizontal = true;
            scrollRect.vertical = false;
            manager.IsPurchase = true;
            manager.IsVertical = false;
        }
        else
        {
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
    #endregion
}
