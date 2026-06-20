using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject ramPrefab;
    public GameObject classicPrefab;
    public GameObject spliterPrefab;
    public GameObject trackerPrefab;
    public GameObject laserShooterPrefab;

    public GameObject BossPrefab;

    [Header("References")]
    public GameObject playerObject;

    [Header("Spawn Position")]
    public float spawnX = 11f;

    [Header("Difficulty")]
    public float difficultyIncreaseDuration = 120f;
    public float minDelayMultiplier = 0.35f;

    [Header("Ram")]
    public float ramStartMinDelay = 3f;
    public float ramStartMaxDelay = 5f;

    [Header("Classic")]
    public float classicStartMinDelay = 6f;
    public float classicStartMaxDelay = 9f;

    [Header("Spliter")]
    public float spliterStartMinDelay = 12f;
    public float spliterStartMaxDelay = 18f;

    [Header("Tracker")]
    public float trackerStartMinDelay = 12f;
    public float trackerStartMaxDelay = 18f;

    [Header("Laser Shooter")]
    public float laserStartMinDelay = 12f;
    public float laserStartMaxDelay = 18f;

    [Header("Boss")]
    public float bossStartDelay = 20f;
    public float bossStartMinDelay = 25f;
    public float bossStartMaxDelay = 40f;

    private float startTime;

    private bool canSpawn = true;

    void Start()
    {
        startTime = Time.time;

        StartCoroutine(SpawnLoop(ramPrefab, -4.4f, 3f, ramStartMinDelay, ramStartMaxDelay));
        StartCoroutine(SpawnLoop(classicPrefab, -3.9f, 2.6f, classicStartMinDelay, classicStartMaxDelay));
        StartCoroutine(SpawnLoop(spliterPrefab, -2.2f, 1.2f, spliterStartMinDelay, spliterStartMaxDelay));
        StartCoroutine(SpawnLoop(trackerPrefab, -2.8f, 1.8f, trackerStartMinDelay, trackerStartMaxDelay));
        StartCoroutine(SpawnLoop(laserShooterPrefab, -4.2f, 2.7f, laserStartMinDelay, laserStartMaxDelay));
        StartCoroutine(BossSpawnLoop());
    }

    public void SetSpawning(bool value)
    {
        canSpawn = value;
    }

    IEnumerator BossSpawnLoop()
    {
        yield return StartCoroutine(PauseAwareDelay(bossStartDelay));

        while (true)
        {
            yield return StartCoroutine(WaitUntilSpawningEnabled());

            SpawnEnemy(BossPrefab, 0f, 0f);

            float delay = GetCurrentDelay(bossStartMinDelay, bossStartMaxDelay);
            yield return StartCoroutine(PauseAwareDelay(delay));
        }
    }

    IEnumerator SpawnLoop(GameObject prefab, float minY, float maxY, float startMinDelay, float startMaxDelay)
    {
        while (true)
        {
            float delay = GetCurrentDelay(startMinDelay, startMaxDelay);

            yield return StartCoroutine(PauseAwareDelay(delay));
            yield return StartCoroutine(WaitUntilSpawningEnabled());

            SpawnEnemy(prefab, minY, maxY);
        }
    }

    private IEnumerator WaitUntilSpawningEnabled()
    {
        while (!canSpawn)
        {
            yield return null;
        }
    }

    private IEnumerator PauseAwareDelay(float duration)
    {
        float timer = 0f;

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
        float elapsed = Time.time - startTime;

        float difficultyRatio = Mathf.Clamp01(elapsed / difficultyIncreaseDuration);

        float multiplier = Mathf.Lerp(1f, minDelayMultiplier, difficultyRatio);

        float minDelay = startMinDelay * multiplier;
        float maxDelay = startMaxDelay * multiplier;

        return Random.Range(minDelay, maxDelay);
    }

    void SpawnEnemy(GameObject prefab, float minY, float maxY)
    {
        if (prefab == null)
            return;

        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0f);

        GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);

        OrangeEnemy orangeEnemy = enemy.GetComponent<OrangeEnemy>();
        if (orangeEnemy != null)
        {
            orangeEnemy.playerObject = playerObject;
        }

        TeleportEnemy boss = enemy.GetComponent<TeleportEnemy>();
        if (boss != null)
        {
            boss.playerObject = playerObject;
        }
    }
}