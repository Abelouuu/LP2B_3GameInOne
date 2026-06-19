using UnityEngine;
using System.Collections;

public class BonusSpawner : MonoBehaviour
{
    [Header("Bonus Prefabs")]
    public GameObject healthBonusPrefab;
    public GameObject bulletBonusPrefab;
    public GameObject shieldBonusPrefab;
    public GameObject speedBonusPrefab;
    public GameObject bombBonusPrefab;

    [Header("Spawn Position")]
    public float spawnX = 11f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Spawn Delay")]
    public float startMinDelay = 8f;
    public float startMaxDelay = 15f;

    [Header("Bonus Speed")]
    public float minSpeed = 2f;
    public float maxSpeed = 5f;

    [Header("Difficulty")]
    public float difficultyIncreaseDuration = 120f;
    public float minDelayMultiplier = 0.5f;

    private float startTime;

    private void Start()
    {
        startTime = Time.time;
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            float delay = GetCurrentDelay();
            yield return new WaitForSeconds(delay);

            SpawnBonus();
        }
    }

    private float GetCurrentDelay()
    {
        float elapsed = Time.time - startTime;
        float difficultyRatio = Mathf.Clamp01(elapsed / difficultyIncreaseDuration);

        float multiplier = Mathf.Lerp(1f, minDelayMultiplier, difficultyRatio);

        float minDelay = startMinDelay * multiplier;
        float maxDelay = startMaxDelay * multiplier;

        return Random.Range(minDelay, maxDelay);
    }

    private void SpawnBonus()
    {
        GameObject prefab = GetRandomBonusPrefab();

        if (prefab == null)
            return;

        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0f);

        GameObject bonus = Instantiate(prefab, spawnPosition, Quaternion.identity);

        BonusMovement movement = bonus.GetComponent<BonusMovement>();

        if (movement != null)
        {
            float randomSpeed = Random.Range(minSpeed, maxSpeed);
            movement.SetSpeed(randomSpeed);
        }
    }

    private GameObject GetRandomBonusPrefab()
    {
        GameObject[] bonusPrefabs =
        {
            healthBonusPrefab,
            bulletBonusPrefab,
            shieldBonusPrefab,
            speedBonusPrefab,
            bombBonusPrefab
        };

        int randomIndex = Random.Range(0, bonusPrefabs.Length);
        return bonusPrefabs[randomIndex];
    }
}