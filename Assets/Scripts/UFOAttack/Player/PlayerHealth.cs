using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    // Segments de vie affichés dans l'interface
    public RectTransform[] healthSegments;

    // SpriteRenderers du joueur, utilisés pour l'effet de clignotement
    private SpriteRenderer[] spriteRenderers;

    // Référence vers le bouclier du joueur
    public PlayerShield playerShield;

    // Son joué quand le joueur prend des dégâts
    [SerializeField] private AudioClip damageSound;

    [Range(0.0f, 1f)]
    public float volume = 1f;

    // Vie actuelle du joueur
    private int currentHealth;

    // Vitesse des animations des segments de vie
    public float animationSpeed = 4f;

    // Tableau qui stocke les animations en cours pour chaque segment
    private Coroutine[] segmentAnimations;

    // Empêche le joueur de reprendre des dégâts juste après avoir été touché
    private bool isInvicible = false;

    void Start()
    {
        // La vie de départ correspond au nombre de segments de vie
        currentHealth = healthSegments.Length;

        // Une coroutine par segment pour pouvoir gérer les animations séparément
        segmentAnimations = new Coroutine[healthSegments.Length];

        // Récupère tous les SpriteRenderers du joueur et de ses enfants pour le clignotement
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        // Si le joueur est invincible temporairement, il ne prend pas de dégâts
        if (!isInvicible)
        {
            // Si le bouclier est actif, les dégâts sont absorbés par le bouclier
            if (playerShield != null && playerShield.IsActive)
            {
                playerShield.TakeShieldDamage(damage);
                return;
            }

            // Joue le son de dégâts
            AudioManager.Instance.PlaySFX(damageSound, volume);

            // Retire un segment de vie pour chaque point de dégât reçu
            for (int i = 0; i < damage; i++)
            {
                if (currentHealth > 0)
                {
                    currentHealth--;

                    // Lance l'animation de disparition du segment perdu
                    StartSegmentAnimation(currentHealth, ShrinkSegment(healthSegments[currentHealth]));
                }
            }

            // Si la vie tombe à 0, on déclenche le game over
            if (currentHealth <= 0)
            {
                GameStateManager.Instance.TriggerGameOver();
            }
            else
            {
                // Sinon, le joueur devient invincible pendant un court instant
                StartCoroutine(InvincibilityCooldown());
            }
        }
    }

    private IEnumerator InvincibilityCooldown()
    {
        float duration = 1f;
        float blinkInterval = 0.1f;

        // Début de l'invincibilité temporaire
        isInvicible = true;

        float timer = 0f;

        // Pendant la durée définie, le joueur clignote
        while (timer < duration)
        {
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.enabled = !sr.enabled;
            }

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // On réaffiche tous les sprites à la fin du clignotement
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = true;
        }

        isInvicible = false;
    }

    public void Heal(int amount)
    {
        // Récupère des points de vie sans dépasser le maximum
        for (int i = 0; i < amount; i++)
        {
            if (currentHealth < healthSegments.Length)
            {
                // Anime le retour du segment de vie
                StartSegmentAnimation(
                    currentHealth,
                    GrowSegment(healthSegments[currentHealth])
                );

                currentHealth++;
            }
        }
    }

    private void StartSegmentAnimation(int index, IEnumerator animation)
    {
        // Si une animation existe déjà sur ce segment, on l'arrête avant d'en lancer une nouvelle
        if (segmentAnimations[index] != null)
        {
            StopCoroutine(segmentAnimations[index]);
        }

        segmentAnimations[index] = StartCoroutine(animation);
    }

    private IEnumerator ShrinkSegment(RectTransform segment)
    {
        Vector3 scale = segment.localScale;

        // Réduit progressivement la taille du segment de vie
        while (scale.x > 1f)
        {
            scale.x -= animationSpeed * Time.deltaTime;
            scale.y -= animationSpeed * Time.deltaTime;

            scale.x = Mathf.Max(scale.x, 0f);
            scale.y = Mathf.Max(scale.y, 0f);

            segment.localScale = scale;

            yield return null;
        }
    }

    private IEnumerator GrowSegment(RectTransform segment)
    {
        Vector3 scale = segment.localScale;

        // Agrandit progressivement le segment de vie récupéré
        while (scale.x < 2f)
        {
            scale.x += animationSpeed * Time.deltaTime;
            scale.y += animationSpeed * Time.deltaTime;

            scale.x = Mathf.Min(scale.x, 2f);
            scale.y = Mathf.Min(scale.y, 2f);

            segment.localScale = scale;

            yield return null;
        }
    }
}