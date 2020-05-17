using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlossaryManager : MonoBehaviour
{
    #region Parameters

    List<Sprite> unlockedMaskParts;
    GameObject gameMaster;

    #endregion

    #region Attributs

    public GameObject masksContainer;
    public GameObject maskPresentation;
    public GameObject shopBackground;

    public bool IsPurchase;
    public bool IsVertical;

    #endregion

    #region Game méthodes

    void Start()
    {
        unlockedMaskParts = new List<Sprite>();
        FillMaskParts();
        InstanciateMasksPresentation();

        if(IsPurchase)
            shopBackground.SetActive(true);
        else
            shopBackground.SetActive(false);
    }

    #endregion

    #region Méthodes

    public void FillMaskParts()
    {
        unlockedMaskParts.Clear();
        MaskPartContainer container = DebugUtils.GetGameMaster().GetComponent<MaskPartContainer>();
        FillPartList(container.ShapeParts);
        FillPartList(container.EyesParts);
        FillPartList(container.MouthParts);
    }

    /// <summary>
    /// fill the unlockedMaskPart list with the unlocked part from the partList
    /// </summary>
    /// <param name="partList"></param>
    private void FillPartList(Dictionary<Sprite, bool> partList)
    {
        if(IsPurchase)
            unlockedMaskParts.AddRange(MaskPartContainer.GetLockedMasks(partList));
        else
            unlockedMaskParts.AddRange(MaskPartContainer.GetUnlockedMasks(partList));
    }

    /// <summary>
    /// Resize container, set his position and fill it with the unlocked masks
    /// </summary>
    private void InstanciateMasksPresentation()
    {
        var offset = 450;
        var distance = IsVertical ? 800 : 500;
        var containerSize = unlockedMaskParts.Count * distance;

        masksContainer.GetComponent<RectTransform>().sizeDelta 
            = IsVertical ? new Vector2(100, containerSize) : new Vector2(containerSize, 100);
        masksContainer.GetComponent<RectTransform>().localPosition 
            = IsVertical ? new Vector2(100, -containerSize) : new Vector2(-containerSize, -400);

        var index = 0;
        foreach (var item in unlockedMaskParts)
        {
            var go = Instantiate(maskPresentation, Vector3.zero, Quaternion.identity, masksContainer.transform);

            var splitName = item.name.Split('_');
            var pos = splitName[1].IndexOf("(Clone)");
            var description = splitName[1];
            if (pos > 0)
                description = splitName[1].Remove(pos);

            go.transform.Find("maskDescription").GetComponent<TextMeshProUGUI>().text = description;
            go.transform.Find("maskPart").GetComponent<SpriteRenderer>().sprite = item;
            var buyBtn = go.transform.Find("purchaseBtn").gameObject;

            if (!IsPurchase)
            {
                buyBtn.SetActive(false);
                go.transform.Find("price").gameObject.SetActive(false);
            }
            else
            {
                buyBtn.GetComponent<Button>().onClick.AddListener(delegate 
                {
                    OnClickBuyMask(go);
                });
            }

            var position = (index * -distance) + (containerSize / 2) - offset;
            go.transform.localPosition = IsVertical ? new Vector3(0, position, 0) : new Vector3(position, 0, 0);
            go.transform.localScale = new Vector3(2, 2, 2);

            index++;
        }
    }

    public void OnClickBuyMask(GameObject item)
    {
        var price = int.Parse(item.transform.Find("price").GetComponentInChildren<TextMeshProUGUI>().text);
        var gold = SaveManager.Instance.state.gold;

        if (gold >= price)
        {
            var sprite = item.transform.Find("maskPart").GetComponent<SpriteRenderer>().sprite;

            var partType = sprite.name.Contains("Eyes") ?
                "Eyes" : sprite.name.Contains("Mouth") ? 
                "Mouth" : sprite.name.Contains("Shape") ? 
                "Shape" : "unknown";

            switch (partType)
            {
                case "Eyes":
                    SaveManager.Instance.UnlockEyes(sprite);
                    break;
                case "Mouth":
                    SaveManager.Instance.UnlockMouth(sprite);
                    break;
                case "Shape":
                    SaveManager.Instance.UnlockShape(sprite);
                    break;
                default:
                    break;
            }

            SaveManager.Instance.state.gold -= price;
            GameObject.Find("GoldCount").GetComponentInChildren<TextMeshProUGUI>().text = SaveManager.Instance.state.gold.ToString();
            SaveManager.Instance.Save();
            Destroy(item.gameObject);
        }
    }

    public void OnClickReturnBtn()
    {
        DebugUtils.GetGameMaster().GetComponent<SceneMaster>().LaunchStartUpScene();
    }
    #endregion
}
