using UnityEngine;
using System.Collections;

public class TrackingMissile : MonoBehaviour
{
    public float speed = 8f;

    public GameObject playerObject;

    [Header("Tracking")]
    public float rotationSpeed = 2f;
    public float trackingDuration = 1.5f;
    public float pauseDuration = 0.5f;

    private Transform player;
    private Vector2 direction;

    private enum MissileState
    {
        Tracking,
        Pause,
        Straight
    }

    private MissileState state = MissileState.Tracking;

    void Start()
    {

        if (playerObject != null)
            player = playerObject.transform;

        StartCoroutine(MissileRoutine());

        Destroy(gameObject, 8f);
    }

    void Update()
    {
        if (state == MissileState.Tracking)
        {
            TrackPlayer();
            Move();
        }
        else if (state == MissileState.Straight)
        {
            Move();
        }
    }

    IEnumerator MissileRoutine()
    {
        state = MissileState.Tracking;

        yield return new WaitForSeconds(trackingDuration);

        state = MissileState.Pause;

        yield return new WaitForSeconds(pauseDuration);

        state = MissileState.Straight;
        speed *= 2f;
    }

    void Move()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void TrackPlayer()
    {
        if (player == null)
            return;

        Vector2 targetDirection =
            ((Vector2)player.position - (Vector2)transform.position).normalized;

        float maxRadiansDelta =
            rotationSpeed * Mathf.Deg2Rad * Time.deltaTime;

        direction = Vector3.RotateTowards(
            direction,
            targetDirection,
            maxRadiansDelta,
            0f
        ).normalized;

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(0f, 0f, angle);
    }
}