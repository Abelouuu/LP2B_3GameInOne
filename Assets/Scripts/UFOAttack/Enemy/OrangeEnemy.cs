using UnityEngine;
using System.Collections;

public class OrangeEnemy : EnemyBase
{
    public GameObject playerObject;
    public GameObject trackingMissilePrefab;

    public float enterSpeed = 3f;
    public float exitSpeed = 5f;

    [Header("Specific Audio")]
    public AudioClip missileSound;

    protected override IEnumerator Behavior()
    {
        while (transform.position.x > 6f)
        {
            transform.Translate(Vector3.left * Time.deltaTime * enterSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        Shoot();

        while (transform.position.x < 10f)
        {
            transform.Translate(Vector3.right * Time.deltaTime * exitSpeed);
            yield return null;
        }

        DestroySelf();
    }

    private void Shoot()
    {
        PlaySound(missileSound);

        GameObject trackingMissile1 = Instantiate(
            trackingMissilePrefab,
            transform.position + new Vector3(0f, 0.2f, 0f),
            Quaternion.identity
        );

        trackingMissile1.GetComponent<TrackingMissile>().SetDirection(new Vector2(-0.5f, 1f));
        trackingMissile1.GetComponent<TrackingMissile>().playerObject = playerObject;

        GameObject trackingMissile2 = Instantiate(
            trackingMissilePrefab,
            transform.position + new Vector3(0f, -0.2f, 0f),
            Quaternion.identity
        );

        trackingMissile2.GetComponent<TrackingMissile>().SetDirection(new Vector2(-0.5f, -1f));
        trackingMissile2.GetComponent<TrackingMissile>().playerObject = playerObject;
    }
}