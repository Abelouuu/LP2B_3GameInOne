using UnityEngine;
using System.Collections;

public class LaserShooter : EnemyBase
{
    // Préfab du laser tiré par l'ennemi
    public GameObject laserPrefab;

    // Vitesse d'entrée de l'ennemi vers la gauche
    public float horizontalSpeed = 5f;

    // Vitesse de déplacement vertical après son arrivée
    public float verticalSpeed = 1f;

    [Header("Specific Audio")]
    public AudioClip laserSound;

    protected override IEnumerator Behavior()
    {
        // L'ennemi avance vers la gauche jusqu'à atteindre sa position de tir
        while (transform.position.x > 5.4f)
        {
            transform.position += Vector3.left * Time.deltaTime * horizontalSpeed;
            yield return null;
        }

        // Premier tir de laser dès que l'ennemi est placé
        ShootLaser();

        yield return new WaitForSeconds(2f);

        while (true)
        {
            // Durée aléatoire pendant laquelle l'ennemi va se déplacer verticalement
            float moveDuration = Random.Range(1f, 3f);
            float t = 0f;

            while (t < moveDuration)
            {
                // Changement de direction quand l'ennemi atteint les limites verticales
                if (transform.position.y > 2.70f)
                {
                    verticalSpeed = -Mathf.Abs(verticalSpeed);
                }
                else if (transform.position.y < -3.7f)
                {
                    verticalSpeed = Mathf.Abs(verticalSpeed);
                }

                t += Time.deltaTime;

                // Déplacement vertical de l'ennemi
                transform.position += Vector3.up * verticalSpeed * Time.deltaTime;

                yield return null;
            }

            // L'ennemi retire un laser après son déplacement
            ShootLaser();

            // Pause avant le prochain déplacement et le prochain tir
            yield return new WaitForSeconds(3f);
        }
    }

    private void ShootLaser()
    {
        // Son du tir laser
        PlaySound(laserSound);

        // Création du laser devant l'ennemi
        // Le laser est enfant de l'ennemi pour suivre sa position et sa rotation
        Instantiate(
            laserPrefab,
            transform.position + new Vector3(-1.4f, 0f, 0f),
            transform.rotation,
            transform
        );
    }
}