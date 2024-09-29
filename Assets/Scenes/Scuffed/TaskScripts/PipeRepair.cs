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
            StopRepairing();
            Debug.Log($"Player exited PipeRepairTask trigger area on {gameObject.name}.");
        }
    }

    private void Update()
    {
        if (isPlayerNear && HasPipeInInventory())
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
                    CompletePipeRepair();
                }
            }
        }
    }

    private bool HasPipeInInventory()
    {
        UsableItemBase activeItem = Inventory.Singleton.GetCurrentActiveItem();
        return activeItem != null && activeItem is Pipe;
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

    private void CompletePipeRepair()
    {
        brokenPipeState.SetActive(false);
        fixedPipeState.SetActive(true);
        
        UsableItemBase activeItem = Inventory.Singleton.GetCurrentActiveItem();
        if (activeItem is Pipe pipe)
        {
            pipe.UseForRepair();
        }
        else
        {
            Debug.LogError("Active item is not a Pipe when completing repair!");
        }

        Debug.Log($"Pipe repaired successfully on {gameObject.name}!");
        
        this.enabled = false;
    }
}