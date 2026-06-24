using UnityEngine;
using System.Collections;

public class RamEnemy : EnemyBase
{
    // Vitesse d'entrée de l'ennemi dans l'écran
    public float enterSpeed;

    // Vitesse utilisée pendant la charge rapide
    public float chargeSpeed;

    // Temps d'attente avant que l'ennemi charge
    public float pauseDuration;

    // Position X où l'ennemi s'arrête avant de charger
    public float visibleXPosition;

    // Position X à partir de laquelle l'ennemi est détruit
    public float destroyXPosition;

    [Header("Specific Audio")]
    public AudioClip chargeSound;

    protected override IEnumerator Behavior()
    {
        // L'ennemi entre lentement dans l'écran jusqu'à sa position visible
        while (transform.position.x > visibleXPosition)
        {
            transform.position += Vector3.left * enterSpeed * Time.deltaTime;
            yield return null;
        }

        // Petite pause pour laisser le joueur réagir
        yield return new WaitForSeconds(pauseDuration);

        // Joue le son de charge
        PlaySound(chargeSound);

        // L'ennemi charge rapidement vers la gauche
        while (transform.position.x > destroyXPosition)
        {
            transform.position += Vector3.left * chargeSpeed * Time.deltaTime;
            yield return null;
        }

        // Une fois sorti de l'écran, l'ennemi se détruit
        DestroySelf();
    }
}