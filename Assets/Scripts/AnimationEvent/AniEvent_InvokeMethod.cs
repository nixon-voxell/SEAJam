using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
struct AnimationEvent
{
    public string EventName;
    public UnityEvent Events;
}


public class AniEvent_InvokeMethod : MonoBehaviour
{
    [SerializeField] AnimationEvent[] AnimationEvents;

    public void InvokeMethod(string title)
    {
        foreach(AnimationEvent _animationEvent in AnimationEvents)
        {
            if (!string.Equals(AnimationEvents, title)) continue;

            _animationEvent.Events?.Invoke();
        }
    }
}
