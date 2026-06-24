using UnityEngine;
using System.Collections;

public class PurpleEnemy : EnemyBase
{
    // Distance parcourue à chaque déplacement vers la gauche
    public float stepWidth;

    // Vitesse de déplacement de l'ennemi
    public float speed;

    // Préfab des projectiles tirés par l'ennemi
    public GameObject bulletPrefab;

    [Header("Specific Audio")]
    public AudioClip projectileSound;

    protected override IEnumerator Behavior()
    {
        // L'ennemi continue son comportement tant qu'il n'est pas trop loin hors écran
        while (transform.position.x > -10f)
        {
            // On garde en mémoire la position de départ avant le déplacement
            float xPosition = transform.position.x;

            // L'ennemi avance vers la gauche sur une distance définie par stepWidth
            while (transform.position.x > xPosition - stepWidth)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
                yield return null;
            }

            // Petite pause avant de tirer
            yield return new WaitForSeconds(1f);

            // Tir de deux projectiles
            Shoot();

            // Pause avant le prochain déplacement
            yield return new WaitForSeconds(1f);
        }

        // Une fois sorti trop loin de l'écran, l'ennemi se détruit
        DestroySelf();
    }

    private void Shoot()
    {
        // Son du tir
        PlaySound(projectileSound);

        // Création d'un projectile légèrement au-dessus de l'ennemi
        GameObject bulletTop = Instantiate(
            bulletPrefab,
            transform.position + new Vector3(-0.9f, 0.1f, 0f),
            transform.rotation
        );

        // Création d'un projectile légèrement en-dessous de l'ennemi
        GameObject bulletBottom = Instantiate(
            bulletPrefab,
            transform.position + new Vector3(-0.9f, -0.1f, 0f),
            transform.rotation
        );

        // Directions des deux projectiles : un vers le haut-gauche et un vers le bas-gauche
        Vector2 directionBulletTop = new Vector2(-1f, 0.5f).normalized;
        Vector2 directionBulletBottom = new Vector2(-1f, -0.5f).normalized;

        // Application des directions aux projectiles
        bulletTop.GetComponent<ProjectileMovement>().SetDirection(directionBulletTop);
        bulletBottom.GetComponent<ProjectileMovement>().SetDirection(directionBulletBottom);
    }
}