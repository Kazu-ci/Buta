using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class playguid : MonoBehaviour
{
    public static playguid instance;

    [Header("FadeSettings")]
    public Image fadeImage;
    public float fadeDuration = 0.5f;   //•\Ž¦‚·‚é‚Ü‚Å‚ÌŽžŠÔ

    [Header("image")]
    public Image title;
    public Image startButton;

    [Header("button")]
    public Button select;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonPressd()
    {
        StartCoroutine(ConfigFadeIn());
        
    }

    private IEnumerator ConfigFadeIn()
    {
        yield return StartCoroutine(FadeInC());
        yield return StartCoroutine(FadeOutTitle());
        yield return StartCoroutine(FadeOutSelect());
    }

    private IEnumerator FadeInC()
    {
        float t = 0;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0.0f, 0.5f, Mathf.Clamp01(t / fadeDuration));
            fadeImage.color = color;
            yield return null;
        }
    }
    private IEnumerator FadeOutTitle()
    {
        float t = fadeDuration;
        Color color = title.color;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            color.a = Mathf.Clamp01(t / fadeDuration);
            title.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOutSelect()
    {
        float t = fadeDuration;
        Color color = startButton.color;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            color.a = Mathf.Clamp01(t / fadeDuration);
            startButton.color = color;
            yield return null;
        }
        select.interactable = false;
    }
}
