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

    #endregion

    #region Game méthodes

    void Awake()
    {
        ShapeParts = LoadPart("Shapes");
        EyesParts = LoadPart("Eyes");
        MouthParts = LoadPart("Mouths");
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    #endregion

    #region Méthodes

    private Dictionary<Sprite, bool> LoadPart(string path)
    {
        Sprite[] loadedSprites = Resources.LoadAll<Sprite>(path);
        var dic = new Dictionary<Sprite, bool>();

        foreach (var sprite in loadedSprites)
        {
            var isUnlocked = sprite.name.Contains("African");
            dic.Add(sprite, isUnlocked);
        }

        return dic;
    }


    static public List<Sprite> getUnlockedMasks(Dictionary<Sprite, bool> dic)
    {
        var res = from kvp in dic
                  where kvp.Value == true
                  select kvp.Key;
        return res.ToList();
    }

    #endregion
}
