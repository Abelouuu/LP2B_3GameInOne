using UnityEngine;

public class DestroyPlayerProj : MonoBehaviour
{
    // Animation d'explosion jouée quand le projectile du joueur est détruit
    public GameObject explosionAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le projectile touche un ennemi ou un missile
        if (collision.CompareTag("Enemy") || collision.CompareTag("Missile"))
        {
            // Crée l'animation d'explosion à la position du projectile
            GameObject explosion = Instantiate(explosionAnim, transform.position, Quaternion.identity);

            // Détruit l'explosion après 1 seconde
            Destroy(explosion, 1f);

            // Détruit le projectile du joueur
            Destroy(gameObject);
        }
    }
}