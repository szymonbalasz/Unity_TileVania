using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDisplayText : MonoBehaviour
{
    [SerializeField] Text tutorialText = default;
    [SerializeField] float fadeInSpeed = 1f;
    [SerializeField] float fadeOutSpeed = 1f;

    void Start()
    {
        tutorialText.color = new Color(tutorialText.color.r, tutorialText.color.g, tutorialText.color.b, 0);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(FadeInText(tutorialText));
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopCoroutine(FadeInText(tutorialText));
        StartCoroutine(FadeOutText(tutorialText));
    }

    private IEnumerator FadeInText(Text c)
    {
        while (c.color.a < 1.0f)
        {
            c.color = new Color(c.color.r, c.color.g, c.color.b, c.color.a + (Time.deltaTime / fadeInSpeed));
            yield return null;
        }
    }

    private IEnumerator FadeOutText(Text c)
    {
        while (c.color.a > 0f)
        {
            c.color = new Color(c.color.r, c.color.g, c.color.b, c.color.a - (Time.deltaTime / fadeOutSpeed));
            yield return null;
        }
    }
}
