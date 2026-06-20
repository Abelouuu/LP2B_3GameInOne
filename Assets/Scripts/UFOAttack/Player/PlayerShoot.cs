using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    public GameObject laserPrefab;
    private Coroutine fireRateCoroutine;

    [SerializeField] private AudioClip shootSound;

    [Range(0.0f, 1f)]
    public float volume = 1f;

    public float fireRate = 0.2f; // temps entre deux tirs

    private float baseFireRate;
    private float nextFireTime = 0f;

    private void Awake()
    {
        baseFireRate = fireRate;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        AudioManager.Instance.PlaySFX(shootSound, volume);

        GameObject laser1 = Instantiate(laserPrefab, transform.position + Vector3.up * 0.2f, Quaternion.identity);
        GameObject laser2 = Instantiate(laserPrefab, transform.position + Vector3.up * -0.2f, Quaternion.identity);

        laser1.GetComponent<ProjectileMovement>().SetDirection(Vector2.right);
        laser2.GetComponent<ProjectileMovement>().SetDirection(Vector2.right);
    }

    public void IncreaseFireRate(float multiplier, float duration)
    {
        if (fireRateCoroutine != null)
        {
            StopCoroutine(fireRateCoroutine);
        }

        fireRateCoroutine = StartCoroutine(FireRateBonus(multiplier, duration));
    }

    private IEnumerator FireRateBonus(float fireRateMultiplier, float duration)
    {
        fireRate = baseFireRate * fireRateMultiplier;

        yield return new WaitForSeconds(duration);

        fireRate = baseFireRate;
        fireRateCoroutine = null;
    }
}