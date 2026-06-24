using UnityEngine;
using System.Collections;

public class EventBossController : EnemyBase
{
    [Header("References")]
    // Référence vers le joueur, utilisée pour viser
    public GameObject playerObject;

    // Point depuis lequel le boss tire ses projectiles
    public Transform shootPoint;

    [Header("Projectile Prefabs")]
    // Différents projectiles/attaques que le boss peut utiliser
    public GameObject projectilePrefab;
    public GameObject trackingMissilePrefab;
    public GameObject laserPrefab;

    [Header("Minions")]
    // Ennemis que le boss peut faire apparaître
    public GameObject[] minionPrefabs;

    // Positions possibles pour faire apparaître les ennemis
    public Transform[] minionSpawnPoints;

    [Header("Movement")]
    // Position où le boss se place au début du combat
    public Vector3 fightStartPosition = new Vector3(6.5f, 0f, 0f);

    public float enterSpeed = 3f;
    public float moveSpeed = 3f;

    // Limites de déplacement du boss pendant le combat
    public float minX = 5f;
    public float maxX = 7.5f;
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Orientation")]
    // Permet de choisir si le boss regarde le joueur ou non
    public bool lookAtPlayer = true;

    // Vitesse à laquelle le boss tourne vers sa cible
    public float rotationSpeed = 120f;

    // Temps entre deux mises à jour de la direction visée
    public float aimRefreshInterval = 0.35f;

    // Décalage utilisé si le sprite n'est pas orienté correctement de base
    public float spriteAngleOffset = 0f;

