using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

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


    public IEnumerator creditsClickCorout(bool isEnter)
    {
        float transitionDuration = 1;
        int direction = isEnter ? 0 : 2000;
        float fade = isEnter ? 0.35f : 0;

        GameObject go = GameObject.Find("creditsDisplay");
        go.transform.DOLocalMoveX(direction, transitionDuration).SetEase(Ease.Linear);

        Image bg = GameObject.Find("BackgroundFaderBlocker").GetComponent<Image>();
        yield return bg.DOFade(fade, transitionDuration).WaitForCompletion();
        bg.raycastTarget = isEnter;
    }
    public void onClickCredits(bool isEnter)
    {
        StartCoroutine(creditsClickCorout(isEnter));
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
