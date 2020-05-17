using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaskPartContainer : MonoBehaviour
{

    #region Attributs
 
    /// <summary>
    /// Dictionary that contain all the shapes parts
    /// Bool : true if the player unlock this parts
    /// </summary>
    public Dictionary<Sprite, bool> ShapeParts;

    /// <summary>
    /// Dictionary that contain all the eyes parts
    /// Bool : true if the player unlock this parts
    /// </summary>
    public Dictionary<Sprite, bool> EyesParts;

    /// <summary>
    /// Dictionary that contain all the mouths parts
    /// Bool : true if the player unlock this parts
    /// </summary>
    public Dictionary<Sprite, bool> MouthParts;

    public bool isDebug;
    public bool allMasksUnlocked;
    #endregion

    #region Game méthodes

    private void Awake()
    {
        if (isDebug)
        {
            LoadMasks();
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);

        if (!isDebug)
        {
            LoadMasks();
            ReloadUnlockedMasks();
        }

    }

    #endregion

    #region Méthodes

    public void LoadMasks()
    {
        ShapeParts = LoadPart("Shapes");
        EyesParts = LoadPart("Eyes");
        MouthParts = LoadPart("Mouths");
    }

    public void ReloadUnlockedMasks()
    {
        SetUnlockedParts(ShapeParts, 1);
        SetUnlockedParts(EyesParts, 2);
        SetUnlockedParts(MouthParts, 3);
    }

    private Dictionary<Sprite, bool> LoadPart(string path)
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>(path);
        var dic = new Dictionary<Sprite, bool>();

        foreach (var sprite in loadedSprites)
        {
            var isUnlocked = 
                allMasksUnlocked ? 
                allMasksUnlocked : 
                isDebug && sprite.name.StartsWith("African") ? 
                true : 
                allMasksUnlocked;

            dic.Add(sprite, isUnlocked);
        }

        return dic;
    }

    private void SetUnlockedParts(Dictionary<Sprite, bool> parts, int partSettings)
    {
        var sprites = parts.Keys.ToList();
        var values = parts.Values.ToList();

        for (int i = 0; i < values.Count; i++)
        {
            if (!sprites[i].name.StartsWith("African"))
            {
                var isUnlocked = false;

                switch (partSettings)
                {
                    case 1:
                        isUnlocked = SaveManager.Instance.isShapeUnlocked(i);
                        break;
                    case 2:
                        isUnlocked = SaveManager.Instance.isEyesUnlocked(i);
                        break;
                    case 3:
                        isUnlocked = SaveManager.Instance.isMouthUnlocked(i);
                        break;
                }

                if (values[i] != isUnlocked)
                {
                    parts[sprites[i]] = isUnlocked;
                }
            }
            else
            {
                parts[sprites[i]] = true;
            }
        }
    }

    static public List<Sprite> GetUnlockedMasks(Dictionary<Sprite, bool> dic)
    {
        var res = from kvp in dic
                  where kvp.Value == true
                  select kvp.Key;
        return res.ToList();
    }

    static public List<Sprite> GetLockedMasks(Dictionary<Sprite, bool> dic)
    {
        var res = from kvp in dic
                  where kvp.Value == false
                  select kvp.Key;
        return res.ToList();
    }

    #endregion
}
