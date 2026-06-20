using UnityEngine;
using System.Collections;

public class EventBossController : EnemyBase
{
    [Header("References")]
    public GameObject playerObject;
    public Transform shootPoint;

    [Header("Projectile Prefabs")]
    public GameObject projectilePrefab;
    public GameObject trackingMissilePrefab;
    public GameObject laserPrefab;

    [Header("Minions")]
    public GameObject[] minionPrefabs;
    public Transform[] minionSpawnPoints;

    [Header("Movement")]
    public Vector3 fightStartPosition = new Vector3(6.5f, 0f, 0f);
    public float enterSpeed = 3f;
    public float moveSpeed = 3f;

    public float minX = 5f;
    public float maxX = 7.5f;
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Orientation")]
    public bool lookAtPlayer = true;

    // Vitesse de rotation du boss
    public float rotationSpeed = 120f;

    // Temps entre deux recalculs de direction vers le joueur
    public float aimRefreshInterval = 0.35f;

    // Décalage si le sprite n'est pas bien orienté
    public float spriteAngleOffset = 0f;

    // Direction par défaut si le boss ne vise pas le joueur
    public Vector2 defaultLookDirection = Vector2.left;

    private Quaternion targetRotation;

    [Header("Attack Timing")]
    public float timeBeforeFirstAttack = 1.5f;
    public float timeBetweenAttacks = 1.2f;

    [Header("Burst Attack")]
    public int burstProjectileCount = 7;
    public float burstDelay = 0.12f;
    public float burstAngleVariation = 12f;

    [Header("Circle Attack")]
    public int circleProjectileCount = 20;

    [Header("Spiral Attack")]
    public int spiralWaves = 5;
    public int spiralProjectilesPerWave = 14;
    public float spiralDelayBetweenWaves = 0.15f;
    public float spiralRotationStep = 18f;

    [Header("Missile Attack")]
    public int missileCount = 4;
    public float missileDelay = 0.25f;

    [Header("Laser Attack")]
    public float laserWarningDelay = 0.4f;
    public float laserAngleSpread = 20f;

    [Header("Audio")]
    public AudioClip projectileSound;
    public AudioClip missileSound;
    public AudioClip laserSound;
    public AudioClip summonSound;

    private EnemyHealth enemyHealth;
    private int maxHealth;

