using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Son d'explosion")]
    // Son joué quand l'explosion apparaît
    public AudioClip explosionSound;

    [Header("Réglage volume")]
    // Volume de base de l'explosion
    public float baseVolume = 0.5f;

    // Augmentation du volume en fonction de la taille de l'explosion
    public float volumeMultiplier = 0.2f;

    private void Start()
    {
        // On récupère la taille de l'explosion
        float scale = transform.localScale.x;

        // Le volume dépend de la taille : plus l'explosion est grande, plus le son est fort
        float volume = baseVolume + scale * volumeMultiplier;

        // On limite le volume entre 0 et 1 pour éviter une valeur trop élevée
        volume = Mathf.Clamp(volume, 0f, 1f);

        // Joue le son de l'explosion avec le volume calculé
        AudioManager.Instance.PlaySFX(explosionSound, volume);
    }
}