using UnityEngine;
using System.Collections;

public class LaserAnimation : MonoBehaviour
{
    public Transform ball;
    public Transform laser;

    public int damage = 3;

    public float laserScaleX = 2600;
    public float laserScaleY = 1f;

    public float ballScale = 1f;

    [Header("Duration Repartition")]
    public float growDuration = 0.5f;
    public float growLaserDuration1 = 0.2f;
    public float growLaserDuration2 = 0.3f;

    public bool canDamage = false;

    private Vector2 direction = Vector2.left;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laser.localScale = new Vector3(laserScaleX, 0f, 1f);
        ball.localScale = Vector3.zero;

        StartCoroutine(BallAnimation());
        StartCoroutine(LaserAnim());
    }
    IEnumerator BallAnimation()
    {
        float t = 0f;

        while (t < growDuration)
        {
            t += Time.deltaTime;
            float ratio = t / growDuration;

            float scale = Mathf.Lerp(0f, ballScale, ratio);
            ball.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        ball.localScale = new Vector3(ballScale, ballScale, 1f);

        yield return new WaitForSeconds(1f);

        t = 0f;
        while (t < growDuration)
        {
            t += Time.deltaTime;
            float ratio = t / growDuration;

            float scale = Mathf.Lerp(ballScale, 0f, ratio);
            ball.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        ball.localScale = Vector3.zero;
        Destroy(gameObject);
    }
    IEnumerator LaserAnim()
    {
        // Le laser commence avant la fin du grossissement de la balle
        yield return new WaitForSeconds(0.2f);

        float t = 0f;

        // Laser fin
        while (t < growLaserDuration1)
        {
            t += Time.deltaTime;
            float ratio = t / growLaserDuration1;

            float y = Mathf.Lerp(0f, laserScaleY / 10f, ratio);
            laser.localScale = new Vector3(laserScaleX, y, 1f);

            yield return null;
        }

        yield return new WaitForSeconds(0.7f);

        // Laser épais
        t = 0f;
        canDamage = true;
        while (t < growLaserDuration2)
        {
            t += Time.deltaTime;
            float ratio = t / growLaserDuration2;

            float y = Mathf.Lerp(laserScaleY / 10f, laserScaleY, ratio);
            laser.localScale = new Vector3(laserScaleX, y, 1f);

            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        
        // Disparition
        t = 0f;

        while (t < growLaserDuration2+growLaserDuration1)
        {
            t += Time.deltaTime;
            float ratio = t / growLaserDuration2+growLaserDuration1;

            float y = Mathf.Lerp(laserScaleY, 0f, ratio);
            laser.localScale = new Vector3(laserScaleX, y, 1f);

            yield return null;
        }
        canDamage = false;
        laser.localScale = new Vector3(laserScaleX, 0f, 1f);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle + 180);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (canDamage && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage);
            canDamage = false;
        }
    }
}
