using UnityEngine;

public class ProximityBattery : MonoBehaviour
{
    public float interactionRadius = 2f;
    private bool playerInRange = false;
    private CircleCollider2D proximityCollider;

    private void Start()
    {
        proximityCollider = GetComponent<CircleCollider2D>();
        if (proximityCollider == null)
        {
            proximityCollider = gameObject.AddComponent<CircleCollider2D>();
        }
        
        proximityCollider.radius = interactionRadius;
        proximityCollider.isTrigger = true;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            UseBattery();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void UseBattery()
    {
        if (GameManager.Singleton != null)
        {
            GameManager.Singleton.ShowBatteryOverlay();
            Destroy(gameObject);  // Destroy the battery GameObject after use
        }
    }
}