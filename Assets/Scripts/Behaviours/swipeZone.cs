﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Direction { left, right };

public class swipeZone : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    #region Parameters

    [SerializeField]
    [Range(0.1f, 3f)]
    private float swipeTreshold = 2;

    [SerializeField]
    [Range(0.1f, 20f)]
    private float swipeUpTreshold = 10;


    [SerializeField]
    private MaskPool maskPool = null;

    private Vector2 startDragPos;
    private float startDragTime;
    private Vector2 endDragPos;
    private Vector2 canvasSize;

    #endregion

    #region Game Méthodes

    void Start()
    {
        RectTransform tf = gameObject.GetComponent<RectTransform>();
        canvasSize = tf.rect.size;
        setupButton("left", Direction.left);
        setupButton("right", Direction.right);
    }

    #endregion

    #region Méthodes

    private void setupButton(string name, Direction dir)
    {
        Transform buttObj = gameObject.transform.Find(name);
        var butt = buttObj.GetComponent<Button>();
        butt.onClick.AddListener(delegate { swipe(dir); });

        //so the swipe is not shadowed by the button press
        var trigger = butt.GetComponent<EventTrigger>();
        TriggerUtils.AddEventTriggerListener(trigger, EventTriggerType.PointerUp, OnPointerUp);
        TriggerUtils.AddEventTriggerListener(trigger, EventTriggerType.PointerDown, OnPointerDown);
    }

    public void swipe(Direction dir)
    {
        StartCoroutine(maskPool.moveDirection(dir));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("pointer up" + eventData.position);
        Vector2 movement = eventData.position - startDragPos;
        movement /= canvasSize;
        float time = Time.time - startDragTime;
        float speedMultiplier = 1 / time;
        if (Mathf.Abs(movement.x) * speedMultiplier >= swipeTreshold)
            swipe(movement.x > 0 ? Direction.left : Direction.right);

        if (Mathf.Abs(movement.y) * speedMultiplier >= swipeUpTreshold)
        {
            //Debug.Log("give " + movement + "speed " + speedMultiplier);
            maskPool.GetComponentInParent<PoolMaskMovement>().giveMask();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("pointer down" + eventData.position);
        startDragPos = eventData.position;
        startDragTime = Time.time;
    }

    #endregion

}
