using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class MapInteractable : MonoBehaviour, IInteractable
{
    [Header("Developer")]
    [SerializeField] Vector2 _MinMaxFlickerDelay;


    [Header("References")]
    [SerializeField] GameObject _PersonPosition;
    [SerializeField] ProximityPromptSystem _InteractSystem;

    #region Properties

    Animator m_Animator;

    #endregion

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartFlickerDelay();
    }

    void StartFlickerDelay()
    {
        // m_Animator.enabled = false;
        StartCoroutine(PlayFlicker());
    }

    IEnumerator PlayFlicker()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(_MinMaxFlickerDelay.x, _MinMaxFlickerDelay.y));
        m_Animator.Play("Ani_MapFlicker");
    }

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
