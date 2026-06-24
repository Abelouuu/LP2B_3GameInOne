using UnityEngine;
using System.Collections;

public class BonusSpawner : MonoBehaviour
{
    [Header("Bonus Prefabs")]
    // Préfabs des différents bonus qui peuvent apparaître
    public GameObject healthBonusPrefab;
    public GameObject bulletBonusPrefab;
    public GameObject shieldBonusPrefab;
    public GameObject speedBonusPrefab;
    public GameObject bombBonusPrefab;

    [Header("Spawn Position")]
    // Position X où les bonus apparaissent, généralement à droite de l'écran
    public float spawnX = 11f;

    // Limites verticales pour faire apparaître les bonus à des hauteurs différentes
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Spawn Delay")]
    // Délais de départ minimum et maximum entre deux apparitions de bonus
    public float startMinDelay = 8f;
    public float startMaxDelay = 15f;

    [Header("Bonus Speed")]
    // Vitesse aléatoire donnée aux bonus après leur apparition
    public float minSpeed = 2f;
    public float maxSpeed = 5f;

    [Header("Difficulty")]
    // Durée au bout de laquelle la difficulté atteint son maximum
    public float difficultyIncreaseDuration = 120f;

    // Multiplicateur minimum du délai : plus il est bas, plus les bonus apparaissent souvent
    public float minDelayMultiplier = 0.5f;

    // Temps auquel le spawner démarre, utilisé pour calculer la progression de difficulté
    private float startTime;

    private void Start()
    {
        // On enregistre le temps de départ
        startTime = Time.time;

        // On lance la boucle d'apparition des bonus
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Calcule le délai actuel avant le prochain bonus
            float delay = GetCurrentDelay();

            yield return new WaitForSeconds(delay);

            // Fait apparaître un bonus
            SpawnBonus();
        }
    }

    private float GetCurrentDelay()
    {
        // Temps écoulé depuis le début de la partie
        float elapsed = Time.time - startTime;

        // Ratio entre 0 et 1 qui représente l'avancement de la difficulté
        float difficultyRatio = Mathf.Clamp01(elapsed / difficultyIncreaseDuration);

        // Réduit progressivement le délai entre les bonus
        float multiplier = Mathf.Lerp(1f, minDelayMultiplier, difficultyRatio);

        float minDelay = startMinDelay * multiplier;
        float maxDelay = startMaxDelay * multiplier;

        // Retourne un délai aléatoire entre les deux valeurs calculées
        return Random.Range(minDelay, maxDelay);
    }

    private void SpawnBonus()
    {
        // Choisit un bonus aléatoire
        GameObject prefab = GetRandomBonusPrefab();

        // Sécurité si aucun préfab n'est assigné
        if (prefab == null)
            return;

        // Position aléatoire en Y pour varier l'apparition des bonus
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0f);

        // Création du bonus dans la scène
        GameObject bonus = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // On récupère son script de déplacement
        BonusMovement movement = bonus.GetComponent<BonusMovement>();

        if (movement != null)
        {
            // On donne une vitesse aléatoire au bonus
            float randomSpeed = Random.Range(minSpeed, maxSpeed);
            movement.SetSpeed(randomSpeed);
        }
    }

    private GameObject GetRandomBonusPrefab()
    {
        // Tableau contenant tous les types de bonus possibles
        GameObject[] bonusPrefabs =
        {
            healthBonusPrefab,
            bulletBonusPrefab,
            shieldBonusPrefab,
            speedBonusPrefab,
            bombBonusPrefab
        };

        // Choisit un bonus au hasard dans le tableau
        int randomIndex = Random.Range(0, bonusPrefabs.Length);
        return bonusPrefabs[randomIndex];
    }
}