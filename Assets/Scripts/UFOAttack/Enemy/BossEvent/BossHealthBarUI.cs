using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI bossNameText;

    [Header("Fade")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.8f;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        HideInstantly();
    }

    public void SetBossName(string bossName)
    {
        if (bossNameText != null)
        {
            bossNameText.text = bossName;
        }
    }

    public void SetMaxHealth(int health)
    {
        if (slider == null)
            return;

        slider.minValue = 0;
        slider.maxValue = health;
        slider.value = health;
        slider.wholeNumbers = true;
        slider.interactable = false;
    }

    public void SetHealth(int health)
    {
        if (slider == null)
            return;

        slider.value = health;
    }

    public void Show(string bossName, int maxHealth)
    {
        gameObject.SetActive(true);

        SetBossName(bossName);
        SetMaxHealth(maxHealth);

        StartFade(1f, fadeInDuration);
    }

    public void Hide()
    {
        StartFade(0f, fadeOutDuration, true);
    }

    private void HideInstantly()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        gameObject.SetActive(false);
    }

    private void StartFade(float targetAlpha, float duration, bool disableAfterFade = false)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha, duration, disableAfterFade));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration, bool disableAfterFade)
    {
        if (canvasGroup == null)
            yield break;

        float startAlpha = canvasGroup.alpha;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float ratio = timer / duration;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, ratio);

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;

        if (disableAfterFade)
        {
            gameObject.SetActive(false);
        }

        fadeCoroutine = null;
    }
}