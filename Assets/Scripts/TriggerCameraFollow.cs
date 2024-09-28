using Unity.VisualScripting;
using UnityEngine;

public class TriggerCameraFollow : MonoBehaviour
{

    public GameObject targetPLayer;
    public bool c_EnableFollow;
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.GameObject() == targetPLayer)
        {
            c_EnableFollow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GameObject() == targetPLayer)
        {
            c_EnableFollow = false;
        }
    }
    
}
