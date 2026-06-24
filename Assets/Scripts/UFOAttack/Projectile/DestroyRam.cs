using UnityEngine;

public class DestroyRam : MonoBehaviour
{
    // Animation d'explosion jouée quand l'ennemi est détruit
    public GameObject explosionAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le Ram touche le joueur
        if (collision.CompareTag("Player"))
        {
            // Crée l'animation d'explosion à la position du Ram
            GameObject explosion = Instantiate(explosionAnim, transform.position, Quaternion.identity);

            // Détruit l'explosion après 1 seconde
            Destroy(explosion, 1f);

            // Détruit le Ram
            Destroy(gameObject);
        }
    }
}