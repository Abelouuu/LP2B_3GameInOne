using UnityEngine;
using System.Collections;

public class PurpleEnemy : EnemyBase
{
    public float stepWidth;
    public float speed;

    public GameObject bulletPrefab;

    [Header("Specific Audio")]
    public AudioClip projectileSound;

    protected override IEnumerator Behavior()
    {
        while (transform.position.x > -10f)
        {
            float xPosition = transform.position.x;

            while (transform.position.x > xPosition - stepWidth)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            Shoot();

            yield return new WaitForSeconds(1f);
        }

        DestroySelf();
    }

    private void Shoot()
    {
        PlaySound(projectileSound);

        GameObject bulletTop = Instantiate(
            bulletPrefab,
            transform.position + new Vector3(-0.9f, 0.1f, 0f),
            transform.rotation
        );

        GameObject bulletBottom = Instantiate(
            bulletPrefab,
            transform.position + new Vector3(-0.9f, -0.1f, 0f),
            transform.rotation
        );

        Vector2 directionBulletTop = new Vector2(-1f, 0.5f).normalized;
        Vector2 directionBulletBottom = new Vector2(-1f, -0.5f).normalized;

        bulletTop.GetComponent<ProjectileMovement>().SetDirection(directionBulletTop);
        bulletBottom.GetComponent<ProjectileMovement>().SetDirection(directionBulletBottom);
    }
}