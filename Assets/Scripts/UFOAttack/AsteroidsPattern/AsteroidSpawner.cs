using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Asteroid Lists")]
    // Listes des différents préfabs d'astéroïdes selon leur taille
    public GameObject[] littleAsteroids;
    public GameObject[] midAsteroids;
    public GameObject[] bigAsteroids;

    [Header("Spawners")]
    // Point de spawn principal pour les petits et moyens astéroïdes
    public Transform spawner;

    // Points de spawn utilisés pour les gros astéroïdes
    public Transform[] bigSpawners;

    // Décalage vertical possible autour du spawner
    public float spawnGap;
    public float bigSpawnGap;

    // Temps moyen entre deux apparitions selon la taille
    public float avgBigInterval;
    public float avgMidInterval;
    public float avgLittleInterval;

    [Header("Speed Range")]
    // Vitesse de rotation aléatoire des astéroïdes
    public float minRotationSpeed = 3f;
    public float maxRotationSpeed = 15f;

    // Vitesses possibles pour les petits astéroïdes
    public float minLittleSpeed = 3f;
    public float maxLittleSpeed = 8f;

    // Vitesses possibles pour les astéroïdes moyens
    public float minMidSpeed = 2f;
    public float maxMidSpeed = 4f;

    // Vitesses possibles pour les gros astéroïdes
    public float minBigSpeed = 0.5f;
    public float maxBigSpeed = 0.7f;

    void Start()
    {
        // Lance un spawner séparé pour chaque point de spawn des gros astéroïdes
        for (int i = 0; i < bigSpawners.Length; i++)
        {
            StartCoroutine(BigSpawner(bigSpawners[i]));
        }

        // Lance les spawners des astéroïdes moyens et petits
        StartCoroutine(MidSpawner());
        StartCoroutine(LittleSpawner());
    }

    void SpawnAsteroid(GameObject[] asteroidArray, Transform spawnPoint, float gap, float minSpeed, float maxSpeed)
    {
        // Choisit aléatoirement un astéroïde dans la liste donnée
        int index = Random.Range(0, asteroidArray.Length);

        // Ajoute un petit décalage vertical aléatoire pour varier les positions
        float yGap = Random.Range(-gap, gap);

        Vector3 spawnPosition = spawnPoint.position + new Vector3(0f, yGap, 0f);

        // Crée l'astéroïde à la position choisie
        GameObject asteroid = Instantiate(asteroidArray[index], spawnPosition, Quaternion.identity, transform);

        // Récupère le script de mouvement de l'astéroïde
        AsteroidMovement movement = asteroid.GetComponent<AsteroidMovement>();

        // Donne des vitesses aléatoires à l'astéroïde
        movement.rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        movement.movementSpeed = Random.Range(minSpeed, maxSpeed);
    }

    IEnumerator BigSpawner(Transform spawn)
    {
        while (true)
        {
            // Fait apparaître un gros astéroïde
            SpawnAsteroid(bigAsteroids, spawn, bigSpawnGap, minBigSpeed, maxBigSpeed);

            // Attend un délai légèrement aléatoire avant le prochain
            yield return new WaitForSeconds(avgBigInterval + Random.Range(-2f, 2f));
        }
    }

    IEnumerator MidSpawner()
    {
        while (true)
        {
            // Fait apparaître un astéroïde moyen
            SpawnAsteroid(midAsteroids, spawner, spawnGap, minMidSpeed, maxMidSpeed);

            // Le délai varie autour de l'intervalle moyen
            yield return new WaitForSeconds(avgMidInterval + Random.Range(-avgMidInterval, avgMidInterval));
        }
    }

    IEnumerator LittleSpawner()
    {
        while (true)
        {
            // Fait apparaître un petit astéroïde
            SpawnAsteroid(littleAsteroids, spawner, spawnGap, minLittleSpeed, maxLittleSpeed);

            // Le délai varie autour de l'intervalle moyen
            yield return new WaitForSeconds(avgLittleInterval + Random.Range(-avgLittleInterval, avgLittleInterval));
        }
    }
}