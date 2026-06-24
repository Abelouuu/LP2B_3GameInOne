using UnityEngine;
using System.Collections;

public class GameOverDeathAnim : MonoBehaviour
{
    [Header("Game Over Animation")]
    // Durée pendant laquelle le joueur tremble avant l'explosion
    [SerializeField] private float shakeDuration = 3f;

    // Intensité du tremblement du joueur
    [SerializeField] private float shakeStrength = 0.08f;

    [Header("Player")]
    // Référence vers le joueur
    [SerializeField] private GameObject player;

    // Particules du joueur, désactivées au moment de sa disparition
    [SerializeField] private GameObject playerParticles;

    [Header("Explosion")]
    // Préfab de l'explosion affichée à la mort du joueur
    [SerializeField] private GameObject explosionPrefab;

    // Taille de l'explosion
    [SerializeField] private float explosionScale = 1f;

    // Temps avant de supprimer l'explosion
    [SerializeField] private float explosionDestroyDelay = 1.5f;

    [Header("Audio")]
    // Son joué pendant le tremblement avant la mort
    [SerializeField] private AudioClip urgencySound;

    // Son de l'explosion finale
    [SerializeField] private AudioClip gameOverBoom;

    // Cri ou son de mort du joueur
    [SerializeField] private AudioClip deathCry;

    private IEnumerator ShakePlayer()
    {
        // Lance un son d'urgence pendant que le joueur tremble
        AudioManager.Instance.PlayGameOverMusic(urgencySound);

        Vector3 startPosition = player.transform.position;
        float timer = 0f;

        // Fait trembler le joueur autour de sa position de départ
        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;

            Vector2 randomOffset = Random.insideUnitCircle * shakeStrength;
            player.transform.position = startPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);

            yield return null;
        }

        // Replace le joueur exactement à sa position initiale
        player.transform.position = startPosition;

        // Arrête le son d'urgence
        AudioManager.Instance.StopGameOverAudio();
    }

    private void SpawnExplosion()
    {
        // Sécurité si aucun préfab d'explosion n'est assigné
        if (explosionPrefab == null)
            return;

        // Crée l'explosion à la position du joueur
        GameObject explosion = Instantiate(explosionPrefab, player.transform.position, Quaternion.identity);

        // Joue le son de l'explosion
        AudioManager.Instance.PlayGameOverSFX(gameOverBoom);

        // Ajuste la taille de l'explosion puis la détruit après un délai
        explosion.transform.localScale = Vector3.one * explosionScale;
        Destroy(explosion, explosionDestroyDelay);
    }

    private void DeathCry()
    {
        // Joue le son de mort du joueur
        AudioManager.Instance.PlayGameOverMusic(deathCry);
    }

    private void HidePlayer()
    {
        // Désactive certains scripts du joueur après sa mort
        GlowAnim glow = player.GetComponent<GlowAnim>();
        BombManager bomb = player.GetComponent<BombManager>();

        if (glow != null) glow.enabled = false;
        if (bomb != null) bomb.enabled = false;

        // Cache tous les sprites du joueur et de ses enfants
        foreach (SpriteRenderer sr in player.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.enabled = false;
        }

        // Désactive les collisions du joueur
        foreach (Collider2D col in player.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        // Désactive les particules du joueur si elles existent
        if (playerParticles != null)
        {
            playerParticles.SetActive(false);
        }
    }
}