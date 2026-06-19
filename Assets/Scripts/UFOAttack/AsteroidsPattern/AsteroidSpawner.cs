using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Asteroid Lists")]
    public GameObject[] littleAsteroids;
    public GameObject[] midAsteroids;
    public GameObject[] bigAsteroids;

    [Header("Spawners")]
    public Transform spawner;
    public Transform[] bigSpawners;

    public float spawnGap;
    public float bigSpawnGap;

    public float avgBigInterval;
    public float avgMidInterval;
    public float avgLittleInterval;

    [Header("Speed Range")]
    public float minRotationSpeed = 3f;
    public float maxRotationSpeed = 15f;

    public float minLittleSpeed = 3f;
    public float maxLittleSpeed = 8f;

    public float minMidSpeed = 2f;
    public float maxMidSpeed = 4f;

    public float minBigSpeed = 0.5f;
    public float maxBigSpeed = 0.7f;

    void Start()
    {
        for (int i = 0; i < bigSpawners.Length; i++)
        {
            StartCoroutine(BigSpawner(bigSpawners[i]));
        }

        StartCoroutine(MidSpawner());
        StartCoroutine(LittleSpawner());
    }

    void SpawnAsteroid(GameObject[] asteroidArray, Transform spawnPoint, float gap, float minSpeed, float maxSpeed)
    {
        int index = Random.Range(0, asteroidArray.Length);

        float yGap = Random.Range(-gap, gap);

        Vector3 spawnPosition = spawnPoint.position + new Vector3(0f, yGap, 0f);

        GameObject asteroid = Instantiate(asteroidArray[index], spawnPosition, Quaternion.identity, transform);

        // Récupération du script
        AsteroidMovement movement = asteroid.GetComponent<AsteroidMovement>();

        // Vitesses aléatoires
        movement.rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
        movement.movementSpeed = Random.Range(minSpeed, maxSpeed);
    }

    IEnumerator BigSpawner(Transform spawn)
    {
        while (true)
        {
            SpawnAsteroid(bigAsteroids, spawn, bigSpawnGap, minBigSpeed, maxBigSpeed);

            yield return new WaitForSeconds(avgBigInterval + Random.Range(-2f, 2f));
        }
    }

    IEnumerator MidSpawner()
    {
        while (true)
        {
            SpawnAsteroid(midAsteroids, spawner, spawnGap, minMidSpeed, maxMidSpeed);

            yield return new WaitForSeconds(avgMidInterval + Random.Range(-avgMidInterval, avgMidInterval));
        }
    }

    IEnumerator LittleSpawner()
    {
        while (true)
        {
            SpawnAsteroid(littleAsteroids, spawner, spawnGap, minLittleSpeed, maxLittleSpeed);

            yield return new WaitForSeconds(avgLittleInterval + Random.Range(-avgLittleInterval, avgLittleInterval));
        }
    }
}