    // Direction utilisée si le boss ne vise pas le joueur
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
        // On récupère le script de vie du boss pour connaître sa vie actuelle
        enemyHealth = GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            maxHealth = enemyHealth.health;
        }

        // Si le joueur n'est pas assigné dans l'inspecteur, on le recherche avec son tag
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        targetRotation = transform.rotation;

        // Coroutine qui met régulièrement à jour la direction vers laquelle le boss doit regarder
        StartCoroutine(UpdateAimTargetRoutine());

        // Lance le comportement défini dans la classe mère EnemyBase
        base.Start();
    }

    private void Update()
    {
        // À chaque frame, le boss tourne progressivement vers sa cible
        RotateTowardTarget();
    }

    private IEnumerator UpdateAimTargetRoutine()
    {
        while (true)
        {
            Vector2 direction;

            // Si possible, le boss vise le joueur
            if (lookAtPlayer && playerObject != null)
            {
                direction = playerObject.transform.position - transform.position;
            }
            else
            {
                direction = defaultLookDirection;
            }

            // On évite de calculer une rotation avec une direction trop petite
            if (direction.sqrMagnitude > 0.001f)
            {
                targetRotation = GetRotationFromDirection(direction);
            }

            yield return new WaitForSeconds(aimRefreshInterval);
        }
    }

    private void RotateTowardTarget()
    {
        // Rotation fluide du boss vers la rotation cible
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

        // Comme le sprite regarde vers le haut de base, on retire 90 degrés
        angle -= 90f;

        // Permet d'ajuster l'orientation si besoin depuis l'inspecteur
        angle += spriteAngleOffset;

        return Quaternion.Euler(0f, 0f, angle);
    }

    protected override IEnumerator Behavior()
    {
        // Le boss commence par entrer dans l'arène
        yield return StartCoroutine(EnterArena());

        // Petite attente avant la première attaque
        yield return new WaitForSeconds(timeBeforeFirstAttack);

        while (true)
        {
            // Le boss se déplace à une position aléatoire
            yield return StartCoroutine(MoveToRandomPosition());

            // Il choisit ensuite une attaque selon sa phase
            yield return StartCoroutine(ChooseAndExecuteAttack());

            yield return new WaitForSeconds(timeBetweenAttacks);
        }
    }

    private IEnumerator EnterArena()
    {
        // Déplacement progressif jusqu'à la position de départ du combat
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
        // Choisit une position aléatoire dans la zone autorisée
        Vector3 targetPosition = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            0f
        );

        // Déplace le boss jusqu'à cette position
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
        // La phase dépend de la vie restante du boss
        int phase = GetCurrentPhase();

        // Valeur aléatoire utilisée pour choisir l'attaque
        int roll = Random.Range(0, 100);

        if (phase == 1)
        {
            // Phase 1 : attaques plutôt simples
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
            // Phase 2 : le boss débloque la spirale et les lasers
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
            // Phase 3 : le boss devient plus agressif
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
        // Si la vie n'est pas disponible, on reste en phase 1
        if (enemyHealth == null || maxHealth <= 0)
            return 1;

        float healthRatio = enemyHealth.health / (float)maxHealth;

        // Phase 3 : moins d'un tiers de vie
        if (healthRatio <= 0.33f)
            return 3;

        // Phase 2 : moins de deux tiers de vie
        if (healthRatio <= 0.66f)
            return 2;

        // Phase 1 : début du combat
        return 1;
    }

    private IEnumerator TargetedBurst()
    {
        if (playerObject == null || shootPoint == null)
            yield break;

        // Direction de base vers le joueur
        Vector2 baseDirection =
            (playerObject.transform.position - shootPoint.position).normalized;

        // Tire plusieurs projectiles avec une légère variation d'angle
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

        // Tire des projectiles tout autour du boss
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

        // Tire plusieurs vagues circulaires avec un décalage d'angle
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

        // Tire plusieurs missiles à tête chercheuse
        for (int i = 0; i < missileCount; i++)
        {
            PlaySound(missileSound);

            Vector3 offset;

            // Alterne les missiles entre le haut et le bas du point de tir
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

        // Direction principale vers le joueur
        Vector2 baseDirection =
            (playerObject.transform.position - shootPoint.position).normalized;

        // Petit délai avant le laser pour laisser le temps au joueur de réagir
        yield return new WaitForSeconds(laserWarningDelay);

        PlaySound(laserSound);

        // Laser au centre
        SpawnLaser(baseDirection);

        // Deux lasers inclinés autour de la direction principale
        Vector2 upperDirection =
            Quaternion.Euler(0f, 0f, laserAngleSpread) * baseDirection;

        Vector2 lowerDirection =
            Quaternion.Euler(0f, 0f, -laserAngleSpread) * baseDirection;

        SpawnLaser(upperDirection);
        SpawnLaser(lowerDirection);
    }

    private IEnumerator EnragedCombo()
    {
        // Combo utilisé en dernière phase : rafale, spirale puis lasers
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

        // Si des points de spawn sont définis, on les utilise
        if (minionSpawnPoints != null && minionSpawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in minionSpawnPoints)
            {
                SpawnMinion(spawnPoint.position);
            }
        }
        else
        {
            // Sinon, on utilise deux positions par défaut autour du boss
            SpawnMinion(transform.position + new Vector3(1.5f, 1.5f, 0f));
            SpawnMinion(transform.position + new Vector3(1.5f, -1.5f, 0f));
        }
    }

    private void SpawnProjectile(Vector2 direction)
    {
        if (projectilePrefab == null || shootPoint == null)
            return;

        // Création d'un projectile simple
        GameObject projectile = Instantiate(
            projectilePrefab,
            shootPoint.position,
            Quaternion.identity
        );

        ProjectileMovement movement =
            projectile.GetComponent<ProjectileMovement>();

        // On donne une direction au projectile
        if (movement != null)
        {
            movement.SetDirection(direction);
        }
    }

    private void SpawnLaser(Vector2 direction)
    {
        if (laserPrefab == null || shootPoint == null)
            return;

        // Création d'un laser
        GameObject laser = Instantiate(
            laserPrefab,
            shootPoint.position,
            Quaternion.identity
        );

        LaserAnimation laserAnimation =
            laser.GetComponent<LaserAnimation>();

        // On donne une direction au laser
        if (laserAnimation != null)
        {
            laserAnimation.SetDirection(direction);
        }
    }

    private void SpawnMinion(Vector3 position)
    {
        // Choisit un ennemi aléatoire parmi les préfabs disponibles
        GameObject prefab =
            minionPrefabs[Random.Range(0, minionPrefabs.Length)];

        GameObject minion = Instantiate(
            prefab,
            position,
            Quaternion.identity
        );

        // Si le minion est un ennemi orange, on lui transmet la référence du joueur
        OrangeEnemy orangeEnemy = minion.GetComponent<OrangeEnemy>();
        if (orangeEnemy != null)
        {
            orangeEnemy.playerObject = playerObject;
        }

        // Si le minion est un ennemi téléporteur, on lui transmet aussi le joueur
        TeleportEnemy teleportEnemy = minion.GetComponent<TeleportEnemy>();
        if (teleportEnemy != null)
        {
            teleportEnemy.playerObject = playerObject;
        }
    }
}