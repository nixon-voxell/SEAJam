using UnityEngine;

public class InteractionIcon : MonoBehaviour
{
    [SerializeField] private Vector3 m_Offset = new Vector3(0.0f, 1.5f, 0.0f);

    private void Update()
    {
        Vector3 parentPosition = this.transform.parent.position;

        this.transform.position = parentPosition + this.m_Offset;
        this.transform.rotation = Quaternion.identity;
    }
}
