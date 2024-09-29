using UnityEngine;
using DG.Tweening;

public class ValveRotationTask : MonoBehaviour
{
    public Transform valveGraphic;
    public float rotationDuration = 2f;
    public float totalRotationAngle = 360f;
    public float interactionRange = 2f;

    private bool isPlayerNear = false;
    private bool isRotating = false;
    private bool isCompleted = false;
    private float rotationProgress = 0f;

    private void Start()
    {
        if (valveGraphic == null)
        {
            Debug.LogError("Valve graphic is not assigned to the ValveRotationTask script!");
            enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCompleted)
        {
            isPlayerNear = true;
            Debug.Log("Player entered valve interaction range.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (!isCompleted) StopRotating();
            Debug.Log("Player exited valve interaction range.");
        }
    }

    private void Update()
    {
        if (isPlayerNear && !isCompleted)
        {
            if (Input.GetKey(KeyCode.E))
            {
                RotateValve();
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                StopRotating();
            }
        }
    }

    private void RotateValve()
    {
        if (!isRotating && !isCompleted)
        {
            isRotating = true;
            Debug.Log("Started rotating valve.");
        }

        rotationProgress += Time.deltaTime / rotationDuration;
        float currentRotation = totalRotationAngle * rotationProgress;

        valveGraphic.DORotate(new Vector3(0, 0, currentRotation), 0.1f, RotateMode.Fast);

        if (rotationProgress >= 1f)
        {
            CompleteValveRotation();
        }
    }
    private int task_r;
    private int task_s;

    private void TasksCompleted_Rje(){

    }

    private void StopRotating()
    {
        if (isRotating && !isCompleted)
        {
            isRotating = false;
            Debug.Log("Stopped rotating valve. Progress: " + (rotationProgress * 100) + "%");
        }
    }

    private void CompleteValveRotation()
    {
        isRotating = false;
        isCompleted = true;
        
            valveGraphic.DORotate(new Vector3(0, 0, totalRotationAngle), 0.1f, RotateMode.Fast).OnComplete(() =>
        {
            Debug.Log("Valve rotation completed!");
        });

        GetComponent<Collider2D>().enabled = false;
    }
}