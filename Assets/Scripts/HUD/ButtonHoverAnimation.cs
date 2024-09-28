using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DG.Tweening;

public class SimpleButtonHoverAnimation : MonoBehaviour
{
    [System.Serializable]
    public class AnimatedButton
    {
        public Button button;
        public float hoverScale = 1.05f; 
    }

    public List<AnimatedButton> buttons = new List<AnimatedButton>();
    [SerializeField] private float hoverDuration = 0.1f; 

    // this engine ends me
    private void Start()
    {
        foreach (var animatedButton in buttons)
        {
            SetupButtonAnimation(animatedButton);
        }
    }

    private void SetupButtonAnimation(AnimatedButton animatedButton)
    {
        if (animatedButton.button == null) return;

        EventTrigger trigger = animatedButton.button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = animatedButton.button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entryEnter = new EventTrigger.Entry();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) => { OnPointerEnter(animatedButton); });
        trigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry();
        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) => { OnPointerExit(animatedButton); });
        trigger.triggers.Add(entryExit);
    }

    private void OnPointerEnter(AnimatedButton animatedButton)
    {
        animatedButton.button.transform.DOScale(Vector3.one * animatedButton.hoverScale, hoverDuration);
    }

    private void OnPointerExit(AnimatedButton animatedButton)
    {
        animatedButton.button.transform.DOScale(Vector3.one, hoverDuration);
    }

    private void OnDisable()
    {
        foreach (var animatedButton in buttons)
        {
            if (animatedButton.button != null)
            {
                animatedButton.button.transform.DOKill();
                animatedButton.button.transform.localScale = Vector3.one;
            }
        }
    }
}