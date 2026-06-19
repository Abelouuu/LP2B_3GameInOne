using UnityEngine;
using System.Collections;

public class LaserShooter : EnemyBase
{
    public GameObject laserPrefab;

    public float horizontalSpeed = 5f;
    public float verticalSpeed = 1f;

    [Header("Specific Audio")]
    public AudioClip laserSound;

    protected override IEnumerator Behavior()
    {
        while (transform.position.x > 5.4f)
        {
            transform.position += Vector3.left * Time.deltaTime * horizontalSpeed;
            yield return null;
        }

        ShootLaser();

        yield return new WaitForSeconds(2f);

        while (true)
        {
            float moveDuration = Random.Range(1f, 3f);
            float t = 0f;

            while (t < moveDuration)
            {
                if (transform.position.y > 2.70f)
                {
                    verticalSpeed = -Mathf.Abs(verticalSpeed);
                }
                else if (transform.position.y < -3.7f)
                {
                    verticalSpeed = Mathf.Abs(verticalSpeed);
                }

                t += Time.deltaTime;
                transform.position += Vector3.up * verticalSpeed * Time.deltaTime;

                yield return null;
            }

            ShootLaser();

            yield return new WaitForSeconds(3f);
        }
    }

    private void ShootLaser()
    {
        PlaySound(laserSound);

        Instantiate(
            laserPrefab,
            transform.position + new Vector3(-1.4f, 0f, 0f),
            transform.rotation,
            transform
        );
    }
}