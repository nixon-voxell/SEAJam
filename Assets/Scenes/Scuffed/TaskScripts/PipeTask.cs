using UnityEngine;
using System.Collections;

public class PipeTask : MonoBehaviour
{
    public GameObject brokenPipeState;
    public GameObject fixedPipeState;
    public float repairTime = 2f; 

    private bool isPlayerNear = false;
    private bool isRepairing = false;
    private float repairProgress = 0f;

    private void Start()
    {
        brokenPipeState.SetActive(true);
        fixedPipeState.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            isRepairing = false;
            repairProgress = 0f;
        }
    }

    private void Update()
    {
        if (isPlayerNear && GameManager.Singleton.HasPipe())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartRepairing();
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                StopRepairing();
            }

            if (isRepairing)
            {
                repairProgress += Time.deltaTime;
                if (repairProgress >= repairTime)
                {
                    RepairPipe();
                }
            }
        }
    }

    private void StartRepairing()
    {
        isRepairing = true;
    }

    private void StopRepairing()
    {
        isRepairing = false;
        repairProgress = 0f;
    }

    private void RepairPipe()
    {
        brokenPipeState.SetActive(false);
        fixedPipeState.SetActive(true);
        GameManager.Singleton.UsePipe();
        this.enabled = false;
    }
}