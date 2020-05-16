using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GlossaryManager : MonoBehaviour
{
    #region Parameters

    List<Sprite> unlockedMaskParts;
    GameObject gameMaster;

    #endregion

    #region Attributs

    public GameObject masksContainer;
    public GameObject maskPresentation;

    #endregion

    #region Game méthodes

    void Start()
    {
        unlockedMaskParts = new List<Sprite>();
        MaskPartContainer container = DebugUtils.getGameMaster().GetComponent<MaskPartContainer>();
        FillUnlockedPartList(container.ShapeParts);
        FillUnlockedPartList(container.EyesParts);
        FillUnlockedPartList(container.MouthParts);
        InstanciateMasksPresentation();
    }

    #endregion

    #region Méthodes

    /// <summary>
    /// fill the unlockedMaskPart list with the unlocked part from the partList
    /// </summary>
    /// <param name="partList"></param>
    private void FillUnlockedPartList(Dictionary<Sprite, bool> partList)
    {
        unlockedMaskParts.AddRange(MaskPartContainer.getUnlockedMasks(partList));
    }

    /// <summary>
    /// Resize container, set his position and fill it with the unlocked masks
    /// </summary>
    private void InstanciateMasksPresentation()
    {
        var height = unlockedMaskParts.Count * 800;
        var offset = 450;
        masksContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(100, height);
        masksContainer.GetComponent<RectTransform>().localPosition = new Vector3(0, -height);

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
            go.transform.localPosition = new Vector3(0, (index * -750) + (height/2) - offset, 0);
            go.transform.localScale = new Vector3(2, 2, 2);

            index++;
        }
    }
    #endregion
}
