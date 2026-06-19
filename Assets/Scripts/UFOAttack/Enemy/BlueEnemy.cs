using UnityEngine;
using System.Collections;

public class BlueEnemy : EnemyBase
{
    public GameObject missilePrefab;
    public GameObject secondMissilePrefab;
    public GameObject explosionEffect;

    public float horizontalSpeed = 3f;
    public float verticalSpeed = 1f;

    public float shotInterval = 4f;
    public float timeBeforeSplit = 1f;

    [Header("Specific Audio")]
    public AudioClip missileSound;
    public AudioClip projectileSound;

    protected override IEnumerator Behavior()
    {
        while (transform.position.x > 7.3f)
        {
            transform.position += Vector3.left * Time.deltaTime * horizontalSpeed;
            yield return null;
        }

        Shoot();
        yield return new WaitForSeconds(1f);

        float shootTimer = 0f;

        while (true)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer >= shotInterval)
            {
                Shoot();
                shootTimer = 0f;
            }

            if (transform.position.y > 1.20f)
            {
                verticalSpeed = -Mathf.Abs(verticalSpeed);
            }
            else if (transform.position.y < -2.2f)
            {
                verticalSpeed = Mathf.Abs(verticalSpeed);
            }

            transform.position += Vector3.up * verticalSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator SplitMissile(GameObject missile1, GameObject missile2)
    {
        yield return new WaitForSeconds(timeBeforeSplit);

        SplitOneMissile(missile1);
        SplitOneMissile(missile2);
    }

    private void SplitOneMissile(GameObject missile)
    {
        if (missile == null)
            return;

        Vector3 position = missile.transform.position;

        GameObject explosion = Instantiate(explosionEffect, position, Quaternion.identity);
        Destroy(explosion, 1f);

        PlaySound(projectileSound);

        GameObject subMissile1 = Instantiate(secondMissilePrefab, position, Quaternion.identity);
        GameObject subMissile2 = Instantiate(secondMissilePrefab, position, Quaternion.identity);

        subMissile1.GetComponent<ProjectileMovement>().SetDirection(new Vector2(-1f, 0.4f));
        subMissile2.GetComponent<ProjectileMovement>().SetDirection(new Vector2(-1f, -0.4f));

        Destroy(missile);
    }

    private void Shoot()
    {
        PlaySound(missileSound);

        GameObject projectile1 = Instantiate(
            missilePrefab,
            transform.position + new Vector3(-1.4f, 0f),
            Quaternion.identity
        );

        projectile1.GetComponent<ProjectileMovement>().SetDirection(new Vector2(-1f, 0.4f));

        GameObject projectile2 = Instantiate(
            missilePrefab,
            transform.position + new Vector3(-1.4f, 0f),
            Quaternion.identity
        );

        projectile2.GetComponent<ProjectileMovement>().SetDirection(new Vector2(-1f, -0.4f));

        StartCoroutine(SplitMissile(projectile1, projectile2));
    }
}