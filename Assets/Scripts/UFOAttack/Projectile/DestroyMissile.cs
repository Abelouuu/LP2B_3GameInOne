using UnityEngine;

public class DestroyMissile : MonoBehaviour
{
    // Animation d'explosion jouée quand le missile est détruit
    public GameObject explosionAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le missile touche le joueur ou une bombe
        if (collision.CompareTag("Player") || collision.CompareTag("Bomb"))
        {
            // Crée l'animation d'explosion à la position du missile
            GameObject explosion = Instantiate(explosionAnim, transform.position, Quaternion.identity);

            // Détruit l'explosion après 1 seconde
            Destroy(explosion, 1f);

            // Détruit le missile
            Destroy(gameObject);
        }
    }
}