using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Transform m_ProgressBar;

    public void SetProgress(float progress)
    {
        this.m_ProgressBar.localScale = new Vector3(progress, 1.0f, 1.0f);
    }
}
