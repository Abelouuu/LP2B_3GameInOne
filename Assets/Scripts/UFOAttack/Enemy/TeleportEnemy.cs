using UnityEngine;
using System.Collections;

public class TeleportEnemy : EnemyBase
{
    [Header("References")]
    public GameObject playerObject;
    public GameObject projectilePrefab;
    public GameObject laserPrefab;
    public Transform shootPoint;

    [Header("Teleport Positions")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("Timing")]
    public float appearDuration;
    public float disappearDuration;
    public float timeBeforeShoot;
    public float timeAfterShoot;

    [Header("Rotation")]
    public float teleportRotationSpeed;
    public float aimRotationSpeed;
    public float aimPrecision;

    private SpriteRenderer spriteRenderer;
    private Vector3 baseScale;
    private bool teleportRotation = false;

    [Header("Specific Audio")]
    public AudioClip laserSound;
    public AudioClip projectileSound;

    protected override void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;

        base.Start();
    }

    private void Update()
    {
        if (teleportRotation)
        {
            transform.Rotate(0f, 0f, teleportRotationSpeed * Time.deltaTime);
        }
    }

    protected override IEnumerator Behavior()
    {
        while (true)
        {
            TeleportToRandomPosition();

            yield return StartCoroutine(Appear());

            yield return StartCoroutine(RotateTowardPlayer());

            yield return new WaitForSeconds(timeBeforeShoot);

            yield return StartCoroutine(Shoot());

            yield return new WaitForSeconds(timeAfterShoot);

            yield return StartCoroutine(Disappear());
        }
    }

    private IEnumerator RotateTowardPlayer()
    {
        if (playerObject == null)
            yield break;

        while (true)
        {
            Vector2 direction = (playerObject.transform.position - transform.position).normalized;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float currentAngle = transform.eulerAngles.z;

            float newAngle = Mathf.MoveTowardsAngle(
                currentAngle,
                targetAngle,
                aimRotationSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Euler(0f, 0f, newAngle);

            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(newAngle, targetAngle));

            if (angleDifference <= aimPrecision)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator Shoot()
    {
        if (playerObject == null)
            yield break;

        int attackType = Random.Range(0, 10);
        Vector2 direction = (playerObject.transform.position - transform.position).normalized;

        if (attackType < 7)
        {
            int projectileCount = 4;
            float delayBetweenShots = 0.15f;
            float randomAngleGap = 10f;

            for (int i = 0; i < projectileCount; i++)
            {
                PlaySound(projectileSound);

                float randomAngle = Random.Range(-randomAngleGap, randomAngleGap);
                Vector2 finalDirection = Quaternion.Euler(0f, 0f, randomAngle) * direction;

                GameObject projectile = Instantiate(
                    projectilePrefab,
                    shootPoint.position,
                    Quaternion.identity
                );

                ProjectileMovement movement = projectile.GetComponent<ProjectileMovement>();

                if (movement != null)
                {
                    movement.SetDirection(finalDirection);
                }

                yield return new WaitForSeconds(delayBetweenShots);
            }
        }
        else
        {
            PlaySound(laserSound);

            GameObject laser = Instantiate(
                laserPrefab,
                shootPoint.position,
                Quaternion.identity,
                transform
            );

            LaserAnimation movement = laser.GetComponent<LaserAnimation>();

            if (movement != null)
            {
                movement.SetDirection(direction);
            }
        }
    }

    private void TeleportToRandomPosition()
    {
        float randomY = Random.Range(minY, maxY);
        float randomX = Random.Range(minX, maxX);

        transform.position = new Vector3(randomX, randomY, 0f);
    }

    private IEnumerator Appear()
    {
        teleportRotation = true;

        float t = 0f;
        transform.localScale = Vector3.zero;
        SetAlpha(0f);

        while (t < appearDuration)
        {
            t += Time.deltaTime;
            float ratio = t / appearDuration;

            transform.localScale = Vector3.Lerp(Vector3.zero, baseScale, ratio);
            SetAlpha(Mathf.Lerp(0f, 1f, ratio));

            yield return null;
        }

        transform.localScale = baseScale;
        SetAlpha(1f);

        teleportRotation = false;
    }

    private IEnumerator Disappear()
    {
        teleportRotation = true;

        float t = 0f;
        Vector3 startScale = transform.localScale;

        while (t < disappearDuration)
        {
            t += Time.deltaTime;
            float ratio = t / disappearDuration;

            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, ratio);
            SetAlpha(Mathf.Lerp(1f, 0f, ratio));

            yield return null;
        }

        transform.localScale = Vector3.zero;
        SetAlpha(0f);

        teleportRotation = false;
    }

    private void SetAlpha(float alpha)
    {
        if (spriteRenderer == null)
            return;

        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}