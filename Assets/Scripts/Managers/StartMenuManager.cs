using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    public Animator maskAnimator;
    public GameObject BtnContainer;

    private GameObject gameMaster;
    private SceneMaster sceneMaster;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = DebugUtils.GetGameMaster();
        sceneMaster = gameMaster.GetComponent<SceneMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFirstClickStartBtn()
    {
        maskAnimator.SetTrigger("StartPressed");
        SetUpBtnMenu();
    }

    public void SetUpBtnMenu()
    {
        BtnContainer.SetActive(true);

        var shopBtn = BtnContainer.transform.Find("ShopBtn");
        shopBtn.GetComponent<Button>().onClick.AddListener(delegate
        {
            sceneMaster.isPurchase = true;
            sceneMaster.LaunchGlossaryScene();
        });

        var optionBtn = BtnContainer.transform.Find("OptionBtn");
        optionBtn.GetComponent<Button>().onClick.AddListener(delegate
        {
            sceneMaster.LaunchPlayerSettingsScene();
        });

        var glossaryBtn = BtnContainer.transform.Find("GlossaryBtn");
        glossaryBtn.GetComponent<Button>().onClick.AddListener(delegate
        {
            sceneMaster.isPurchase = false;
            sceneMaster.LaunchGlossaryScene();
        });

        GameObject.Find("PlayButton").GetComponent<Button>().onClick.AddListener(delegate 
        {
            sceneMaster.LaunchShopScene();
        });
    }
}
