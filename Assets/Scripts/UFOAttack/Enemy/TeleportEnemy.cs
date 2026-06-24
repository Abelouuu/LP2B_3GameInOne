using UnityEngine;
using System.Collections;

public class TeleportEnemy : EnemyBase
{
    [Header("References")]
    // Référence vers le joueur, utilisée pour viser
    public GameObject playerObject;

    // Préfab du projectile simple
    public GameObject projectilePrefab;

    // Préfab du laser
    public GameObject laserPrefab;

    // Point depuis lequel l'ennemi tire
    public Transform shootPoint;

    [Header("Teleport Positions")]
    // Limites de la zone dans laquelle l'ennemi peut se téléporter
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("Timing")]
    // Durée d'apparition de l'ennemi
    public float appearDuration;

    // Durée de disparition de l'ennemi
    public float disappearDuration;

    // Temps d'attente avant le tir
    public float timeBeforeShoot;

    // Temps d'attente après le tir avant de disparaître
    public float timeAfterShoot;

    [Header("Rotation")]
    // Vitesse de rotation pendant l'apparition et la disparition
    public float teleportRotationSpeed;

    // Vitesse à laquelle l'ennemi tourne pour viser le joueur
    public float aimRotationSpeed;

    // Précision nécessaire pour considérer que l'ennemi vise correctement
    public float aimPrecision;

    // SpriteRenderer utilisé pour modifier la transparence de l'ennemi
    private SpriteRenderer spriteRenderer;

    // Taille de base de l'ennemi, sauvegardée au démarrage
    private Vector3 baseScale;

    // Indique si l'ennemi doit tourner pendant sa téléportation
    private bool teleportRotation = false;

    [Header("Specific Audio")]
    public AudioClip laserSound;
    public AudioClip projectileSound;

    protected override void Start()
    {
        // Récupère le SpriteRenderer pour pouvoir gérer le fondu
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Sauvegarde la taille normale de l'ennemi
        baseScale = transform.localScale;

        // Lance le Start de la classe mère, donc le comportement de l'ennemi
        base.Start();
    }

    private void Update()
    {
        // Pendant l'apparition ou la disparition, l'ennemi tourne sur lui-même
        if (teleportRotation)
        {
            transform.Rotate(0f, 0f, teleportRotationSpeed * Time.deltaTime);
        }
    }

    protected override IEnumerator Behavior()
    {
        while (true)
        {
            // Choisit une nouvelle position aléatoire
            TeleportToRandomPosition();

            // Animation d'apparition
            yield return StartCoroutine(Appear());

            // L'ennemi se tourne vers le joueur
            yield return StartCoroutine(RotateTowardPlayer());

            // Attend avant de tirer
            yield return new WaitForSeconds(timeBeforeShoot);

            // Lance une attaque
            yield return StartCoroutine(Shoot());

            // Attend après le tir
            yield return new WaitForSeconds(timeAfterShoot);

            // Animation de disparition
            yield return StartCoroutine(Disappear());
        }
    }

    private IEnumerator RotateTowardPlayer()
    {
        // Si le joueur n'est pas assigné, on arrête cette étape
        if (playerObject == null)
            yield break;

        while (true)
        {
            // Calcule la direction entre l'ennemi et le joueur
            Vector2 direction = (playerObject.transform.position - transform.position).normalized;

            // Calcule l'angle cible correspondant à cette direction
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float currentAngle = transform.eulerAngles.z;

            // Tourne progressivement vers l'angle cible
            float newAngle = Mathf.MoveTowardsAngle(
                currentAngle,
                targetAngle,
                aimRotationSpeed * Time.deltaTime
            );

            transform.rotation = Quaternion.Euler(0f, 0f, newAngle);

            // Vérifie si l'ennemi est assez proche de l'angle cible
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(newAngle, targetAngle));

            if (angleDifference <= aimPrecision)
            {
                // Force l'angle final pour être bien aligné
                transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator Shoot()
    {
        // Si le joueur n'existe pas, l'ennemi ne peut pas viser
        if (playerObject == null)
            yield break;

        // Choix aléatoire du type d'attaque
        int attackType = Random.Range(0, 10);

        // Direction principale vers le joueur
        Vector2 direction = (playerObject.transform.position - transform.position).normalized;

        if (attackType < 7)
        {
            // Dans 70% des cas, l'ennemi tire une rafale de projectiles
            int projectileCount = 4;
            float delayBetweenShots = 0.15f;
            float randomAngleGap = 10f;

            for (int i = 0; i < projectileCount; i++)
            {
                PlaySound(projectileSound);

                // Ajoute une légère variation d'angle pour rendre le tir moins prévisible
                float randomAngle = Random.Range(-randomAngleGap, randomAngleGap);
                Vector2 finalDirection = Quaternion.Euler(0f, 0f, randomAngle) * direction;

                // Création du projectile
                GameObject projectile = Instantiate(
                    projectilePrefab,
                    shootPoint.position,
                    Quaternion.identity
                );

                ProjectileMovement movement = projectile.GetComponent<ProjectileMovement>();

                // Donne sa direction au projectile
                if (movement != null)
                {
                    movement.SetDirection(finalDirection);
                }

                yield return new WaitForSeconds(delayBetweenShots);
            }
        }
        else
        {
            // Dans 30% des cas, l'ennemi tire un laser
            PlaySound(laserSound);

            GameObject laser = Instantiate(
                laserPrefab,
                shootPoint.position,
                Quaternion.identity,
                transform
            );

            LaserAnimation movement = laser.GetComponent<LaserAnimation>();

            // Oriente le laser vers le joueur
            if (movement != null)
            {
                movement.SetDirection(direction);
            }
        }
    }

    private void TeleportToRandomPosition()
    {
        // Choisit une position aléatoire dans les limites données
        float randomY = Random.Range(minY, maxY);
        float randomX = Random.Range(minX, maxX);

        transform.position = new Vector3(randomX, randomY, 0f);
    }

    private IEnumerator Appear()
    {
        // Active la rotation pendant l'apparition
        teleportRotation = true;

        float t = 0f;

        // L'ennemi commence invisible et sans taille
        transform.localScale = Vector3.zero;
        SetAlpha(0f);

        // Augmente progressivement la taille et l'opacité
        while (t < appearDuration)
        {
            t += Time.deltaTime;
            float ratio = t / appearDuration;

            transform.localScale = Vector3.Lerp(Vector3.zero, baseScale, ratio);
            SetAlpha(Mathf.Lerp(0f, 1f, ratio));

            yield return null;
        }

        // Force les valeurs finales
        transform.localScale = baseScale;
        SetAlpha(1f);

        // Arrête la rotation de téléportation
        teleportRotation = false;
    }

    private IEnumerator Disappear()
    {
        // Active la rotation pendant la disparition
        teleportRotation = true;

        float t = 0f;
        Vector3 startScale = transform.localScale;

        // Réduit progressivement la taille et l'opacité
        while (t < disappearDuration)
        {
            t += Time.deltaTime;
            float ratio = t / disappearDuration;

            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, ratio);
            SetAlpha(Mathf.Lerp(1f, 0f, ratio));

            yield return null;
        }

        // Force les valeurs finales
        transform.localScale = Vector3.zero;
        SetAlpha(0f);

        // Arrête la rotation de téléportation
        teleportRotation = false;
    }

    private void SetAlpha(float alpha)
    {
        // Sécurité si le SpriteRenderer n'est pas trouvé
        if (spriteRenderer == null)
            return;

        // Modifie uniquement la transparence du sprite
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}