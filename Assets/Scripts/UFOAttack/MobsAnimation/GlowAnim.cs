using UnityEngine;

public class GlowAnim : MonoBehaviour
{
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float spawnInterval = 0.08f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnGhost();
            timer = 0f;
        }
    }

    private void SpawnGhost()
    {
        Instantiate(ghostPrefab, transform.position, transform.rotation);
    }
}
