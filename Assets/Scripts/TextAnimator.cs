using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class TextAnimator : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private float typeSpeed = 0.05f;
    [SerializeField] private float delayBeforeFade = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    
    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource typingSoundSource;
    [SerializeField] private float minPitchVariation = 0.9f;
    [SerializeField] private float maxPitchVariation = 1.1f;

    [Header("Alert Settings")]
    [SerializeField] private TMP_Text alert;

    private void Start()
    {
        // Ensure text starts invisible
        // if (textComponent != null)
        // {
        //     textComponent.alpha = 0;
        // }
    }

    public void AnimateText(string textToShow)
    {
        // Stop any existing animations
        StopAllCoroutines();
        
        // Start the sequence
        StartCoroutine(TextSequence(textToShow));
    }

    public void AnimateAlert(string textToShow){
        StopAllCoroutines();
        StartCoroutine(Alert(textToShow));
    }

    private IEnumerator Alert(string textToShow){
        alert.text = textToShow;
        alert.alpha = 1;
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(FadeOutText(alert));
    }

    private IEnumerator TextSequence(string textToShow)
    {
        // Reset text and make it visible
        textComponent.text = "";
        textComponent.alpha = 1;

        // Type out the text
        yield return StartCoroutine(TypeText(textToShow));

        // Wait before fading
        yield return new WaitForSeconds(delayBeforeFade);

        // Fade out
        yield return StartCoroutine(FadeOutText(textComponent));
    }

    private IEnumerator TypeText(string textToShow)
    {
        foreach (char c in textToShow)
        {
            textComponent.text += c;
            
            // Play typing sound with pitch variation if audio source is assigned
            if (typingSoundSource != null)
            {
                typingSoundSource.pitch = Random.Range(minPitchVariation, maxPitchVariation);
                typingSoundSource.Play();
            }

            // Skip delay for spaces
            if (c != ' ')
            {
                yield return new WaitForSeconds(typeSpeed);
            }
        }
    }

    private IEnumerator FadeOutText(TMP_Text targetTextComponent)
    {
        float elapsedTime = 0;
        float startAlpha = targetTextComponent.alpha;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeOutDuration);
            targetTextComponent.alpha = newAlpha;
            yield return null;
        }

        // Ensure text is completely invisible at the end
        targetTextComponent.alpha = 0;
    }

    // Optional: Method to skip the current animation
    public void SkipAnimation()
    {
        StopAllCoroutines();
        if (textComponent != null)
        {
            textComponent.alpha = 0;
        }
    }
}