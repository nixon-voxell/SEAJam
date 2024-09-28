using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField, Voxell.Util.Scene] public string m_GameScene;

    public void PlayGame()
    {
        SceneManager.LoadScene(this.m_GameScene, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
