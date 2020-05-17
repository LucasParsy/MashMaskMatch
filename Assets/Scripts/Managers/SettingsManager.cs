using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public void OnClickReturnBtn()
    {
        DebugUtils.GetGameMaster().GetComponent<SceneMaster>().LaunchStartUpScene();
    }

    public void OnClickResetProgress()
    {
        SaveManager.Instance.ResetProgression();
        OnClickReturnBtn();
    }
}
