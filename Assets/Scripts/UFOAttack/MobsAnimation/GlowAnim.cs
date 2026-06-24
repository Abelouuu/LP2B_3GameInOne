using UnityEngine;

public class GlowAnim : MonoBehaviour
{
    // Préfab du fantôme créé derrière le joueur
    [SerializeField] private GameObject ghostPrefab;

    // Temps entre deux apparitions de fantômes
    [SerializeField] private float spawnInterval = 0.08f;

    // Timer utilisé pour compter le temps entre deux créations
    private float timer;

    private void Update()
    {
        // On augmente le timer à chaque frame
        timer += Time.deltaTime;

        // Quand le délai est atteint, on crée un fantôme
        if (timer >= spawnInterval)
        {
            SpawnGhost();
            timer = 0f;
        }
    }

    private void SpawnGhost()
    {
        // Crée un fantôme à la position et à la rotation actuelles de l'objet
        Instantiate(ghostPrefab, transform.position, transform.rotation);
    }
}