using UnityEngine;

public class ValveRotationTask : MonoBehaviour
{
    public Transform valveGraphic;
    public KeyItemFix keyItemFix;

    public void Rotate(float progress)
    {
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -progress * 360.0f);
    }

    private void Start()
    {
        this.keyItemFix.ProgressionAction += this.Rotate;
    }
}
