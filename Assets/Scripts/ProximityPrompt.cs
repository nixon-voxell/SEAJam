using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class ProximityPromptSystem : MonoBehaviour, IInteractable
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private GameObject promptIconObject;
    [SerializeField] private float maxScale = 1.1f;
    [SerializeField] private UsableItemBase Item;

    [SerializeField] UnityEvent _InteractionEvent;

    private bool playerInRange = false;
    private bool isVisible = false;
    private SpriteRenderer promptIconRenderer;

    private void Start()
    {
        if (promptIconObject != null)
        {
            promptIconRenderer = promptIconObject.GetComponent<SpriteRenderer>();
        }
        HidePrompt(true);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ShowPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        if (!isVisible && promptIconObject != null)
        {
            isVisible = true;
            promptIconObject.transform.localScale = Vector3.one * 0.9f;
            promptIconObject.transform.DOScale(Vector3.one * maxScale, animationDuration).SetEase(Ease.OutBack);
            promptIconRenderer.DOFade(1f, animationDuration);
        }
    }

    private void HidePrompt(bool instant = false)
    {
        if ((isVisible || instant) && promptIconObject != null)
        {
            isVisible = false;
            if (instant)
            {
                promptIconObject.transform.localScale = Vector3.zero;
                promptIconRenderer.color = new Color(1f, 1f, 1f, 0f);
            }
            else
            {
                promptIconObject.transform.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack);
                promptIconRenderer.DOFade(0f, animationDuration);
            }
        }
    }

    public void Interact()
    {
        this.HidePrompt();
        _InteractionEvent?.Invoke();
    }

    public void AddInteraction(UnityAction newAction)
    {
        _InteractionEvent.AddListener(newAction);
    }
}
