using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
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
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }

    #endregion
}
