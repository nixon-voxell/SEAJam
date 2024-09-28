using UnityEngine;
using UnityEngine.SceneManagement;

public class RadiationRadius : MonoBehaviour
{
    public float maxRadiationCounter = 100f;
    public float maxRadiationRate = 50f;
    public float minRadiationRate = 5f;

    private float currentRadiationCounter = 0f;
    private Transform playerTransform;
    private CircleCollider2D radiationCollider;

    private void Start()
    {
        radiationCollider = GetComponent<CircleCollider2D>();
        if (radiationCollider == null)
        {
            Debug.LogError("CircleCollider2D not found on RadiationRadius object!");
        }
        
        GameManager.Singleton.RegisterRadiationSource(this);
    }

    private void OnDestroy()
    {
        if (GameManager.Singleton != null)
        {
            GameManager.Singleton.UnregisterRadiationSource(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = null;
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            float distanceToCenter = Vector2.Distance(transform.position, playerTransform.position);
            float radiationRate = CalculateRadiationRate(distanceToCenter);
            IncreaseRadiationCounter(radiationRate);

        }
        else
        {
            DecreaseRadiationCounter();
        }

        CheckRadiationLevel();
    }

    private float CalculateRadiationRate(float distance)
    {
        float radius = radiationCollider.radius;
        float normalizedDistance = Mathf.Clamp01(distance / radius);
        return Mathf.Lerp(maxRadiationRate, minRadiationRate, normalizedDistance);
    }

    private void IncreaseRadiationCounter(float rate)
    {
        currentRadiationCounter += rate * Time.deltaTime;
        currentRadiationCounter = Mathf.Min(currentRadiationCounter, maxRadiationCounter);
    }

    private void DecreaseRadiationCounter()
    {
        currentRadiationCounter -= minRadiationRate * Time.deltaTime;
        currentRadiationCounter = Mathf.Max(currentRadiationCounter, 0f);
    }

    private void CheckRadiationLevel()
    {
        if (currentRadiationCounter >= maxRadiationCounter)
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public float GetCurrentRadiationLevel()
    {
        return currentRadiationCounter / maxRadiationCounter;
    }
}