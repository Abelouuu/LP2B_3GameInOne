using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    // Préfab du projectile tiré par le joueur
    public GameObject laserPrefab;

    // Coroutine utilisée pour gérer le bonus de cadence de tir
    private Coroutine fireRateCoroutine;

    // Son joué quand le joueur tire
    [SerializeField] private AudioClip shootSound;

    [Range(0.0f, 1f)]
    public float volume = 1f;

    // Temps entre deux tirs : plus la valeur est petite, plus le joueur tire vite
    public float fireRate = 0.2f;

    // Cadence de tir de base, utilisée pour revenir à la normale après un bonus
    private float baseFireRate;

    // Moment à partir duquel le joueur pourra tirer à nouveau
    private float nextFireTime = 0f;

    private void Awake()
    {
        // On sauvegarde la cadence de tir initiale
        baseFireRate = fireRate;
    }

    void Update()
    {
        // Si le joueur maintient Espace et que le délai entre deux tirs est écoulé
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();

            // Définit le prochain moment où le joueur pourra tirer
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Joue le son de tir
        AudioManager.Instance.PlaySFX(shootSound, volume);

        // Crée deux lasers légèrement décalés verticalement
        GameObject laser1 = Instantiate(
            laserPrefab,
            transform.position + Vector3.up * 0.2f,
            Quaternion.identity
        );

        GameObject laser2 = Instantiate(
            laserPrefab,
            transform.position + Vector3.up * -0.2f,
            Quaternion.identity
        );

        // Les deux lasers partent vers la droite
        laser1.GetComponent<ProjectileMovement>().SetDirection(Vector2.right);
        laser2.GetComponent<ProjectileMovement>().SetDirection(Vector2.right);
    }

    public void IncreaseFireRate(float multiplier, float duration)
    {
        // Si un bonus de tir est déjà actif, on l'arrête avant d'en lancer un nouveau
        if (fireRateCoroutine != null)
        {
            StopCoroutine(fireRateCoroutine);
        }

        // Lance le bonus temporaire de cadence de tir
        fireRateCoroutine = StartCoroutine(FireRateBonus(multiplier, duration));
    }

    private IEnumerator FireRateBonus(float fireRateMultiplier, float duration)
    {
        // Réduit le temps entre deux tirs, donc augmente la cadence
        fireRate = baseFireRate * fireRateMultiplier;

        // Attend la durée du bonus
        yield return new WaitForSeconds(duration);

        // Remet la cadence de tir normale
        fireRate = baseFireRate;
        fireRateCoroutine = null;
    }
}