using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    // Nombre de dégâts infligés au joueur
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si l'objet touché est le joueur
        if (other.CompareTag("Player"))
        {
            // Récupère le script de vie du joueur
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // Si le joueur possède bien ce script, on lui inflige des dégâts
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}