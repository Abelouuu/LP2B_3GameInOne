using UnityEngine;
using System.Collections;

public class LaserAnimation : MonoBehaviour
{
    // Partie ronde au départ du laser
    public Transform ball;

    // Partie principale du laser
    public Transform laser;

    // Dégâts infligés au joueur si le laser le touche
    public int damage = 3;

    // Taille finale du laser
    public float laserScaleX = 2600;
    public float laserScaleY = 1f;

    // Taille finale de la boule
    public float ballScale = 1f;

    [Header("Duration Repartition")]
    // Durée de grossissement et de rétrécissement de la boule
    public float growDuration = 0.5f;

    // Durée pendant laquelle le laser devient fin
    public float growLaserDuration1 = 0.2f;

    // Durée pendant laquelle le laser devient épais
    public float growLaserDuration2 = 0.3f;

    // Indique si le laser peut actuellement infliger des dégâts
    public bool canDamage = false;

    // Direction dans laquelle le laser est orienté
    private Vector2 direction = Vector2.left;

    private void Start()
    {
        // Au départ, le laser est invisible en hauteur et la boule est invisible
        laser.localScale = new Vector3(laserScaleX, 0f, 1f);
        ball.localScale = Vector3.zero;

        // Lance les deux animations en parallèle
        StartCoroutine(BallAnimation());
        StartCoroutine(LaserAnim());
    }

    private IEnumerator BallAnimation()
    {
        float t = 0f;

        // La boule grossit progressivement
        while (t < growDuration)
        {
            t += Time.deltaTime;
            float ratio = t / growDuration;

            float scale = Mathf.Lerp(0f, ballScale, ratio);
            ball.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        ball.localScale = new Vector3(ballScale, ballScale, 1f);

        // La boule reste visible un court moment
        yield return new WaitForSeconds(1f);

        t = 0f;

        // La boule rétrécit progressivement
        while (t < growDuration)
        {
            t += Time.deltaTime;
            float ratio = t / growDuration;

            float scale = Mathf.Lerp(ballScale, 0f, ratio);
            ball.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        // La boule disparaît complètement, puis l'objet laser est détruit
        ball.localScale = Vector3.zero;
        Destroy(gameObject);
    }

    private IEnumerator LaserAnim()
    {
        // Le laser commence un peu après l'apparition de la boule
        yield return new WaitForSeconds(0.2f);

        float t = 0f;

        // Première phase : laser fin, utilisé comme avertissement visuel
        while (t < growLaserDuration1)
        {
            t += Time.deltaTime;
            float ratio = t / growLaserDuration1;

            float y = Mathf.Lerp(0f, laserScaleY / 10f, ratio);
            laser.localScale = new Vector3(laserScaleX, y, 1f);

            yield return null;
        }

        // Petite attente avant que le laser devienne dangereux
        yield return new WaitForSeconds(0.7f);

        t = 0f;

        // À partir de cette phase, le laser peut infliger des dégâts
        canDamage = true;

        // Deuxième phase : le laser devient plus épais
        while (t < growLaserDuration2)
        {
            t += Time.deltaTime;
            float ratio = t / growLaserDuration2;

            float y = Mathf.Lerp(laserScaleY / 10f, laserScaleY, ratio);
            laser.localScale = new Vector3(laserScaleX, y, 1f);

            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        t = 0f;

        // Dernière phase : le laser disparaît progressivement
        while (t < growLaserDuration2 + growLaserDuration1)
        {
            t += Time.deltaTime;
            float ratio = t / (growLaserDuration2 + growLaserDuration1);

            float y = Mathf.Lerp(laserScaleY, 0f, ratio);
            laser.localScale = new Vector3(laserScaleX, y, 1f);

            yield return null;
        }

        // Une fois disparu, il ne peut plus faire de dégâts
        canDamage = false;
        laser.localScale = new Vector3(laserScaleX, 0f, 1f);
    }

    public void SetDirection(Vector2 newDirection)
    {
        // Normalise la direction pour garder une orientation correcte
        direction = newDirection.normalized;

        // Calcule l'angle correspondant à la direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Oriente le laser dans la bonne direction
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 180);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Le laser inflige des dégâts seulement pendant sa phase dangereuse
        if (canDamage && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage(damage);

            // Empêche le laser d'infliger plusieurs fois les dégâts pendant la même attaque
            canDamage = false;
        }
    }
}