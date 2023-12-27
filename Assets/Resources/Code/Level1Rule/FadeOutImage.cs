using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutImage : MonoBehaviour
{
    public float fadeOutTime = 1f;

    private Image imageComponent;

    private void Start()
    {
        imageComponent = GetComponent<Image>();

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        if (imageComponent == null)
        {
            Debug.LogWarning("No Image component found for fading out.");
            yield break;
        }

        Color startColor = imageComponent.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeOutTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutTime);

            imageComponent.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        imageComponent.color = targetColor;

        gameObject.SetActive(false);
    }
}
