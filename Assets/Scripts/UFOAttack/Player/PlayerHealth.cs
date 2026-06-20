using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public RectTransform[] healthSegments;
    private SpriteRenderer[] spriteRenderers;
    public PlayerShield playerShield;
    [SerializeField] private AudioClip damageSound;

    [Range(0.0f, 1f)]
    public float volume = 1f;

    private int currentHealth;

    public float animationSpeed = 4f;

    private Coroutine[] segmentAnimations;

    private bool isInvicible = false;

    void Start()
    {
        currentHealth = healthSegments.Length;
        segmentAnimations = new Coroutine[healthSegments.Length];

        // Collecting all the threads to make the object disappear completely (invincibility animation)
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (!isInvicible)
        {
            if (playerShield != null && playerShield.IsActive)
            {
                playerShield.TakeShieldDamage(damage);
                return;
            }

            AudioManager.Instance.PlaySFX(damageSound, volume);
            for (int i = 0; i < damage; i++)
            {
                if (currentHealth > 0)
                {
                    currentHealth--;

                    StartSegmentAnimation(currentHealth, ShrinkSegment(healthSegments[currentHealth]));
                }
            }
            if(currentHealth <= 0)
            {
                GameStateManager.Instance.TriggerGameOver();
            }
            else
            {
                StartCoroutine(InvincibilityCooldown());
            }
            
        }
    }

    private IEnumerator InvincibilityCooldown()
    {
        float duration = 1f;
        float blinkInterval = 0.1f;

        isInvicible = true;

        float timer = 0f;

        while (timer < duration)
        {
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.enabled = !sr.enabled;
            }

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.enabled = true;
        }

        isInvicible = false;
    }

    public void Heal(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (currentHealth < healthSegments.Length)
            {
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
        // Stop any existing animation on this segment before starting a new one
        if (segmentAnimations[index] != null)
        {
            StopCoroutine(segmentAnimations[index]);
        }

        segmentAnimations[index] = StartCoroutine(animation);
    }

    private IEnumerator ShrinkSegment(RectTransform segment)
    {
        Vector3 scale = segment.localScale;

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