    protected override void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            maxHealth = enemyHealth.health;
        }

        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        targetRotation = transform.rotation;

        StartCoroutine(UpdateAimTargetRoutine());

        base.Start();
    }

    private void Update()
    {
        RotateTowardTarget();
    }

    private IEnumerator UpdateAimTargetRoutine()
    {
        while (true)
        {
            Vector2 direction;

            if (lookAtPlayer && playerObject != null)
            {
                direction = playerObject.transform.position - transform.position;
            }
            else
            {
                direction = defaultLookDirection;
            }

            if (direction.sqrMagnitude > 0.001f)
            {
                targetRotation = GetRotationFromDirection(direction);
            }

            yield return new WaitForSeconds(aimRefreshInterval);
        }
    }

    private void RotateTowardTarget()
    {
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private Quaternion GetRotationFromDirection(Vector2 direction)
    {
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Ton sprite regarde vers le haut de base, donc on retire 90 degrés
        angle -= 90f;

        angle += spriteAngleOffset;

        return Quaternion.Euler(0f, 0f, angle);
    }

    protected override IEnumerator Behavior()
    {
        yield return StartCoroutine(EnterArena());

        yield return new WaitForSeconds(timeBeforeFirstAttack);

        while (true)
        {
            yield return StartCoroutine(MoveToRandomPosition());

            yield return StartCoroutine(ChooseAndExecuteAttack());

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    private IEnumerator EnterArena()
    {
        while (Vector3.Distance(transform.position, fightStartPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                fightStartPosition,
                enterSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = fightStartPosition;
    }

    private IEnumerator MoveToRandomPosition()
    {
        Vector3 targetPosition = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0f
        );

        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }
    }

    private IEnumerator ChooseAndExecuteAttack()
    {
        int phase = GetCurrentPhase();
        int roll = Random.Range(0, 100);

        if (phase == 1)
        {
            if (roll < 35)
            {
                yield return StartCoroutine(TargetedBurst());
            }
            else if (roll < 65)
            {
                CircleAttack();
            }
            else if (roll < 85)
            {
                yield return StartCoroutine(TrackingMissileSalvo());
            }
            else
            {
                SummonMinions();
            }
        }
        else if (phase == 2)
        {
            if (roll < 25)
            {
                yield return StartCoroutine(TargetedBurst());
            }
            else if (roll < 45)
            {
                yield return StartCoroutine(SpiralAttack());
            }
            else if (roll < 65)
            {
                yield return StartCoroutine(TrackingMissileSalvo());
            }
            else if (roll < 85)
            {
                yield return StartCoroutine(TripleLaserAttack());
            }
            else
            {
                SummonMinions();
            }
        }
        else
        {
            if (roll < 25)
            {
                yield return StartCoroutine(SpiralAttack());
            }
            else if (roll < 45)
            {
                yield return StartCoroutine(TrackingMissileSalvo());
            }
            else if (roll < 70)
            {
                yield return StartCoroutine(TripleLaserAttack());
            }
            else if (roll < 90)
            {
                yield return StartCoroutine(EnragedCombo());
            }
            else
            {
                SummonMinions();
            }
        }
    }

    private int GetCurrentPhase()
    {
        if (enemyHealth == null || maxHealth <= 0)
            return 1;

        float healthRatio = enemyHealth.health / (float)maxHealth;

        if (healthRatio <= 0.33f)
            return 3;

        if (healthRatio <= 0.66f)
            return 2;

        return 1;
    }

    private IEnumerator TargetedBurst()
    {
        if (playerObject == null || shootPoint == null)
            yield break;

        Vector2 baseDirection =
            (playerObject.transform.position - shootPoint.position).normalized;

        for (int i = 0; i < burstProjectileCount; i++)
        {
            PlaySound(projectileSound);

            float randomAngle = Random.Range(-burstAngleVariation, burstAngleVariation);

            Vector2 finalDirection =
                Quaternion.Euler(0f, 0f, randomAngle) * baseDirection;

            SpawnProjectile(finalDirection);

            yield return new WaitForSeconds(burstDelay);
        }
    }

    private void CircleAttack()
    {
        if (shootPoint == null)
            return;

        PlaySound(projectileSound);

        for (int i = 0; i < circleProjectileCount; i++)
        {
            float angle = i * 360f / circleProjectileCount;

            Vector2 direction =
                Quaternion.Euler(0f, 0f, angle) * Vector2.right;

            SpawnProjectile(direction);
        }
    }

    private IEnumerator SpiralAttack()
    {
        if (shootPoint == null)
            yield break;

        float angleOffset = Random.Range(0f, 360f);

        for (int wave = 0; wave < spiralWaves; wave++)
        {
            PlaySound(projectileSound);

            for (int i = 0; i < spiralProjectilesPerWave; i++)
            {
                float angle =
                    angleOffset + i * 360f / spiralProjectilesPerWave;

                Vector2 direction =
                    Quaternion.Euler(0f, 0f, angle) * Vector2.right;

                SpawnProjectile(direction);
            }

            angleOffset += spiralRotationStep;

            yield return new WaitForSeconds(spiralDelayBetweenWaves);
        }
    }

    private IEnumerator TrackingMissileSalvo()
    {
        if (trackingMissilePrefab == null || shootPoint == null)
            yield break;

        for (int i = 0; i < missileCount; i++)
        {
            PlaySound(missileSound);

            Vector3 offset;

            if (i % 2 == 0)
            {
                offset = new Vector3(0f, 0.8f, 0f);
            }
            else
            {
                offset = new Vector3(0f, -0.8f, 0f);
            }

            GameObject missile = Instantiate(
                trackingMissilePrefab,
                shootPoint.position + offset,
                Quaternion.identity
            );

            TrackingMissile trackingMissile =
                missile.GetComponent<TrackingMissile>();

            if (trackingMissile != null)
            {
                trackingMissile.playerObject = playerObject;

                Vector2 startDirection;

                if (playerObject != null)
                {
                    startDirection =
                        (playerObject.transform.position - missile.transform.position).normalized;
                }
                else
                {
                    startDirection = Vector2.left;
                }

                trackingMissile.SetDirection(startDirection);
            }

            yield return new WaitForSeconds(missileDelay);
        }
    }

    private IEnumerator TripleLaserAttack()
    {
        if (playerObject == null || shootPoint == null || laserPrefab == null)
            yield break;

        Vector2 baseDirection =
            (playerObject.transform.position - shootPoint.position).normalized;

        yield return new WaitForSeconds(laserWarningDelay);

        PlaySound(laserSound);

        SpawnLaser(baseDirection);

        Vector2 upperDirection =
            Quaternion.Euler(0f, 0f, laserAngleSpread) * baseDirection;

        Vector2 lowerDirection =
            Quaternion.Euler(0f, 0f, -laserAngleSpread) * baseDirection;

        SpawnLaser(upperDirection);
        SpawnLaser(lowerDirection);
    }

    private IEnumerator EnragedCombo()
    {
        yield return StartCoroutine(TargetedBurst());

        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(SpiralAttack());

        yield return new WaitForSeconds(0.4f);

        yield return StartCoroutine(TripleLaserAttack());
    }

    private void SummonMinions()
    {
        if (minionPrefabs == null || minionPrefabs.Length == 0)
            return;

        PlaySound(summonSound);

        if (minionSpawnPoints != null && minionSpawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in minionSpawnPoints)
            {
                SpawnMinion(spawnPoint.position);
            }
        }
        else
        {
            SpawnMinion(transform.position + new Vector3(1.5f, 1.5f, 0f));
            SpawnMinion(transform.position + new Vector3(1.5f, -1.5f, 0f));
        }
    }

    private void SpawnProjectile(Vector2 direction)
    {
        if (projectilePrefab == null || shootPoint == null)
            return;

        GameObject projectile = Instantiate(
            projectilePrefab,
            shootPoint.position,
            Quaternion.identity
        );

        ProjectileMovement movement =
            projectile.GetComponent<ProjectileMovement>();

        if (movement != null)
        {
            movement.SetDirection(direction);
        }
    }

    private void SpawnLaser(Vector2 direction)
    {
        if (laserPrefab == null || shootPoint == null)
            return;

        GameObject laser = Instantiate(
            laserPrefab,
            shootPoint.position,
            Quaternion.identity
        );

        LaserAnimation laserAnimation =
            laser.GetComponent<LaserAnimation>();

        if (laserAnimation != null)
        {
            laserAnimation.SetDirection(direction);
        }
    }

    private void SpawnMinion(Vector3 position)
    {
        GameObject prefab =
            minionPrefabs[Random.Range(0, minionPrefabs.Length)];

        GameObject minion = Instantiate(
            prefab,
            position,
            Quaternion.identity
        );

        OrangeEnemy orangeEnemy = minion.GetComponent<OrangeEnemy>();
        if (orangeEnemy != null)
        {
            orangeEnemy.playerObject = playerObject;
        }

        TeleportEnemy teleportEnemy = minion.GetComponent<TeleportEnemy>();
        if (teleportEnemy != null)
        {
            teleportEnemy.playerObject = playerObject;
        }
    }
}