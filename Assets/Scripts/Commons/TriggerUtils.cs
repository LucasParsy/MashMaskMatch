using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class TriggerUtils
{
    //manually add a callback to a Trigger Event
    //src: http://answers.unity.com/answers/1440143/view.html
    public static void AddEventTriggerListener(EventTrigger trigger,
                                               EventTriggerType eventType,
                                               System.Action<PointerEventData> listener)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(data => listener.Invoke((PointerEventData)data));
        trigger.triggers.Add(entry);
    }

}