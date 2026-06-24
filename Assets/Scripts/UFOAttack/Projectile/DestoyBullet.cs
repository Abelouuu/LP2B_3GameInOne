using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    // Animation d'explosion jouée quand le projectile est détruit
    public GameObject explosionAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le projectile touche le joueur ou une bombe
        if (collision.CompareTag("Player") || collision.CompareTag("Bomb"))
        {
            // Crée l'animation d'explosion à la position du projectile
            GameObject explosion = Instantiate(explosionAnim, transform.position, Quaternion.identity);

            // Détruit l'explosion après 1 seconde
            Destroy(explosion, 1f);

            // Détruit le projectile
            Destroy(gameObject);
        }
    }
}