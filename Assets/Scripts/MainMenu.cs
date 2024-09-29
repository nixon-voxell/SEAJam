using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField, Voxell.Util.Scene] private string m_GameScene;
    [SerializeField] private TutorialPromptUI tutorialPrompt;
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private RawImage backgroundImage;
    [SerializeField] private Image blackBackground;

    private void Start()
    {
        // Ensure the tutorial prompt is initially hidden
        tutorialPrompt.gameObject.SetActive(false);

        // Set the black background to cover the screen
        blackBackground.color = Color.black;
    }

    public void PlayGame()
    {
        StartCoroutine(PlayGameSequence());
    }

    private IEnumerator PlayGameSequence()
    {
        // Fade out specific UI elements
        yield return FadeUIElements(0f, 0.5f);

        // Activate and run the tutorial prompt
        tutorialPrompt.gameObject.SetActive(true);
        yield return new WaitUntil(() => tutorialPrompt.IsComplete);

        // Load the game scene
        SceneManager.LoadScene(m_GameScene, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    private IEnumerator FadeUIElements(float endValue, float duration)
    {
        float startValue = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float currentAlpha = Mathf.Lerp(startValue, endValue, t);

            // Fade the buttons
            playButton.image.color = new Color(playButton.image.color.r, playButton.image.color.g, playButton.image.color.b, currentAlpha);
            exitButton.image.color = new Color(exitButton.image.color.r, exitButton.image.color.g, exitButton.image.color.b, currentAlpha);

            // Fade the background image
            backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, currentAlpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final alpha is set
        playButton.image.color = new Color(playButton.image.color.r, playButton.image.color.g, playButton.image.color.b, endValue);
        exitButton.image.color = new Color(exitButton.image.color.r, exitButton.image.color.g, exitButton.image.color.b, endValue);
        backgroundImage.color = new Color(backgroundImage.color.r, backgroundImage.color.g, backgroundImage.color.b, endValue);

        // Disable the buttons after fading
        playButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
    }
}