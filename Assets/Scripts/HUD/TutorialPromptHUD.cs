using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class TutorialPromptUI : MonoBehaviour
{
    [SerializeField] private List<Image> lineImages;
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.3f;

    private int currentIndex = -1;
    private bool isComplete = false;

    private void Start()
    {
        foreach (var image in lineImages)
        {
            image.color = new Color(1f, 1f, 1f, 0f);
        }

        ShowNextLine();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isComplete)
        {
            ShowNextLine();
        }
    }

    private void ShowNextLine()
    {
        currentIndex++;

        if (currentIndex >= lineImages.Count)
        {
            HideLastImage();
            return;
        }

        if (currentIndex > 0)
        {
            Image previousImage = lineImages[currentIndex - 1];
            previousImage.DOFade(0f, fadeOutDuration).SetEase(Ease.InCubic);
        }

        Image currentImage = lineImages[currentIndex];
        currentImage.DOFade(1f, fadeInDuration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() => {
            });
    }

    private void HideLastImage()
    {
        if (lineImages.Count > 0)
        {
            Image lastImage = lineImages[lineImages.Count - 1];
            lastImage.DOFade(0f, fadeOutDuration)
                .SetEase(Ease.InCubic)
                .OnComplete(() => {
                    isComplete = true;
                });
        }
    }
}