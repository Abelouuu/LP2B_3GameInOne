using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    // Préfabs des différents ennemis qui peuvent apparaître
    public GameObject ramPrefab;
    public GameObject classicPrefab;
    public GameObject spliterPrefab;
    public GameObject trackerPrefab;
    public GameObject laserShooterPrefab;

    // Préfab du boss classique
    public GameObject BossPrefab;

    [Header("References")]
    // Référence vers le joueur, donnée aux ennemis qui doivent le viser
    public GameObject playerObject;

    [Header("Spawn Position")]
    // Position X où les ennemis apparaissent, généralement à droite de l'écran
    public float spawnX = 11f;

    [Header("Difficulty")]
    // Temps au bout duquel la difficulté atteint son niveau maximal
    public float difficultyIncreaseDuration = 120f;

    // Multiplicateur minimal des délais : plus il est faible, plus les ennemis apparaissent souvent
    public float minDelayMultiplier = 0.35f;

    [Header("Ram")]
    // Délais de départ pour les ennemis qui chargent
    public float ramStartMinDelay = 3f;
    public float ramStartMaxDelay = 5f;

    [Header("Classic")]
    // Délais de départ pour les ennemis classiques
    public float classicStartMinDelay = 6f;
    public float classicStartMaxDelay = 9f;

    [Header("Spliter")]
    // Délais de départ pour les ennemis qui tirent des missiles séparables
    public float spliterStartMinDelay = 12f;
    public float spliterStartMaxDelay = 18f;

    [Header("Tracker")]
    // Délais de départ pour les ennemis qui tirent des missiles à tête chercheuse
    public float trackerStartMinDelay = 12f;
    public float trackerStartMaxDelay = 18f;

    [Header("Laser Shooter")]
    // Délais de départ pour les ennemis qui tirent des lasers
    public float laserStartMinDelay = 12f;
    public float laserStartMaxDelay = 18f;

    [Header("Boss")]
    // Temps avant la première apparition du boss classique
    public float bossStartDelay = 20f;

    // Délais entre les apparitions suivantes du boss classique
    public float bossStartMinDelay = 25f;
    public float bossStartMaxDelay = 40f;

    // Temps de départ utilisé pour calculer la progression de difficulté
    private float startTime;

    // Permet d'activer ou désactiver temporairement l'apparition des ennemis
    private bool canSpawn = true;

    void Start()
    {
        // On mémorise le temps de début de partie
        startTime = Time.time;

        // Chaque type d'ennemi possède sa propre boucle d'apparition
        StartCoroutine(SpawnLoop(ramPrefab, -4.4f, 3f, ramStartMinDelay, ramStartMaxDelay));
        StartCoroutine(SpawnLoop(classicPrefab, -3.9f, 2.6f, classicStartMinDelay, classicStartMaxDelay));
        StartCoroutine(SpawnLoop(spliterPrefab, -2.2f, 1.2f, spliterStartMinDelay, spliterStartMaxDelay));
        StartCoroutine(SpawnLoop(trackerPrefab, -2.8f, 1.8f, trackerStartMinDelay, trackerStartMaxDelay));
        StartCoroutine(SpawnLoop(laserShooterPrefab, -4.2f, 2.7f, laserStartMinDelay, laserStartMaxDelay));

        // Boucle séparée pour le boss classique
        StartCoroutine(BossSpawnLoop());
    }

    public void SetSpawning(bool value)
    {
        // Permet à un autre script, comme BossEventManager, d'arrêter ou relancer les spawns
        canSpawn = value;
    }

    IEnumerator BossSpawnLoop()
    {
        // Attend avant de faire apparaître le premier boss classique
        yield return StartCoroutine(PauseAwareDelay(bossStartDelay));

        while (true)
        {
            // Attend que le spawn soit autorisé
            yield return StartCoroutine(WaitUntilSpawningEnabled());

            // Fait apparaître le boss à une position fixe en Y
            SpawnEnemy(BossPrefab, 0f, 0f);

            // Attend avant le prochain boss, avec un délai qui dépend de la difficulté
            float delay = GetCurrentDelay(bossStartMinDelay, bossStartMaxDelay);
            yield return StartCoroutine(PauseAwareDelay(delay));
        }
    }

    IEnumerator SpawnLoop(GameObject prefab, float minY, float maxY, float startMinDelay, float startMaxDelay)
    {
        while (true)
        {
            // Calcule le délai avant la prochaine apparition
            float delay = GetCurrentDelay(startMinDelay, startMaxDelay);

            // Attend le délai, mais seulement quand le spawn est autorisé
            yield return StartCoroutine(PauseAwareDelay(delay));

            // Si le spawn est désactivé, on attend qu'il soit réactivé
            yield return StartCoroutine(WaitUntilSpawningEnabled());

            // Fait apparaître l'ennemi
            SpawnEnemy(prefab, minY, maxY);
        }
    }

    private IEnumerator WaitUntilSpawningEnabled()
    {
        // Attend tant que l'apparition des ennemis est désactivée
        while (!canSpawn)
        {
            yield return null;
        }
    }

    private IEnumerator PauseAwareDelay(float duration)
    {
        float timer = 0f;

        // Le timer avance seulement si le spawn est actif
        while (timer < duration)
        {
            if (canSpawn)
            {
                timer += Time.deltaTime;
            }

            yield return null;
        }
    }

    float GetCurrentDelay(float startMinDelay, float startMaxDelay)
    {
        // Temps écoulé depuis le début
        float elapsed = Time.time - startTime;

        // Ratio entre 0 et 1 représentant la progression de la difficulté
        float difficultyRatio = Mathf.Clamp01(elapsed / difficultyIncreaseDuration);

        // Réduit progressivement les délais d'apparition
        float multiplier = Mathf.Lerp(1f, minDelayMultiplier, difficultyRatio);

        float minDelay = startMinDelay * multiplier;
        float maxDelay = startMaxDelay * multiplier;

        // Retourne un délai aléatoire entre les deux valeurs
        return Random.Range(minDelay, maxDelay);
    }

    void SpawnEnemy(GameObject prefab, float minY, float maxY)
    {
        // Sécurité si le préfab n'est pas assigné
        if (prefab == null)
            return;

        // Position verticale aléatoire entre les limites données
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0f);

        // Création de l'ennemi dans la scène
        GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Si l'ennemi est orange, on lui donne la référence du joueur
        OrangeEnemy orangeEnemy = enemy.GetComponent<OrangeEnemy>();
        if (orangeEnemy != null)
        {
            orangeEnemy.playerObject = playerObject;
        }

        // Si l'ennemi est un boss téléporteur, on lui donne aussi la référence du joueur
        TeleportEnemy boss = enemy.GetComponent<TeleportEnemy>();
        if (boss != null)
        {
            boss.playerObject = playerObject;
        }
    }
}