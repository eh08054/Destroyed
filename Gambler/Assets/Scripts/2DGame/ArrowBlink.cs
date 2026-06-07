using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class ArrowBlink : MonoBehaviour
{
    [SerializeField] private float fadeTime;
    private Image fadeImage;
    private Coroutine fadeInOutCoroutine;

    private void Awake()
    {
        fadeImage = GetComponent<Image>();
    }
    private void OnEnable()
    {
        fadeInOutCoroutine = StartCoroutine(FadeInOut());
    }
    private void OnDisable()
    {
        if (fadeInOutCoroutine != null)
        {
            StopCoroutine(fadeInOutCoroutine);
        }
    }
    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(1, 0));
            yield return StartCoroutine(Fade(0, 1));
        }
    }
    private IEnumerator Fade(float start, float end)
    {
        float current = 0;
        float percent = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / fadeTime;

            Color color = fadeImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            fadeImage.color = color;

            yield return null;
        }
    }
}
