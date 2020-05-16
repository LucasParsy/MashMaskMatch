using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    #region Attributs

    public static SaveManager Instance { get; set; }
    public bool noSave;
    public PlayerSettings state;

    #endregion

    #region Game méthodes
    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        Load();

        if (noSave)
            ResetProgression();
    }
    #endregion

    #region Méthodes

    /// <summary>
    /// Save profile
    /// </summary>
    public void Save()
    {
        PlayerPrefs.SetString("player", SerializeHelper.Serialize<PlayerSettings>(state));
    }

    /// <summary>
    /// Load profile
    /// </summary>
    public void Load()
    {
        if (PlayerPrefs.HasKey("player"))
        {
            state = SerializeHelper.Deserialize<PlayerSettings>(PlayerPrefs.GetString("player"));
        }
        else
        {
            state = new PlayerSettings();
            Save();
        }
    }

    /// <summary>
    /// Check if the shape is unlocked or not by bit comparaison
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool isShapeUnlocked(int index)
    {
        //check if the bit is set, if so the shape is unlock
        return (state.unlockedShapes & 1 << index) != 0;
    }

    /// <summary>
    /// Check if the mouth is unlocked or not by bit comparaison
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool isMouthUnlocked(int index)
    {
        //check if the bit is set, if so the mouth is unlock
        return (state.unlockedMouth & 1 << index) != 0;
    }

    /// <summary>
    /// Check if the eyes is unlocked or not by bit comparaison
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool isEyesUnlocked(int index)
    {
        //check if the bit is set, if so the mouth is eyes
        return (state.unlockedEyes & 1 << index) != 0;
    }

    /// <summary>
    /// Toggle the right bit to unlock the shape
    /// </summary>
    /// <param name="index"></param>
    public void UnlockShape(int index)
    {
        //toggle on the bit at index
        state.unlockedShapes |= 1 << index;
        var shapes = GetComponent<MaskPartContainer>().ShapeParts.Values.ToList();
        shapes[index] = true;
    }

    /// <summary>
    /// Toggle the right bit to unlock the mouth
    /// </summary>
    /// <param name="index"></param>
    public void UnlockMouth(int index)
    {
        //toggle on the bit at index
        state.unlockedMouth |= 1 << index;
        var mouths = GetComponent<MaskPartContainer>().MouthParts.Values.ToList();
        mouths[index] = true;
    }

    /// <summary>
    /// Toggle the right bit to unlock the eyes
    /// </summary>
    /// <param name="index"></param>
    public void UnlockEyes(int index)
    {
        //toggle on the bit at index
        state.unlockedShapes |= 1 << index;
        var eyes = GetComponent<MaskPartContainer>().EyesParts.Values.ToList();
        eyes[index] = true;
    }

    /// <summary>
    /// Reset save state for player progression
    /// </summary>
    public void ResetProgression()
    {
        PlayerPrefs.DeleteKey("player");
    }
    #endregion
}
