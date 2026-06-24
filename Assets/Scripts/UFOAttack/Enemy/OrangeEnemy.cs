using UnityEngine;
using System.Collections;

public class OrangeEnemy : EnemyBase
{
    // Référence vers le joueur, utilisée par les missiles à tête chercheuse
    public GameObject playerObject;

    // Préfab du missile qui va suivre le joueur
    public GameObject trackingMissilePrefab;

    // Vitesse d'entrée de l'ennemi dans l'écran
    public float enterSpeed = 3f;

    // Vitesse de sortie après avoir tiré
    public float exitSpeed = 5f;

    [Header("Specific Audio")]
    public AudioClip missileSound;

    protected override IEnumerator Behavior()
    {
        // L'ennemi entre dans l'écran jusqu'à atteindre une position fixe
        while (transform.position.x > 6f)
        {
            transform.Translate(Vector3.left * Time.deltaTime * enterSpeed);
            yield return null;
        }

        // Petite pause avant le tir
        yield return new WaitForSeconds(2f);

        // L'ennemi tire ses missiles
        Shoot();

        // Après le tir, l'ennemi repart vers la droite
        while (transform.position.x < 10f)
        {
            transform.Translate(Vector3.right * Time.deltaTime * exitSpeed);
            yield return null;
        }

        // Une fois hors écran, l'ennemi se détruit
        DestroySelf();
    }

    private void Shoot()
    {
        // Son du tir de missile
        PlaySound(missileSound);

        // Premier missile, tiré légèrement vers le haut
        GameObject trackingMissile1 = Instantiate(
            trackingMissilePrefab,
            transform.position + new Vector3(0f, 0.2f, 0f),
            Quaternion.identity
        );

        trackingMissile1.GetComponent<TrackingMissile>().SetDirection(new Vector2(-0.5f, 1f));
        trackingMissile1.GetComponent<TrackingMissile>().playerObject = playerObject;

        // Deuxième missile, tiré légèrement vers le bas
        GameObject trackingMissile2 = Instantiate(
            trackingMissilePrefab,
            transform.position + new Vector3(0f, -0.2f, 0f),
            Quaternion.identity
        );

        trackingMissile2.GetComponent<TrackingMissile>().SetDirection(new Vector2(-0.5f, -1f));
        trackingMissile2.GetComponent<TrackingMissile>().playerObject = playerObject;
    }
}