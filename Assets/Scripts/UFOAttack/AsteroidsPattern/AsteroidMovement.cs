using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public float rotationSpeed;
    public float movementSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        transform.position += Vector3.left * movementSpeed * Time.deltaTime;

        if(transform.position.x < -12f)
        {
            Destroy(gameObject);
        }
    }
}
