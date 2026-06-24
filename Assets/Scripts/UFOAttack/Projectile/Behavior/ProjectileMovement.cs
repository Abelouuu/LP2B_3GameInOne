using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    // Vitesse de déplacement du projectile
    public float speed = 10f;

    // Direction dans laquelle le projectile va avancer
    private Vector2 direction;

    public void SetDirection(Vector2 newDirection)
    {
        // On normalise la direction pour garder une vitesse constante
        direction = newDirection.normalized;

        // Calcule l'angle correspondant à la direction du projectile
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Oriente visuellement le projectile dans sa direction de déplacement
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Update()
    {
        // Déplace le projectile dans la direction choisie
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Détruit le projectile quand il sort trop loin de l'écran
        if (transform.position.x > 10f || transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}