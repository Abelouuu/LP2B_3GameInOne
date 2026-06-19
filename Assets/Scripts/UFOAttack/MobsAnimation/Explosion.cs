using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Son d'explosion")]
    public AudioClip explosionSound;

    [Header("Réglage volume")]
    public float baseVolume = 0.5f;
    public float volumeMultiplier = 0.2f;

    private void Start()
    {
        float scale = transform.localScale.x;

        float volume = baseVolume + scale * volumeMultiplier;
        volume = Mathf.Clamp(volume, 0f, 1f);

        AudioManager.Instance.PlaySFX(explosionSound, volume);
    }
}