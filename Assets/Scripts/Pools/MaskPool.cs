using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MaskPool : MonoBehaviour
{
    #region Parameters

    [SerializeField]
    private int sortOrder = 2;

    private int current;
    private int minLimitRoll = 0;
    private List<GameObject> masks = new List<GameObject>();

    private PoolMaskMovement poolMovVals;
    private Camera cam;
    private bool isMoving = false;

    #endregion

    #region Attributs

    /// <summary>
    /// Enabled mask list
    /// </summary>
    [HideInInspector]
    public List<Sprite> masksSprites;
    public float distance;

    #endregion

    #region Game Méthodes

    void Awake()
    {
        poolMovVals = GetComponentInParent<PoolMaskMovement>();
        cam = Camera.main;
    }

    void Start()
    {

    }

    #endregion

    #region Méthodes

    /// <summary>
    /// Return the mask curently created
    /// </summary>
    /// <returns>Current Mask in présentation</returns>
    public GameObject getCurrentMask()
    {
        return masks[modIndex(current)];
    }

    /// <summary>
    /// Get the index of the current mask
    /// </summary>
    /// <param name="i"></param>
    /// <returns>index</returns>
    private int modIndex(int i)
    {
        int c = masks.Count;
        return (((i % c) + c) % c);
    }

    /// <summary>
    /// Use for infinit scroll of the mask
    /// </summary>
    /// <param name="dir"></param>
    private void moveMaskAtEdge(Direction dir)
    {
        int multiplier = (dir == Direction.right ? 1 : -1);
        minLimitRoll += multiplier;

        GameObject currentObj = masks[modIndex(current)];
        GameObject movedObject = masks[modIndex(current + multiplier)];
        movedObject.transform.position = currentObj.transform.position;
        movedObject.transform.Translate(new Vector3(distance * -multiplier, 0, 0));
    }

    /// <summary>
    /// Set up the masks on start
    /// </summary>
    public void ShowMasks()
    {
        if (masksSprites.Count > 0)
        {
            distance = Screen.width * poolMovVals.maskDistance;
            current = masksSprites.Count / 2;

            for (int i = 0; i < masksSprites.Count; i++)
            {
                //sprite instanciation
                GameObject go = new GameObject(masksSprites[i].name);
                go.transform.SetParent(transform);
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = masksSprites[i];
                sr.sortingOrder = sortOrder;
                Vector3 pos = getPositionFromIndex(i);
                //scale the masks on the border smaller than the center one
                go.transform.localScale = new Vector3(poolMovVals.scale * 0.7f, poolMovVals.scale * 0.7f, 1);
                go.transform.position = pos;
                masks.Add(go);

                //sprite shadow
                GameObject shadow = Instantiate(go);
                shadow.transform.SetParent(go.transform);
                shadow.transform.position = go.transform.position;
                shadow.transform.Translate(new Vector3(0.1f, -0.1f, 0));
                SpriteRenderer shadow_render = shadow.GetComponent<SpriteRenderer>();
                shadow_render.color = Color.black;
                shadow_render.sortingOrder = -1;
            }
            masks[current].transform.localScale = new Vector3(poolMovVals.scale, poolMovVals.scale, 1);
            //retreive distance between 2 sprites, for movement
            distance = masks[0].transform.position.x - masks[1].transform.position.x;
        }
    }

    /// <summary>
    /// Return the position of the mask at the selected index
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private Vector3 getPositionFromIndex(int i)
    {
        float posX = distance * (i - Mathf.FloorToInt(masksSprites.Count / 2)) + (Screen.width / 2);
        Vector3 pos = cam.ScreenToWorldPoint(new Vector3(posX, 0, transform.position.z));
        pos.y = transform.position.y;
        return pos;
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// mask Tansition
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public IEnumerator moveDirection(Direction dir)
    {
        if (isMoving)
            yield break;
        isMoving = true;

        int dirMult = (dir == Direction.left ? -1 : 1);

        GameObject prevGo = masks[modIndex(current)];
        current += dirMult;
        GameObject currGo = masks[modIndex(current)];

        if (current == minLimitRoll)
            moveMaskAtEdge(Direction.left);
        if (current == minLimitRoll + masks.Count - 1)
            moveMaskAtEdge(Direction.right);


        float localPos = transform.position.x + (distance * dirMult);

        float scale = poolMovVals.scale;
        currGo.transform.DOScale(new Vector3(scale, scale, 1), poolMovVals.transitionSpeed);
        prevGo.transform.DOScale(new Vector3(scale * 0.7f, scale * 0.7f, 1), poolMovVals.transitionSpeed);

        yield return transform.DOMoveX(localPos, poolMovVals.transitionSpeed)
        .SetEase(poolMovVals.transitionCurve)
        .WaitForCompletion();

        isMoving = false;
    }

    #endregion
}

public class RandomSorter : IComparer
{
    #region Méthodes
    /// <summary>
    /// Use for random positionnement of the masks
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public int Compare(object x, object y)
    {
        return Random.Range(-1, 2);
    }
    #endregion
}
