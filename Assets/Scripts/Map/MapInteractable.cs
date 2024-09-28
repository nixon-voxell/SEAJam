using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MapInteractable : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] GameObject _PersonPosition;
    [SerializeField] ProximityPromptSystem _InteractSystem;

    public void Interact()
    {
        _PersonPosition.SetActive(true);
    }

    public void CloseInteract()
    {
        _PersonPosition.SetActive(false);
    }

    public void AddInteraction(UnityAction newAction)
    {
        _InteractSystem.AddInteraction(newAction);
    }
}
