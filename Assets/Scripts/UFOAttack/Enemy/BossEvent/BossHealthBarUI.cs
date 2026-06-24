using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    [Header("References")]
    // CanvasGroup utilisé pour faire apparaître/disparaître la barre avec un fade
    [SerializeField] private CanvasGroup canvasGroup;

    // Slider qui représente visuellement la vie du boss
    [SerializeField] private Slider slider;

    // Texte affichant le nom du boss
    [SerializeField] private TextMeshProUGUI bossNameText;

    [Header("Fade")]
    // Durée de l'apparition progressive de la barre
    [SerializeField] private float fadeInDuration = 0.5f;

    // Durée de la disparition progressive de la barre
    [SerializeField] private float fadeOutDuration = 0.8f;

    // Coroutine utilisée pour gérer le fade en cours
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // Au lancement, la barre est cachée
        HideInstantly();
    }

    public void SetBossName(string bossName)
    {
        // Met à jour le nom du boss affiché
        if (bossNameText != null)
        {
            bossNameText.text = bossName;
        }
    }

    public void SetMaxHealth(int health)
    {
        // Vérifie que le slider existe avant de le modifier
        if (slider == null)
            return;

        // Configure les valeurs du slider selon la vie maximale du boss
        slider.minValue = 0;
        slider.maxValue = health;
        slider.value = health;

        // La vie est affichée avec des valeurs entières
        slider.wholeNumbers = true;

        // Le joueur ne peut pas interagir avec la barre
        slider.interactable = false;
    }

    public void SetHealth(int health)
    {
        // Met à jour la valeur actuelle de la vie du boss
        if (slider == null)
            return;

        slider.value = health;
    }

    public void Show(string bossName, int maxHealth)
    {
        // Active l'objet UI avant de l'afficher
        gameObject.SetActive(true);

        // Initialise le nom et la vie maximale du boss
        SetBossName(bossName);
        SetMaxHealth(maxHealth);

        // Fait apparaître progressivement la barre
        StartFade(1f, fadeInDuration);
    }

    public void Hide()
    {
        // Fait disparaître progressivement la barre
        StartFade(0f, fadeOutDuration, true);
    }

    private void HideInstantly()
    {
        // Cache directement la barre sans animation
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
        // Si un fade est déjà en cours, on l'arrête pour éviter les conflits
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Lance une nouvelle transition vers l'alpha demandé
        fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha, duration, disableAfterFade));
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration, bool disableAfterFade)
    {
        // Si le CanvasGroup n'est pas assigné, on arrête la coroutine
        if (canvasGroup == null)
            yield break;

        float startAlpha = canvasGroup.alpha;
        float timer = 0f;

        // Change progressivement la transparence de la barre
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float ratio = timer / duration;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, ratio);

            yield return null;
        }

        // On force la valeur finale pour éviter les petites imprécisions
        canvasGroup.alpha = targetAlpha;

        // Si demandé, on désactive l'objet après la disparition
        if (disableAfterFade)
        {
            gameObject.SetActive(false);
        }

        fadeCoroutine = null;
    }
}