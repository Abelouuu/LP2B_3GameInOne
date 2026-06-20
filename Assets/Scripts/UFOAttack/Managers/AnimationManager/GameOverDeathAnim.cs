using UnityEngine;
using System.Collections;

public class GameOverDeathAnim : MonoBehaviour
{
    [Header("Game Over Animation")]
    [SerializeField] private float shakeDuration = 3f;
    [SerializeField] private float shakeStrength = 0.08f;

    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerParticles;

    [Header("Explosion")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionScale = 1f;
    [SerializeField] private float explosionDestroyDelay = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip urgencySound;
    [SerializeField] private AudioClip gameOverBoom;
    [SerializeField] private AudioClip deathCry;

    private IEnumerator ShakePlayer()
    {
        AudioManager.Instance.PlayGameOverMusic(urgencySound);
        Vector3 startPosition = player.transform.position;
        float timer = 0f;

        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;

            Vector2 randomOffset = Random.insideUnitCircle * shakeStrength;
            player.transform.position = startPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);

            yield return null;
        }

        player.transform.position = startPosition;
        AudioManager.Instance.StopGameOverAudio();
    }

    private void SpawnExplosion()
    {
        if (explosionPrefab == null)
            return;

        GameObject explosion = Instantiate(explosionPrefab, player.transform.position, Quaternion.identity);
        AudioManager.Instance.PlayGameOverSFX(gameOverBoom);

        explosion.transform.localScale = Vector3.one * explosionScale;
        Destroy(explosion, explosionDestroyDelay);
    }
    private void DeathCry()
    {
        AudioManager.Instance.PlayGameOverMusic(deathCry);
    }

    private void HidePlayer()
    {
        GlowAnim glow = player.GetComponent<GlowAnim>();
        BombManager bomb = player.GetComponent<BombManager>();

        if (glow != null) glow.enabled = false;
        if (bomb != null) bomb.enabled = false;

        foreach (SpriteRenderer sr in player.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = false;
        }

        foreach (Collider2D col in player.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        if (playerParticles != null)
        {
            playerParticles.SetActive(false);
        }
    }
}
