using UnityEngine;
using System.Collections;

// Classe de base abstraite pour tous les ennemis
// Elle permet de regrouper les éléments communs à plusieurs ennemis
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Audio")]
    [Range(0f, 1f)]
    public float shootVolume = 0.5f; // Volume utilisé pour les sons des ennemis

    protected virtual void Start()
    {
        // Au lancement de l'ennemi, on démarre automatiquement son comportement
        StartCoroutine(Behavior());
    }

    // Chaque ennemi devra définir son propre comportement
    protected abstract IEnumerator Behavior();

    protected void PlaySound(AudioClip clip)
    {
        // Joue un son seulement si l'AudioManager existe et si le son est bien assigné
        if (AudioManager.Instance != null && clip != null)
        {
            AudioManager.Instance.PlaySFX(clip, shootVolume);
        }
    }

    protected void DestroySelf()
    {
        // Détruit l'ennemi actuel
        Destroy(gameObject);
    }
}