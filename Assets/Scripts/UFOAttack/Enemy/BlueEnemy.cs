using UnityEngine;
using System.Collections;

public class BlueEnemy : EnemyBase
{
    // Préfab des missiles principaux
    public GameObject missilePrefab;

    // Préfab des petits missiles créés après la séparation
    public GameObject secondMissilePrefab;

    // Effet visuel joué quand un missile se divise
    public GameObject explosionEffect;

    // Vitesse d'entrée de l'ennemi vers la gauche
    public float horizontalSpeed = 3f;

    // Vitesse de déplacement vertical une fois placé
    public float verticalSpeed = 1f;

    // Temps entre deux tirs
    public float shotInterval = 4f;

    // Temps avant qu'un missile principal se sépare
    public float timeBeforeSplit = 1f;

    [Header("Specific Audio")]
    public AudioClip missileSound;
    public AudioClip projectileSound;

    protected override IEnumerator Behavior()
    {
        // L'ennemi entre dans l'écran jusqu'à atteindre une position fixe en X
        while (transform.position.x > 7.3f)
        {
            transform.position += Vector3.left * Time.deltaTime * horizontalSpeed;
            yield return null;
        }

        // Premier tir juste après son arrivée
        Shoot();
        yield return new WaitForSeconds(1f);

        float shootTimer = 0f;

        while (true)
        {
            // Timer permettant de tirer à intervalle régulier
            shootTimer += Time.deltaTime;

            if (shootTimer >= shotInterval)
            {
                Shoot();
                shootTimer = 0f;
            }

            // L'ennemi change de direction quand il atteint les limites verticales
            if (transform.position.y > 1.20f)
            {
                verticalSpeed = -Mathf.Abs(verticalSpeed);
            }
            else if (transform.position.y < -2.2f)
            {
                verticalSpeed = Mathf.Abs(verticalSpeed);
            }

            // Déplacement vertical continu de l'ennemi
            transform.position += Vector3.up * verticalSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator SplitMissile(GameObject missile1, GameObject missile2)
    {
        // On attend un court instant avant de diviser les missiles
        yield return new WaitForSeconds(timeBeforeSplit);

        SplitOneMissile(missile1);
        SplitOneMissile(missile2);
    }

    private void SplitOneMissile(GameObject missile)
    {
        // Si le missile a déjà été détruit, on évite une erreur
        if (missile == null)
            return;

        Vector3 position = missile.transform.position;

        // Création d'une explosion à la position du missile
        GameObject explosion = Instantiate(explosionEffect, position, Quaternion.identity);
        Destroy(explosion, 1f);

        PlaySound(projectileSound);

        // Création de deux sous-missiles qui partent dans deux directions différentes
        GameObject subMissile1 = Instantiate(secondMissilePrefab, position, Quaternion.identity);
        GameObject subMissile2 = Instantiate(secondMissilePrefab, position, Quaternion.identity);

        subMissile1.GetComponent<ProjectileMovement>().SetDirection(new Vector2(-1f, 0.4f));
        subMissile2.GetComponent<ProjectileMovement>().SetDirection(new Vector2(-1f, -0.4f));

        // Destruction du missile principal après sa séparation
        Destroy(missile);
    }

    private void Shoot()
    {
        // Son du tir principal
        PlaySound(missileSound);

        // Premier missile tiré vers le haut-gauche
        GameObject projectile1 = Instantiate(
            missilePrefab,
            transform.position + new Vector3(-1.4f, 0f),
            Quaternion.identity
        );

        projectile1.GetComponent<ProjectileMovement>().SetDirection(new Vector2(-1f, 0.4f));

        // Deuxième missile tiré vers le bas-gauche
        GameObject projectile2 = Instantiate(
            missilePrefab,
            transform.position + new Vector3(-1.4f, 0f),
            Quaternion.identity
        );

        projectile2.GetComponent<ProjectileMovement>().SetDirection(new Vector2(-1f, -0.4f));

        // Les deux missiles vont se diviser après un délai
        StartCoroutine(SplitMissile(projectile1, projectile2));
    }
}