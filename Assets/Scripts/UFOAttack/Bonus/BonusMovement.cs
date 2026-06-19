using UnityEngine;

public class BonusMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float destroyX = -12f;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }
}