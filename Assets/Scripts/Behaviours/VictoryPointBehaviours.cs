using TMPro;
using UnityEngine;

public class VictoryPointBehaviours : MonoBehaviour
{

    #region Parameters

    /// <summary>
    /// Rotation speed of the coin
    /// </summary>
    [SerializeField]
    [Range(0, 1f)]
    private float textRotationSpeed = 0.4f;
    
    /// <summary>
    /// Text mesh pro element acces
    /// </summary>
    private TextMeshProUGUI tmproElem;
    
    /// <summary>
    /// sprite index use for the coin animation
    /// </summary>
    private int spriteIndex = 0;
    
    /// <summary>
    /// srite timer use for the coin animation
    /// </summary>
    private float spriteTimer = 0f;
    
    /// <summary>
    /// Is the player earn a coin
    /// </summary>
    private bool showCoin;
    
    /// <summary>
    /// life duration of the GameObject
    /// </summary>
    private float lifeSpan;

    #endregion

    #region Attributs

    /// <summary>
    /// Notification text
    /// </summary>
    public string text;

    #endregion

    #region Game Méthodes

    void Start()
    {
        lifeSpan = 3f;
        tmproElem = GetComponentInChildren<TextMeshProUGUI>();
        tmproElem.text = text;

        showCoin = text.Contains("+");
        if (showCoin)
            tmproElem.text += "  <sprite=\"Gold\" index=0>";
    }

    void Update()
    {
        lifeSpan -= Time.deltaTime;

        spriteTimer += Time.deltaTime;
        if (spriteTimer >= textRotationSpeed && showCoin)
        {
            spriteTimer = 0;
            spriteIndex++;
            tmproElem.text = text + "  <sprite=\"Gold\" index=" + spriteIndex % 4 + ">";
        }
        transform.position += Vector3.up * Time.deltaTime;

        if (lifeSpan <= 0)
            Destroy(gameObject);
    }

    #endregion

}
