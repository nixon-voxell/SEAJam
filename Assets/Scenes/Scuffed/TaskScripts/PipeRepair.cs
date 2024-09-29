using UnityEngine;

public class PipeRepairTask : MonoBehaviour
{
    public GameObject brokenPipeState;
    public GameObject fixedPipeState;
    public float repairTime = 2f;

    private bool isPlayerNear = false;
    private bool isRepairing = false;
    private float repairProgress = 0f;

    private void Start()
    {
        Debug.Log($"PipeRepairTask Start method called on GameObject: {gameObject.name}");
        
        if (brokenPipeState == null || fixedPipeState == null)
        {
            Debug.LogError($"PipeRepairTask on {gameObject.name}: Broken or Fixed pipe state is not assigned!");
            enabled = false;
            return;
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError($"PipeRepairTask on {gameObject.name}: No Collider2D found. Please add a trigger Collider2D.");
            enabled = false;
            return;
        }

        if (!collider.isTrigger)
        {
            Debug.LogWarning($"PipeRepairTask on {gameObject.name}: Collider2D is not set as a trigger. Setting it now.");
            collider.isTrigger = true;
        }

        brokenPipeState.SetActive(true);
        fixedPipeState.SetActive(false);

        Debug.Log($"PipeRepairTask on {gameObject.name} initialized successfully.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            Debug.Log($"Player entered PipeRepairTask trigger area on {gameObject.name}.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            isRepairing = false;
            repairProgress = 0f;
            Debug.Log($"Player exited PipeRepairTask trigger area on {gameObject.name}.");
        }
    }

    private void Update()
    {
        if (isPlayerNear)
        {
            if (GameManager.Singleton == null)
            {
                Debug.LogError("PipeRepairTask: GameManager.Singleton is null!");
                return;
            }

            bool hasPipe = GameManager.Singleton.HasPipe();
            Debug.Log($"Player near pipe on {gameObject.name}. Has pipe: {hasPipe}. IsRepairing: {isRepairing}");

            if (hasPipe)
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
                    Debug.Log($"Repairing progress on {gameObject.name}: {repairProgress}/{repairTime}");
                    if (repairProgress >= repairTime)
                    {
                        RepairPipe();
                    }
                }
            }
        }
    }

    private void StartRepairing()
    {
        isRepairing = true;
        Debug.Log($"Started repairing pipe on {gameObject.name}.");
    }

    private void StopRepairing()
    {
        isRepairing = false;
        repairProgress = 0f;
        Debug.Log($"Stopped repairing pipe on {gameObject.name}.");
    }

    private void RepairPipe()
    {
        brokenPipeState.SetActive(false);
        fixedPipeState.SetActive(true);
        GameManager.Singleton.UsePipe();
        Debug.Log($"Pipe repaired successfully on {gameObject.name}!");
        
        this.enabled = false;
    }
}