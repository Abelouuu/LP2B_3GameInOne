using UnityEngine;
using System.Collections;

public class RamEnemy : EnemyBase
{
    public float enterSpeed;
    public float chargeSpeed;
    public float pauseDuration;

    public float visibleXPosition;
    public float destroyXPosition;

    [Header("Specific Audio")]
    public AudioClip chargeSound;

    protected override IEnumerator Behavior()
    {
        while (transform.position.x > visibleXPosition)
        {
            transform.position += Vector3.left * enterSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(pauseDuration);

        PlaySound(chargeSound);

        while (transform.position.x > destroyXPosition)
        {
            transform.position += Vector3.left * chargeSpeed * Time.deltaTime;
            yield return null;
        }

        DestroySelf();
    }
}